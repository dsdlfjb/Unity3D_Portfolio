public interface IState<T>
{
    public void Enter(T target);

    public void Execute(T target);

    public void Exit(T target);
}