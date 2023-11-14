using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveState : State<EnemyController>
{
    Animator _anim;
    CharacterController _controller;
    NavMeshAgent _agent;

    int _isMoveHash = Animator.StringToHash("IsMove");
    int _moveSpeedHash = Animator.StringToHash("MoveSpeed");

    AttackStateController _attackStateController;

    public override void OnInitialized()
    {
        _anim = _context.GetComponent<Animator>();
        _controller = _context.GetComponent<CharacterController>();
        _agent = _context.GetComponent<NavMeshAgent>();

        _attackStateController = _context.GetComponent<AttackStateController>();
    }

    public override void OnEnter()
    {
        _agent.stoppingDistance = _context.AttackRange - 0.5f;
        _agent?.SetDestination(_context.Target.position);

        _anim?.SetBool(_isMoveHash, true);
    }

    public override void Update(float deltaTime)
    {
        if (_attackStateController.IsInAttackState == false)
        {
            if (_context.Target)
            {
                if (_anim.GetBool("IsMove"))
                {
                    _agent.SetDestination(_context.Target.position);
                    _controller.Move(_agent.velocity * Time.deltaTime);
                }

                else
                    _agent.ResetPath();
            }
        }

        if (_agent.remainingDistance > _agent.stoppingDistance)
            _anim.SetFloat(_moveSpeedHash, _agent.velocity.magnitude / _agent.speed, .1f, Time.deltaTime);

        else
        {
                _anim.SetFloat(_moveSpeedHash, 0);
                _anim.SetBool(_isMoveHash, false);
                _agent.ResetPath();

                _stateMachine.ChangeState<IdleState>();
        }
    }

    public override void OnExit()
    {
        _agent.stoppingDistance = 0f;
        _agent.ResetPath();

        _anim?.SetBool(_isMoveHash, false);
        _anim?.SetFloat(_moveSpeedHash, 0f);
    }
}
