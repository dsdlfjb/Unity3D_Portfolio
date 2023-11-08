using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State<EnemyController>
{
    Animator _anim;

    protected int _isAliveHash = Animator.StringToHash("IsAlive");

    public override void OnInitialized()
    {
        _anim = _context.GetComponent<Animator>();
    }

    public override void OnEnter()
    {
        _anim?.SetBool(_isAliveHash, false);
    }

    public override void Update(float deltaTime)
    {
        if (_stateMachine.ElapsedTimeInState > 3f)
        {
            GameObject.Destroy(_context.gameObject);
        }
    }

    public override void OnExit() { }
}
