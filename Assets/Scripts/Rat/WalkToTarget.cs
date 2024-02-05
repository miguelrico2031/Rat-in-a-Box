using Pathfinding;
using UnityEngine;

namespace Rat
{
    public class WalkToTarget : IRatState
    {
        private RatController _controller;
        private Transform _target;
        private Path _path;

        private Vector2 _direction;
        private string _currentAnimation;
        
        public void Enter(RatController controller, IRatState previousState = null)
        {
            _controller = controller;
            _target = _controller.CurrentTarget.transform;

            _path = _controller.Seeker.StartPath(controller.transform.position, _target.position);
            
        }


        public void Update()
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
            
        }
    }
}