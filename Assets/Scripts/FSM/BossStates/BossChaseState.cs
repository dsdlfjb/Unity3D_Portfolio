using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BossBaseState
{
    public override void Enter(DragonController target)
    {
        target.Anim.SetBool("IsChasing", true);
        if (!target.Agent.enabled)
        {
            target.Agent.enabled = true;
            
        }
    }

    public override void Execute(DragonController target)
    {
        target.transform.LookAt(new Vector3(target._target.position.x, target.transform.position.y , target._target.position.z));
        // 만약 소리 치는 상태가 아니라면
        if (!target.Anim.GetCurrentAnimatorStateInfo(0).IsName("Scream"))
        {
            // 공격상태로 변경
            if (target.distance < target.allAttackDistance)
            {
                target.ChangeState(BossStateEnum.Attack);
            }
        
            target.Agent.destination = target._target.position;
        }
    }

    public override void Exit(DragonController target)
    {
        
    }
}
