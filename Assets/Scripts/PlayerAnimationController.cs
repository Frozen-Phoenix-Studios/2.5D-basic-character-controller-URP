using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnimationController : MonoBehaviour
{


    //Variables//
    private bool _jumping;
    private float _xDirection;

    //Component references//
    private Animator _anim;
    private CharacterController _controller;


    //Script references//
    private PlayerController _controls;


    //Unity functions//

    private void Awake()
    {
        AssignComponents();
    }

    private void AssignComponents()
    {
        _anim = GetComponentInChildren<Animator>();
        if (_anim == null)
        {
            Debug.LogError($"{_anim.GetType().ToString()} is null on {this.transform.name.ToString()}");
        }

        _controls = GetComponent<PlayerController>();
        if (_controls == null)
        {
            Debug.LogError($"{_controls.GetType().ToString()} is null on {this.transform.name.ToString()}");
        }
    }


    public void UpdateJumpingAnim(bool isJumping)
    {
        if (isJumping == _jumping)
        {
            return;
        }

        _jumping = isJumping;

        _anim.SetBool("Jumping", _jumping);
    }


    //Custom functions//
    public void ReadMovement(InputAction.CallbackContext context)
    {
        _xDirection = context.ReadValue<Vector2>().normalized.x;
        _anim.SetFloat("Speed", Mathf.Abs(_xDirection));
        AssignDirectionToFace();
    }

    private void AssignDirectionToFace()
    {
        var directionToFace = transform.localEulerAngles;

        if (_xDirection > 0 && !_jumping)
        {
            directionToFace.y = 0;
            transform.localEulerAngles = directionToFace;
        }
        else if (_xDirection < 0 && !_jumping)
        {
            directionToFace.y = 180;
            transform.localEulerAngles = directionToFace;
        }
    }
}