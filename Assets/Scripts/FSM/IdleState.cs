using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : EnemyState
{
    public override void Enter(EnemyController target)
    {
        target.Idle();
    }

    public override void Execute(EnemyController target) { }

    public override void Exit(EnemyController target) { }
}
