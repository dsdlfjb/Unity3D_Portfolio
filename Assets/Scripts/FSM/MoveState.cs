using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : EnemyState
{
    public override void Enter(EnemyController target) { }

    public override void Execute(EnemyController target) 
    {
        target.Move();
    }

    public override void Exit(EnemyController target) { }
}