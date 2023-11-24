using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : EnemyState
{
    public override void Enter(EnemyController target)
    {
        target.Die();
    }

    public override void Execute(EnemyController target) { }

    public override void Exit(EnemyController target) { }
}