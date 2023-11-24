// ±âº» EnemyState

public abstract class EnemyState : IState<EnemyController>
{
    public virtual void Enter(EnemyController target) { }

    public virtual void Execute(EnemyController target) { }

    public virtual void Exit(EnemyController target) { }
}
