using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Variables//
    [SerializeField] private float _speed = 5.0f;
    [SerializeField] private float _gravity = 5.0f;
    [SerializeField] private float _jumpStrength = 5.0f;

    private float _xDirection;
    private Vector3 _direction;
    private Vector3 _velocity;
    private bool _jumping;
    public bool Jumping => _jumping;
    private float _yVelocity;


    //Component references//
    private CharacterController _controller;

    //Script references//
    private PlayerAnimationController _anim;
    
    //Unity functions//

    private void Awake()
    {
        AssignComponents();
    }

    private void AssignComponents()
    {
        _controller = GetComponent<CharacterController>();
        if (_controller == null)
        {
            Debug.LogError($"{_controller.GetType().ToString()} is null on {this.transform.name.ToString()}");
        }

        _anim = GetComponent<PlayerAnimationController>();
        if (_anim == null)
        {
            Debug.LogError($"{_anim.GetType().ToString()} is null on {this.transform.name.ToString()}");
        }
    }

    private void Update()
    {
        Movement();
    }
    
    //Custom functions//

    private void ResetJumpingStatus()
    {
        if (_jumping && _velocity.y <= 0)
        {
            _jumping = false;
            _anim.UpdateJumpingAnim(_jumping);
        }
    }

    private void Movement()
    {
        if (_controller.isGrounded && _yVelocity <= 0)
        {
            ResetJumpingStatus();
            _direction = new Vector3(0, _yVelocity, _xDirection);
            _velocity = _direction * _speed;
        }
        else
        {
            _yVelocity -= _gravity * Time.deltaTime;
            _velocity.y = _yVelocity;
        }

        _controller.Move(_velocity * Time.deltaTime);
    }


    private void Jump()
    {
        _anim.UpdateJumpingAnim(true);
        _yVelocity = _jumpStrength;
        _velocity.y = _yVelocity;
        _jumping = true;
    }

    private void ApplyGravity()
    {
        _velocity.y -= _gravity * Time.deltaTime;
        _controller.Move(_velocity * Time.deltaTime);
    }


    //Input Action//
    public void ReadMovement(InputAction.CallbackContext context)
    {
        _xDirection = context.ReadValue<Vector2>().normalized.x;
    }
    
    public void ReadJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!_controller.isGrounded)
            {
                return;
            }

            Jump();
        }
    }


}
