using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreatheAttacks : MonoBehaviour
{
    public int _damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.TakeDamage(_damage);
        }
    }
}
