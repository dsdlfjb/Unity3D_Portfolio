using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    public int _damage = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            transform.parent = other.transform;
            other.GetComponent<DragonController>().TakeDamage(_damage);
        }
    }
}