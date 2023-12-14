using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSleepState : BossBaseState
{
    public override void Enter(DragonController target) { }

    public override void Execute(DragonController target)
    {
        float distance = Vector3.Distance(target.transform.position, target._target.position);
        
        //일정 범위 안일때
        if (distance < target.wakeupDistance)
        {
            //깨어남
            target.ChangeState(BossStateEnum.Idle);
        }
    }

    public override void Exit(DragonController target)
    {
        target.Anim.SetTrigger("Awake");
        target.bossHpBar.gameObject.SetActive(true);
    }
}
