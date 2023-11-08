using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyController, IAttackable, IDamageable
{
    protected FSM<EnemyController> _stateMachine;

    FieldOfView _fov;
    //public LayerMask _targetMask;
    //public Transform _target;
    //public float _viewRadius;
    public float _attackRange;
    public Transform Target => _fov?.NearestTarget;

    public Transform[] _wayPoints;
    [HideInInspector]
    public Transform _targetWayPoint = null;
    int _wayPointIndex = 0;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());
        _stateMachine.AddState(new DeadState());
    }

    public AttackBehaviour CurrentAttackBehaviour { get; private set; }

    public void OnExcuteAttack(int attackIndex)
    {

    }
}
