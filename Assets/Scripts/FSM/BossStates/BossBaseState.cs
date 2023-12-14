using System;

[Serializable]
public enum BossStateEnum
{
    Sleep,
    Idle,
    Chase,
    Attack,
    Damage,
    Die
}

public abstract class BossBaseState : IState<DragonController>
{
    public virtual void Enter(DragonController target) { }

    public virtual void Execute(DragonController target) { }

    public virtual void Exit(DragonController target) { }
}
