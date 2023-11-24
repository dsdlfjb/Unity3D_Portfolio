using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : EnemyState
{
    public override void Enter(EnemyController target)
    {
        
    }

    public override void Execute(EnemyController target) 
    {
        target.Attack();
    }

    public override void Exit(EnemyController target) { }
}