using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<EnemyController>
{
    Animator _anim;
    CharacterController _controller;

    protected int _hasMove = Animator.StringToHash("IsMove");
    protected int _hasMoveSpeed = Animator.StringToHash("Speed");

    public override void OnInitialized()
    {
        _anim = _context.GetComponent<Animator>();
        _controller = _context.GetComponent<CharacterController>();
    }

    public override void OnEnter()
    {
        _anim?.SetBool(_hasMove, false);
        _anim?.SetFloat(_hasMoveSpeed, 0);
        _controller?.Move(Vector3.zero);
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = _context.SearchEnemy();

        if (enemy)
        {
            if (_context.IsAvailableAttack)
                _stateMachine.ChangeState<AttackState>();

            else
                _stateMachine.ChangeState<MoveState>();
        }
    }

    public override void OnExit()
    {

    }
}
