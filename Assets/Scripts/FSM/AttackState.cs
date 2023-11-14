using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    Animator _anim;
    AttackStateController _attackStateController;
    IAttackable _attackable;

    protected int _attackTriggerHash = Animator.StringToHash("AttackTrigger");
    protected int _attackIndexHash = Animator.StringToHash("AttackIndex");

    public override void OnInitialized()
    {
        _anim = _context.GetComponent<Animator>();
        _attackStateController = _context.GetComponent<AttackStateController>();
        _attackable = _context.GetComponent<IAttackable>();
    }

    public override void OnEnter()
    {
        if (_attackable == null || _attackable.CurrentAttackBehaviour == null)
        {
            _stateMachine.ChangeState<IdleState>();
            return;
        }

        _attackStateController.enterAttackStateHandler += OnEnterAttackState;
        _attackStateController.exitAttackStateHandler += OnExitAttackState;

        _anim?.SetInteger(_attackIndexHash, _attackable.CurrentAttackBehaviour._animationIndex);
        _anim?.SetTrigger(_attackTriggerHash);

    }

    public override void Update(float deltaTime) { }

    public override void OnExit()
    {
        _attackStateController.enterAttackStateHandler -= OnEnterAttackState;
        _attackStateController.exitAttackStateHandler -= OnExitAttackState;
    }

    public void OnEnterAttackState()
    {
        Debug.Log("OnEnterAttackState()");
    }

    public void OnExitAttackState()
    {
        Debug.Log("OnExitAttackState()");
        _stateMachine.ChangeState<IdleState>();
    }
}
