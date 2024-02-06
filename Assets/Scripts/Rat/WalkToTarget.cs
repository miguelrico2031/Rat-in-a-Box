using Pathfinding;
using UnityEngine;


public class WalkToTarget : IRatState
{
    private RatController _controller;
    private Vector2 _target;
    private Path _path;

    private Vector2 _direction;
    private string _currentAnimation;
    
    public void Enter(RatController controller, IRatState previousState = null)
    {
        _controller = controller;
        _target = controller.CurrentTarget.GetTarget(controller.transform.position);
        controller.AIPath.enabled = true;
        _path = controller.Seeker.StartPath(controller.transform.position, _target);

        controller.StartCoroutine(controller.CheckForItems());
    }


    public void Update()
    {
        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        var newDirection = (Vector2)_controller.AIPath.desiredVelocity;
        if (newDirection == _direction) return;

        _direction = newDirection;

        _controller.Renderer.flipX = _direction.x > 0f;

        if (_direction.y < 0f && _currentAnimation != "Move Front")
        {
            _controller.Animator.Play("Move Front");
            _currentAnimation = "Move Front";
        }
        
        if (_direction.y >= 0f && _currentAnimation != "Move Back")
        {
            _controller.Animator.Play("Move Back");
            _currentAnimation = "Move Back";
        }
    }

    public void Exit(IRatState nextState = null)
    {
        _controller.AIPath.enabled = false;
    }
}
