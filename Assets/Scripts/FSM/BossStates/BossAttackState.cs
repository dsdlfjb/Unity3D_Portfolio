using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    public override void Enter(DragonController target)
    {
        // 우선 계속 쫒아다닐 수 있게
        target.Agent.enabled = true;
    }

    public override void Execute(DragonController target)
    {
        // 만약 전체 공격범위를 벗어날경우
        if (target.distance > target.allAttackDistance)
        {
            target.ChangeState(BossStateEnum.Chase);
            return;
        }
        
        // 공격하는 애니메이션이 아닐때만
        if (!(target.Anim.GetCurrentAnimatorStateInfo(0).IsName("Mouth Attack")||
              target.Anim.GetCurrentAnimatorStateInfo(0).IsName("Flame Attack")))
        {
            target.Agent.enabled = true;
            target.Agent.destination = target._target.position;
        }
        else
        {
            target.Agent.enabled = false;
            //target.Agent.destination = target.transform.position;
            return;
        }
        
        // 급접해있다면
        if (target.distance < target.mouthAttackDistance)
        {
            target.Agent.enabled = false;
            //target.Agent.destination = target.transform.position;
        }
        else
        {
            target.Agent.enabled = true;
            target.Agent.destination = target._target.position;
        }

        target._attackTimer += Time.deltaTime;
        if (target._attackTimer > target._maxAttackTime)
        {
            // 특정 애니메이션이 아닐때만 작동
            if (!(target.Anim.GetCurrentAnimatorStateInfo(0).IsName("Mouth Attack") || target.Anim.GetCurrentAnimatorStateInfo(0).IsName("Flame Attack")))
            {
                target.transform.LookAt(new Vector3(target._target.position.x, target.transform.position.y , target._target.position.z));
                //근접
                if (target.distance < target.mouthAttackDistance)
                {
                    // 근접 공격
                    target.Anim.SetTrigger("MouthAttack");
                }
                //원거리
                else if (target.distance > target.mouthAttackDistance && target.distance < target.breatheAttackDistance)
                {
                    // 원거리 공격
                    target.Anim.SetTrigger("FlameAttack");
                }

                target._attackTimer = 0;
                return;
            }
        }
        
        
    }

    public override void Exit(DragonController target)
    {
        
    }
}
