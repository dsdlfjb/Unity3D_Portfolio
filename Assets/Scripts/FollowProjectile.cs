using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowProjectile : Projectile
{
    public float _destroyDelay = 5f;

    protected override void Start()
    {
        base.Start();

        StartCoroutine(Coroutine_DestroyParticle(_destroyDelay));
    }

    protected override void FixedUpdate()
    {
        if (_target)
        {
            Vector3 dest = _target.transform.position;
            dest.y += 1.5f;
            transform.LookAt(dest);
        }

        base.FixedUpdate();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
}
