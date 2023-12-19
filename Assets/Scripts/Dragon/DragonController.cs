using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public int _hp = 1000;

    Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damageAmount)
    {
        _hp -= damageAmount;

        if (_hp <= 0)
        {
            _anim.SetTrigger("Die");
            GetComponent<Collider>().enabled = false;
        }

        else
        {
            _anim.SetTrigger("Hit");
        }
    }
}
