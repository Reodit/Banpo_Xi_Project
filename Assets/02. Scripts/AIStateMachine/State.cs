public abstract class State
{
    /// <summary>
    /// 해당 상태를 시작할 때 1회 호출
    /// </summary>
    public abstract void Enter(Boss_Dragon entity);

    /// <summary>
    /// 해당 상태를 업데이트할 때 매 프레임 호출
    /// </summary>
    public abstract void Execute(Boss_Dragon entity);

    /// <summary>
    /// 해당 상태를 종료할 때 1회 호출.
    /// </summary>
    public abstract void Exit(Boss_Dragon entity);
}

