using UnityEngine;

public interface IRatState
{
    public Vector2 Direction { get; }
    public void Enter(RatController controller, IRatState previousState = null);
    public void Update();
    public void Exit(IRatState nextState = null);
}
