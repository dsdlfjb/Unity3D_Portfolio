using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float _speed;
    public GameObject _muzzlePrefab;
    public GameObject _hitPrefab;
    public AudioClip _shotSFX;
    public AudioClip _hitSFX;

    bool _isCollided;
    Rigidbody _rb;

    [HideInInspector]
    public AttackBehaviour _attackBehaviour;
    [HideInInspector]
    public GameObject _owner;
    [HideInInspector]
    public GameObject _target;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        if (_owner != null)
        {
            Collider projectileCollider = GetComponent<Collider>();
            Collider[] ownerColliders = _owner.GetComponentsInChildren<Collider>();

            foreach (Collider collider in ownerColliders)
                Physics.IgnoreCollision(projectileCollider, collider);
        }

        _rb = GetComponent<Rigidbody>();

        if (_muzzlePrefab != null)
        {
            var muzzleVFX = Instantiate(_muzzlePrefab, transform.position, Quaternion.identity);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            ParticleSystem particleSystem = muzzleVFX.GetComponent<ParticleSystem>();

            if (particleSystem != null)
                Destroy(muzzleVFX, particleSystem.main.duration);

            else
            {
                ParticleSystem childParticleSystem = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, childParticleSystem.main.duration);
            }
        }

        if (_shotSFX != null && GetComponent<AudioSource>())
            GetComponent<AudioSource>().PlayOneShot(_shotSFX);

        if (_target != null)
        {
            Vector3 dest = _target.transform.position;
            dest.y += 1.5f;
            transform.LookAt(dest);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (_speed != 0 && _rb != null)
            _rb.position += (transform.forward) * (_speed * Time.deltaTime);
    }

    protected virtual void OnCollisionEnter(Collision collision)
    {
        if (_isCollided) return;

        Collider projectileCollider = GetComponent<Collider>();
        projectileCollider.enabled = false;

        _isCollided = true;

        if (_hitSFX != null && GetComponent<AudioSource>())
            GetComponent<AudioSource>().PlayOneShot(_hitSFX);

        _speed = 0;
        _rb.isKinematic = true;

        ContactPoint contact = collision.contacts[0];
        Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 contactPosition = contact.point;

        if (_hitPrefab != null)
        {
            var hitVFX = Instantiate(_hitPrefab, contactPosition, contactRotation) as GameObject;

            ParticleSystem particleSystem = hitVFX.GetComponent<ParticleSystem>();

            if (particleSystem == null)
            {
                ParticleSystem childParticleSystem = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitVFX, childParticleSystem.main.duration);
            }

            else
                Destroy(hitVFX, particleSystem.main.duration);
        }

        IDamageable damagable = collision.gameObject.GetComponent<IDamageable>();

        if (damagable != null)
            damagable.TakeDamage(_attackBehaviour?._damage ?? 0, null);

        StartCoroutine(Coroutine_DestroyParticle(0f));
    }

    public IEnumerator Coroutine_DestroyParticle(float waitTime)
    {
        if (transform.childCount > 0 && waitTime != 0)
        {
            List<Transform> childs = new List<Transform>();

            foreach (Transform t in transform.GetChild(0).transform)
                childs.Add(t);

            while (transform.GetChild(0).localScale.x > 0)
            {
                yield return new WaitForSeconds(0.01f);
                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);

                for (int i = 0; i < childs.Count; i++)
                    childs[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
            }
        }

        yield return new WaitForSeconds(waitTime);
        Destroy(gameObject);
    }
}
