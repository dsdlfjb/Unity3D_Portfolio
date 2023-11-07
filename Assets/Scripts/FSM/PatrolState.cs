using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State<EnemyController>
{
    Animator _anim;
    CharacterController _controller;
    NavMeshAgent _agent;

    protected int _hashMove = Animator.StringToHash("IsMove");
    protected int _hashMoveSpeed = Animator.StringToHash("Speed");

    public override void OnInitialized()
    {
        _anim = _context.GetComponent<Animator>();
        _controller = _context.GetComponent<CharacterController>();
        _agent = _context.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        if (_context._targetWayPoint == null)
        {
            _context.FindNextWayPoint();
        }

        if (_context._targetWayPoint)
        {
            _agent?.SetDestination(_context._targetWayPoint.position);
            _anim?.SetBool(_hashMove, true);
        }
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

        else
        {
            if (!_agent.pathPending && (_agent.remainingDistance <= _agent.stoppingDistance))
            {
                Transform nextDist = _context.FindNextWayPoint();

                if (nextDist)
                    _agent.SetDestination(nextDist.position);

                _stateMachine.ChangeState<IdleState>();
            }

            else
            {
                _controller.Move(_agent.velocity * deltaTime);
                _anim.SetFloat(_hashMoveSpeed, _agent.velocity.magnitude / _agent.speed, .1f, deltaTime);
            }
        }
    }

    public override void OnExit()
    {
        _anim?.SetBool(_hashMove, false);
        _agent.ResetPath();
    }
}
