using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageState : EnemyState
{
    public override void Enter(EnemyController target) { }

    public override void Execute(EnemyController target) 
    {
        target.Damage();
    }

    public override void Exit(EnemyController target) { }
}