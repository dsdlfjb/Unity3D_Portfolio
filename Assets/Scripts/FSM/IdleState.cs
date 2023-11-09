using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State<EnemyController>
{
    public bool _isPatrol = true;
    float _minIdleTime = 0f;
    float _maxIdleTime = 3f;
    float _idleTime = 0f;

    Animator _anim;
    CharacterController _controller;

    protected int _hashMove = Animator.StringToHash("IsMove");
    protected int _hashMoveSpeed = Animator.StringToHash("Speed");

    public override void OnInitialized()
    {
        _anim = _context.GetComponent<Animator>();
        _controller = _context.GetComponent<CharacterController>();
    }

    public override void OnEnter()
    {
        _anim?.SetBool(_hashMove, false);
        _anim?.SetFloat(_hashMoveSpeed, 0);
        _controller?.Move(Vector3.zero);

        if (_context is EnemyController_Patrol)
        {
            _isPatrol = true;
            _idleTime = Random.Range(_minIdleTime, _maxIdleTime);
        }
    }

    public override void Update(float deltaTime)
    {
        if (_context.Target)
        {
            if (_context.IsAvailableAttack)
                _stateMachine.ChangeState<AttackState>();

            else
                _stateMachine.ChangeState<MoveState>();
        }

        else if (_isPatrol && _stateMachine.ElapsedTimeInState > _idleTime)
            _stateMachine.ChangeState<PatrolState>();
    }

    public override void OnExit() { }
}
