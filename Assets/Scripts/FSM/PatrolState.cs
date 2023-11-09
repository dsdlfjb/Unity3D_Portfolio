using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolState : State<EnemyController>
{
    Animator _anim;
    CharacterController _controller;
    NavMeshAgent _agent;

    EnemyController_Patrol _patrolController;

    Transform _targetWaypoint = null;
    int _waypointIndex = 0;

    protected int _isHashMove = Animator.StringToHash("IsMove");
    protected int _moveSpeedHash = Animator.StringToHash("MoveSpeed");

    Transform[] WayPoints => ((EnemyController_Patrol)_context)?._waypoints;

    public override void OnInitialized()
    {
        _anim = _context.GetComponent<Animator>();
        _controller = _context.GetComponent<CharacterController>();
        _agent = _context.GetComponent<NavMeshAgent>();

        _patrolController = _context as EnemyController_Patrol;
    }

    public override void OnEnter()
    {
        _agent.stoppingDistance = 0f;

        if (_targetWaypoint == null)
        {
            FindNextWaypoint();
        }

        if (_targetWaypoint)
        {
            _agent?.SetDestination(_targetWaypoint.position);
            _anim?.SetBool(_isHashMove, true);
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
        
        else
        {
            if (!_agent.pathPending && (_agent.remainingDistance <= _agent.stoppingDistance))
            {
                FindNextWaypoint();
                _stateMachine.ChangeState<IdleState>();
            }

            else
            {
                _controller.Move(_agent.velocity * Time.deltaTime);
                _anim.SetFloat(_moveSpeedHash, _agent.velocity.magnitude / _agent.speed, .1f, Time.deltaTime);
            }
        }
    }

    public override void OnExit()
    {
        _agent.stoppingDistance = _context.AttackRange;
        _anim?.SetBool(_isHashMove, false);
        _agent.ResetPath();
    }

    public Transform FindNextWaypoint()
    {
        _targetWaypoint = null;

        if (WayPoints != null && WayPoints.Length > 0)
        {
            _targetWaypoint = WayPoints[_waypointIndex];

            _waypointIndex = (_waypointIndex + 1) % WayPoints.Length;
        }

        return _targetWaypoint;
    }
}
