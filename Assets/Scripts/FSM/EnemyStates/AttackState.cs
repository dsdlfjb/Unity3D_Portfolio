using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    public override void Enter(EnemyController target)
    {
        target.Agent.enabled = false;
    }

    public override void Execute(EnemyController target) 
    {
        target.Agent.enabled = false;
        
        target._curTime += Time.deltaTime;
        Vector3 dir = target._target.position - target.transform.position;
        float distance = dir.magnitude;
        
        
        if (distance > target._attackRange)
        {
            target.ChangeState(EEnemyState.Move);
            return;
        }
        
        if (target._curTime > target._attackDelayTime)
        {
            target.transform.LookAt(new Vector3(target._target.position.x, target.transform.position.y , target._target.position.z));
            target.Attack();
        }
    }

    public override void Exit(EnemyController target) { }
}