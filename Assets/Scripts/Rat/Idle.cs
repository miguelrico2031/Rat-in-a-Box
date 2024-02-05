using UnityEngine;

public class Idle : IRatState
{
    public void Enter(RatController controller, IRatState previousState = null)
    {
        controller.Animator.Play("Idle");
    }

    public void Update()
    {
        
    }

    public void Exit(IRatState nextState = null)
    {
        
    }
}
