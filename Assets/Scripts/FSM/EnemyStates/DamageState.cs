using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : EnemyState
{
    public override void Enter(EnemyController target)
    {
        target.StartCoroutine(Coroutine_Damage(target));
    }

    private IEnumerator Coroutine_Damage(EnemyController target)
    {
        target.Anim.SetTrigger("HitTrigger");

        target.StartCoroutine(Coroutine_OnHitColor(target));

        yield return new WaitForSeconds(target._damageDelayTime);

        target.ChangeState(EEnemyState.Idle);
    }

    private IEnumerator Coroutine_OnHitColor(EnemyController target)
    {
        target.MeshRenderer.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        target.MeshRenderer.material.color = target.OriginColor;
    }

    public override void Execute(EnemyController target) { }

    public override void Exit(EnemyController target) { }
}
