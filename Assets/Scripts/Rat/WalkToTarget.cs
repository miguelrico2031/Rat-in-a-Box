using Pathfinding;
using UnityEngine;

namespace Rat
{
    public class WalkToTarget : IRatState
    {
        private RatController _controller;
        private Transform _target;
        private Path _path;
        
        public void Enter(RatController controller, IRatState previousState = null)
        {
            _controller = controller;
            _target = _controller.CurrentTarget.transform;

            _path = _controller.Seeker.StartPath(controller.transform.position, _target.position);
            
        }


        public void Update()
        {
        }

        public void Exit(IRatState nextState = null)
        {
            
        }
    }
}