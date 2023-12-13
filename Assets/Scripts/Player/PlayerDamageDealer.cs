using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageDealer : MonoBehaviour
{
    private bool canDealDamage;

    public List<GameObject> enemys;
    public LayerMask layer;

    [SerializeField] private float weaponLength;
    [SerializeField] private int weaponDamage;

    private void Update()
    {
        if (canDealDamage)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layer))
            {
                // 적이 부딛혔을때
                if (hit.transform.TryGetComponent(out EnemyController enem) &&
                    !enemys.Contains(hit.transform.gameObject))
                {
                    Debug.Log($"{enem.name} damaged");
                    // 데미지 처리
                    enem.TakeDamage(weaponDamage);
                    enemys.Add(enem.gameObject);
                }
                // 드래곤이 부딛혔을때
                if (hit.transform.TryGetComponent(out DragonController dragon) &&
                    !enemys.Contains(hit.transform.gameObject))
                {
                    Debug.Log($"{dragon.name} damaged");
                    // 데미지 처리
                    dragon.TakeDamage(weaponDamage);
                    enemys.Add(dragon.gameObject);
                }
            }
        }
    }
    
    public void StartDealDamage()
    {
        canDealDamage = true;
        enemys.Clear();
    }

    public void EndDealDamage()
    {
        canDealDamage = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}
