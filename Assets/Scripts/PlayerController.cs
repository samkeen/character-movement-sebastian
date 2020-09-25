using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 2;
    [SerializeField] private float runSpeed = 3;
    [SerializeField] private float speedSmoothTime = 0.1f;
    // roughly number of seconds to go from current angle to the target angle.
    [SerializeField] private float turnSmoothTime = 0.2f;

    private Animator _animator;
    private float _turnSmoothVelocity;
    private float _speedSmoothVelocity;
    private float _currentSpeed;

    private void Start()
    {
        // %%%%% Need to add animated Character
        // _animator = GetComponent<Animator>();
    }

    void Update()
    {
        // create a vector2 for the keyboard input (x,z).  y is handed separately to allow 
        //   for jumping and gravity
        // --------------------------------------------------------- y is actually z here
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        // turn the input vector into a direction
        //   "When normalized, a vector keeps the same direction but its length is 1.0"
        Vector2 inputDirection = input.normalized;
        // determine the character's rotation
        // (-) = atan(y/x) but in unity we rotate anticlockwise 90deg, so
        // r = 90 - (-), or r = atan(x/y)
        // below we could have done Mathf.Atan(input_direction.x/input_direction.y) but Atan2 with 2
        //   params takes care of division by zero
        // ------------------------------------------------------------------ y is actually z here
        if (inputDirection != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(
                transform.eulerAngles.y, // current angle
                targetRotation, // target angle
                ref _turnSmoothVelocity, // allow function to reference the _turnSmoothVelocity var
                turnSmoothTime // time in sec to perform rotation
            );
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        // is inputDirection.magnitude is 0, speed is zero, else the inputDirection.magnitude will be
        //   1, which will not change the speed
        float targetSpeed = ((running) ? this.runSpeed : this.walkSpeed) * inputDirection.magnitude;
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, speedSmoothTime);

        // move the character in the direction they are facing in worldspace
        transform.Translate(transform.forward * _currentSpeed * Time.deltaTime, Space.World);

        // %%%%% Need to add animated Character
        // float animationSpeedPercent = ((running) ? 1f : .5f) * inputDirection.magnitude;
        // _animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }
}