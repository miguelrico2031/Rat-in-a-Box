using Pathfinding;
using UnityEngine;


public class WalkToDestination : IRatState
{
    public Vector2 Direction { get; private set; }
    
    private RatController _controller;
    private Vector2 _destination;
    private Path _path;

    private string _currentAnimation;
    private int randomIndex;
    private string soundName;
    private int pasoSonido = 0;
    private const int maxSonido= 100; 
    
    public void Enter(RatController controller, IRatState previousState = null)
    {
        _controller = controller;
        var pos = controller.transform.position;
        _destination = controller.CurrentTarget.GetDestination(pos);
        controller.AIPath.enabled = true;
        _path = controller.Seeker.StartPath(pos, _destination);

        controller.StartCoroutine(controller.CheckForItems());
        //controller.StartCoroutine(controller.PlayStepsSounds());
    }


    public void Update()
    {
        UpdateAnimation();

        //ANTON : poner sonido pasos
        if (pasoSonido > maxSonido)
        {
            pasoSonido=0;
            randomIndex = Random.Range(0, 3);
            soundName = "Anda" + randomIndex;
            MusicManager.Instance.PlaySound(soundName);
        }
        else
        {
            pasoSonido++;
        }
    }

    private void UpdateAnimation()
    {
        var newDirection = (Vector2)_controller.AIPath.desiredVelocity;
        if (newDirection == Direction) return;

        Direction = newDirection;

        _controller.Renderer.flipX = Direction.x < 0f;

        if (Direction.y < 0f && _currentAnimation != "Move Front")
        {
            _controller.Animator.Play("Move Front");
            _currentAnimation = "Move Front";
        }
        
        if (Direction.y >= 0f && _currentAnimation != "Move Back")
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
