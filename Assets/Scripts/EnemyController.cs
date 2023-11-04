using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    protected FSM<EnemyController> _stateMachine;

    public LayerMask _targetMask;
    public Transform _target;
    public float _viewRadius;
    public float _attackRange;

    // Start is called before the first frame update
    void Start()
    {
        _stateMachine = new FSM<EnemyController>(this, new IdleState());    
        _stateMachine.AddState(new MoveState());
        _stateMachine.AddState(new AttackState()); 
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
            if (!_target)
                return false;

            float distance = Vector3.Distance(transform.position, _target.position);
            return (distance <= _attackRange);
        }
    }

    public Transform SearchEnemy()
    {
        _target = null;

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, _viewRadius, _targetMask);

        if (targetInViewRadius.Length > 0)
            _target = targetInViewRadius[0].transform;

        return _target;
    }
}
