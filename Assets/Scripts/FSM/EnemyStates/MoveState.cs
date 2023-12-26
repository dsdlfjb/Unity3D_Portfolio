using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : EnemyState
{
    public override void Enter(EnemyController target)
    {
        target.Anim.SetTrigger("Move");
        target.Agent.enabled = true;
    }

    public override void Execute(EnemyController target) 
    {
        target.Agent.enabled = true;
        
        Vector3 dir = target._target.position - target.transform.position;
        float distance = dir.magnitude;
        
        // 타겟이 공격 범위 안에 들어오면 공격 상태로 전환
        if (distance < target._attackRange)
        {
            // 공격 상태로 변경
            target.ChangeState(EEnemyState.Attack);
            target._curTime = target._attackDelayTime;
        
            target.Agent.enabled = false;
            return;
        }
        
        target.Agent.destination = target._target.position;
    }

    public override void Exit(EnemyController target) { }
}