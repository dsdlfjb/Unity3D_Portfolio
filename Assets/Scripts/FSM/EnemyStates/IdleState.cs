using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    public override void Enter(EnemyController target) { }

    public override void Execute(EnemyController target)
    {
        target.Mat.SetFloat("_DissolveAmount", 0);

        target._curTime += Time.deltaTime;
        
        if (target._curTime > target._idleDelayTime)
        {
            // 움직이는 상태로 변경
            target.ChangeState(EEnemyState.Move);
            target._curTime = 0;
        }
    }

    public override void Exit(EnemyController target)
    {
        target._curTime = 0;
    }
}
