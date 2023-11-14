using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyController : MonoBehaviour
{
    protected FSM<EnemyController> _stateMachine;
    public virtual float AttackRange => 3f;

    protected NavMeshAgent _agent;
    protected Animator _anim;

    FieldOfView _fov;

    public Transform Target => _fov.NearestTarget;
    public LayerMask TargetMask => _fov._targetMask;

    protected Vector3 _savePosition;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _stateMachine = new FSM<EnemyController>(this, new IdleState());

        _agent = GetComponent<NavMeshAgent>();
        _agent.updatePosition = false;
        _agent.updateRotation = true;

        _anim = GetComponent<Animator>();
        _fov = GetComponent<FieldOfView>();

        _savePosition = transform.position;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        _stateMachine.Update(Time.deltaTime);

        if (!(_stateMachine.CurrentState is MoveState) && !(_stateMachine.CurrentState is DeadState))
            FaceTarget();
    }

    void FaceTarget()
    {
        if (Target)
        {
            Vector3 dir = (Target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }

    private void OnAnimatorMove()
    {
        if (_anim.GetBool("IsMove") == true)
        {
            Vector3 position = transform.position;
            position.y = _agent.nextPosition.y;

            _anim.rootPosition = position;
            _agent.nextPosition = position;
        }
    }

    public virtual bool IsAvailableAttack => false;

    public R ChangeState<R>() where R : State<EnemyController>
    {
        return _stateMachine.ChangeState<R>();
    }
}
