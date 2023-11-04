using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State<T>
{
    protected FSM<T> _stateMachine;
    protected T _context;

    public State()
    {

    }

    internal void SetMachineAndContext(FSM<T> stateMachine, T context)
    {
        this._stateMachine = stateMachine;
        this._context = context;

        OnInitialized();
    }

    public virtual void OnInitialized()
    {

    }

    public virtual void OnEnter()
    {

    }

    public abstract void Update(float deltaTime);

    public virtual void OnExit()
    {

    }
}

public sealed class FSM<T>      // ���̻� ������ ������ sealed�� �����
{
    T _context;

    State<T> _currentState;
    public State<T> CurrentState => _currentState;

    State<T> _previousState;
    public State<T> PreviousState => _previousState;

    // ���°� ��ȯ�� �Ǿ��� �� ��ȯ ���¿��� ��ŭ �ð��� �귶���� �˱� ���� ����
    float _elapsedTimeInState = 0.0f;
    public float ElapsedTimeInState => _elapsedTimeInState;

    Dictionary<System.Type, State<T>> _states = new Dictionary<System.Type, State<T>>();

    public FSM(T context, State<T> initialState)
    {
        this._context = context;

        AddState(initialState);
        _currentState = initialState;
        _currentState.OnEnter();
    }

    public void AddState(State<T> state)
    {
        state.SetMachineAndContext(this, _context);
        _states[state.GetType()] = state;
    }

    public void Update(float deltaTme)
    {
        _elapsedTimeInState += deltaTme;

        _currentState.Update(deltaTme);
    }

    public R ChangeState<R>() where R : State<T>
    {
        var newType = typeof(R);

        if (_currentState.GetType() == newType)
        {
            return _currentState as R;
        }

        if (_currentState != null)
            _currentState.OnExit();

        _previousState = _currentState;
        _currentState = _states[newType];
        _currentState.OnEnter();
        _elapsedTimeInState = 0.0f;

        return _currentState as R;
    }
}
