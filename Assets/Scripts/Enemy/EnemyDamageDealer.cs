using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
    private bool canDealDamage;
    public GameObject target;

    public LayerMask layer;

    [SerializeField] private float weaponLength;
    [SerializeField] private int weaponDamage;

    void Start()
    {
        canDealDamage = false;
        target = null;
    }

    private void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, (int)layer))
            {
                Debug.Log($"{hit.transform.name} 충돌");
                if (hit.transform.TryGetComponent(out PlayerController player) && !target)
                {
                    //데미지 처리
                    Debug.Log($"{hit.transform.name} Damaged");
                    player.TakeDamage(weaponDamage);
                    target = hit.transform.gameObject;
                }
            }
        }
    }

    public void StartDealDamage()
    {
        canDealDamage = true;
        target = null;
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
        target = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
