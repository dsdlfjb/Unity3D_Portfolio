using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : State<EnemyController>
{
    Animator _anim;
    CharacterController _controller;
    NavMeshAgent _agent;

    int _hashMove = Animator.StringToHash("IsMove");
    int _hasMoveSpeed = Animator.StringToHash("Speed");

    public override void OnInitialized()
    {
        _anim = _context.GetComponent<Animator>();
        _controller = _context.GetComponent<CharacterController>();
        _agent = _context.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        _agent?.SetDestination(_context.Target.position);
        _anim?.SetBool(_hashMove, true);
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = _context.SearchEnemy();

        if (enemy)
        {
            _agent.SetDestination(_context.Target.position);

            if (_agent.remainingDistance > _agent.stoppingDistance)
            {
                _controller.Move(_agent.velocity * deltaTime);
                _anim.SetFloat(_hasMoveSpeed, _agent.velocity.magnitude / _agent.speed, .1f, deltaTime);
                return;
            }
        }

            _stateMachine.ChangeState<IdleState>();
    }

    public override void OnExit()
    {
        _anim?.SetBool(_hashMove, false);
        //_anim?.SetFloat(_hasMoveSpeed, 0f);
        _agent.ResetPath();
    }
}
