using UnityEngine;

public class Idle : IRatState
{
    public Vector2 Direction { get; private set; } 
    
    public void Enter(RatController controller, IRatState previousState = null)
    {
        Direction = previousState != null ? previousState.Direction : new(-1, -1);
        controller.PlayLoopingAnimationXY("Idle", Direction);
    }

    public void Update()
    {
        
    }

    public void Exit(IRatState nextState = null)
    {
        
    }
}
