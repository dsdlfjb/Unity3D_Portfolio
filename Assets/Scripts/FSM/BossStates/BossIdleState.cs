using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossBaseState
{
    public override void Enter(DragonController target)
    {
        target.ChangeState(BossStateEnum.Chase);
    }

    public override void Execute(DragonController target)
    {
        
    }

    public override void Exit(DragonController target)
    {
        
    }
}
