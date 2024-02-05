

public interface IRatState
{
    public void Init(RatController controller, IRatState previousState = null);
    public void Update();
    public void Exit(IRatState nextState = null);
}
