using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    Animator _anim;

    int _hashAttack = Animator.StringToHash("Attack");

    public override void OnInitialized()
    {
        _anim = _context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        if (_context.IsAvailableAttack)
            _anim?.SetTrigger(_hashAttack);

        else
            _stateMachine.ChangeState<IdleState>();
    }

    public override void Update(float deltaTime)
    {

    }
}
