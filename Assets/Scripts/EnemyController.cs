using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
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
    void Start()
    {
        _stateMachine = new FSM<EnemyController>(this, new PatrolState());
        IdleState idleState = new IdleState();
        idleState._isPatrol = true;
        _stateMachine.AddState(idleState);
        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState());

        _fov = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Update(Time.deltaTime);
    }

    public bool IsAvailableAttack
    {
        get
        {
            if (!Target)
                return false;

            float distance = Vector3.Distance(transform.position, Target.position);
            return (distance <= _attackRange);
        }
    }

    public Transform SearchEnemy()
    {
        return Target;
        /*
        _target = null;

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);

        if (targetInViewRadius.Length > 0)
            _target = targetInViewRadius[0].transform;

        return _target;
        */
    }
    /*
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _viewRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
    */

    public Transform FindNextWayPoint()
    {
        _targetWayPoint = null;

        if (_wayPoints.Length == 0)
            _targetWayPoint = _wayPoints[_wayPointIndex];

        _wayPointIndex = (_wayPointIndex + 1) % _wayPoints.Length;
        
        return _targetWayPoint;
    }
}
