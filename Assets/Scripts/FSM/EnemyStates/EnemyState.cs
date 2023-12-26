// �⺻ EnemyState
using System;

/// <summary>
/// 이 enum에 저장된 순서대로 배열에 넣어주세요! 그래야 에러없이 작동합니다 :)
/// </summary>
[Serializable]
public enum EEnemyState
{
    Idle = 0,
    Move = 1,
    Attack = 2,
    Damage = 3,
    Die = 4
}

public abstract class EnemyState : IState<EnemyController>
{
    public virtual void Enter(EnemyController target) { }

    public virtual void Execute(EnemyController target) { }

    public virtual void Exit(EnemyController target) { }
}
