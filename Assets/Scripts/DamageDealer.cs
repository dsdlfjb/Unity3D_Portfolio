using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    bool _canDealDamage;
    List<GameObject> _hasDealtDamage;

    [SerializeField] float _weaponLength;
    [SerializeField] int _weaponDamage;

    void Start()
    {
        _canDealDamage = false;
        _hasDealtDamage = new List<GameObject>();
    }

    void Update()
    {
        if (_canDealDamage)
        {
            RaycastHit hit;

            int layerMask = 1 << 9;

            if (Physics.Raycast(transform.position, -transform.up, out hit, _weaponLength, layerMask))
            {
                if (hit.transform.TryGetComponent(out EnemyController enemy) && !_hasDealtDamage.Contains(hit.transform.gameObject))
                {
                    print("Damage");
                    enemy.TakeDamage(_weaponDamage);
                    enemy.HitVFX(hit.point);
                    _hasDealtDamage.Add(hit.transform.gameObject);
                }
            }
        }
    }

    public void StartDealDamage()
    {
        _canDealDamage = true;
        _hasDealtDamage.Clear();
    }

    public void EndDealDamage()
    {
        _canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * _weaponLength);
    }
}