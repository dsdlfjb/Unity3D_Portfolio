using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMouthAttack : MonoBehaviour
{
    public int _mouthAttack;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerController player))
        {
            player.TakeDamage(_mouthAttack);
        }
    }
}
