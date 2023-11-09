using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttackBehaviour : AttackBehaviour
{
    public override void ExecuteAttack(GameObject target = null, Transform startPoint = null)
    {
        if (target == null) return;

        Vector3 projectilePosition = startPoint?.position ?? transform.position;

        if (_effectPrefab != null)
        {
            GameObject projectileGO = GameObject.Instantiate<GameObject>(_effectPrefab, projectilePosition, Quaternion.identity);
            Projectile projectile = projectileGO.GetComponent<Projectile>();

            if (projectile != null)
            {
                projectile._owner = this.gameObject;
                projectile._target = target;
                projectile._attackBehaviour = this;
            }
        }

        _calCoolTime = 0f;
    }
}
