using UnityEngine;

public class Idle : IRatState
{
    public Vector2 Direction { get; private set; } 
    
    public void Enter(RatController controller, IRatState previousState = null)
    {
        if (previousState == null)
        {
            controller.Animator.Play("Idle Front");
            return;
        }

        Direction = previousState.Direction;
        controller.Renderer.flipX = Direction.x < 0f;
        controller.Animator.Play(Direction.y < 0f ? "Idle Front" : "Idle Back");
        
    }

    public void Update()
    {
        
    }

    public void Exit(IRatState nextState = null)
    {
        
    }
}
