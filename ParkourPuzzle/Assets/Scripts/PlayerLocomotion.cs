using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    InputManager inputManager;
    Rigidbody playerRigidbody;
    BoxCollider boxCollider;

    [Header("Camera reference")]
    public Transform cameraObject;
    public float mouseSensitivity = 2f;
    private float mouseSensitivityMultiplier = 10f;
    private float yCameraRotation;
    private float xCameraRotation;

    [Header("General Movement Stats")]
    public float currentVelocity; // To monitor speed
    public float maxVelocity = 50f;
    public float maxSpeed = 6f;
    public float friction = 6f;
    public float gravity = 20f;

    [Header("Player Ground Movement Stats")]
    public float walkSpeed = 7f;
    public float acceleration = 14f;
    public float deceleration = 10f;
    public LayerMask groundLayer;

    [Header("Player Jumping Stats")]
    public float jumpForce = 6.5f;

    [Header("Player Air Movement Stats")]
    public float airAcceleration = 12f;
    public float airCap = 0.4f;
    public bool clampAirSpeed = true;

    [Header("Player Crouch Movement Stats")]
    public float crouchSpeed = 4f;
    public float crouchAcceleration = 8f;
    public float crouchDeceleration = 4f;
    public float crouchFriction = 3f;

    Vector3 moveDirection;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        inputManager = GetComponent<InputManager>();
        playerRigidbody = GetComponent<Rigidbody>();

        boxCollider = GetComponent<BoxCollider>();
    }

    public void HandleAllMovement()
    {
        if (isGrounded())
        {
            HandleGroundMovement();
            HandleFriction();
        }
        else
        {
            //HandleAirMovement();
        }

        ApplyGravity();

        if (inputManager.jumpInput && isGrounded())
            Jump();

        // Camera and player rotation
        HandleRotation();
    }

    private void HandleGroundMovement()
    {
        moveDirection = transform.right * inputManager.horizontalMovementInput + transform.forward * inputManager.verticalMovementInput;
        moveDirection.Normalize();


        float currentSpeed = Vector3.Dot(playerRigidbody.velocity, moveDirection);
        float wishSpeed = moveDirection.magnitude;
        wishSpeed *= walkSpeed;
        float addSpeed = wishSpeed - currentSpeed;


        if (addSpeed <= 0)
            return;

        float accelerationSpeed = Mathf.Min(acceleration * Time.deltaTime * wishSpeed, addSpeed);

        Vector3 playerVelocity = playerRigidbody.velocity;

        // Add the velocity.
        playerVelocity.x += accelerationSpeed * moveDirection.x;
        //if (yMovement) { playerRigidbody.velocity.y += accelerationSpeed * moveDirection.y; }
        playerVelocity.z += accelerationSpeed * moveDirection.z;

        float maxVelocityMagnitude = maxVelocity;

        playerVelocity = Vector3.ClampMagnitude(new Vector3(playerVelocity.x, 0f, playerVelocity.z), maxVelocityMagnitude);

        playerRigidbody.velocity = playerVelocity;

        currentVelocity = playerVelocity.magnitude;
    }

    private void HandleAirMovement()
    {
        Vector3 wishVel, wishDir;
        float wishSpeed;

        //Get wish values
        wishVel = Vector3.zero;
        wishDir = Vector3.zero;
        wishSpeed = 0f;

        Vector3 forward = transform.forward,
        right = transform.right;

        forward[1] = 0;
        right[1] = 0;

        forward.Normalize();
        right.Normalize();

        for (int i = 0; i < 3; i++)

            wishVel[i] = forward[i] * (inputManager.verticalMovementInput < 0 ? -acceleration : acceleration) + right[i] * (inputManager.horizontalMovementInput < 0 ? -acceleration : acceleration);

        wishVel[1] = 0;

        wishSpeed = wishVel.magnitude;

        wishDir = wishVel.normalized;


        if (clampAirSpeed && (wishSpeed != 0f && (wishSpeed > maxSpeed)))
        {
            wishVel = wishVel * (maxSpeed / wishSpeed);
            wishSpeed = maxSpeed;
        }

        var wishspd = wishSpeed;

        // Cap speed
        wishspd = Mathf.Min(wishspd, airCap);

        // Determine veer amount
        var currentspeed = Vector3.Dot(playerRigidbody.velocity, wishDir);

        // See how much to add
        var addspeed = wishspd - currentspeed;

        if (addspeed > 0)
        {
            // Determine acceleration speed after acceleration
            var accelspeed = airAcceleration * wishSpeed * Time.deltaTime;

            // Cap it
            accelspeed = Mathf.Min(accelspeed, addspeed);

            var result = Vector3.zero;

            // Adjust pmove vel.
            for (int i = 0; i < 3; i++)

                result[i] += accelspeed * wishDir[i];

            playerRigidbody.velocity += result;
        }
        else
        {
            playerRigidbody.velocity += Vector3.zero;
        }
    }

    private void HandleRotation()
    {
        yCameraRotation += inputManager.horizontalCameraInput * mouseSensitivity * mouseSensitivityMultiplier * Time.deltaTime;
        xCameraRotation -= inputManager.verticalCameraInput * mouseSensitivity * mouseSensitivityMultiplier * Time.deltaTime;

        xCameraRotation = Mathf.Clamp(xCameraRotation, -90f, 90f);

        cameraObject.rotation = Quaternion.Euler(xCameraRotation, yCameraRotation, 0f);
        transform.rotation = Quaternion.Euler(0f, yCameraRotation, 0f);
    }

    private void HandleFriction()
    {
        Vector3 playerVelocity = playerRigidbody.velocity;
        float playerYVelocity = playerVelocity.y;

        float _speed;
        float _newSpeed;
        float _control;
        float _drop;

        playerVelocity.y = 0.0f;
        _speed = playerVelocity.magnitude;
        _drop = 0.0f;

        playerVelocity.y = playerYVelocity;
        _control = _speed < deceleration ? deceleration : _speed;
        _drop = _control * friction * Time.deltaTime * 1;

        _newSpeed = Mathf.Max(_speed - _drop, 0f);
        if (_speed > 0.0f)
            _newSpeed /= _speed;

        // Set the end-velocity
        playerVelocity.x *= _newSpeed;
        playerVelocity.y *= _newSpeed;
        playerVelocity.z *= _newSpeed;

        playerRigidbody.velocity = playerVelocity;
    }

    private void ApplyGravity()
    {
        Vector3 playerVelocity = playerRigidbody.velocity;
        playerVelocity.y -= (gravity * Time.deltaTime);

        playerRigidbody.velocity = playerVelocity;
    }

    private bool isGrounded()
    {
        RaycastHit hit;
        Physics.BoxCast(boxCollider.bounds.center, boxCollider.transform.lossyScale / 2, Vector3.down, out hit, boxCollider.transform.rotation, 0.6f, groundLayer);

        return hit.collider != null;
    }

    private void Jump()
    {

        // if (!_config.autoBhop)
        //     _surfer.moveData.wishJump = false;

        Vector3 playerVelocity = playerRigidbody.velocity;

        playerVelocity.y += jumpForce;

        playerRigidbody.velocity = playerVelocity;

        inputManager.jumpInput = false;
    }
}
