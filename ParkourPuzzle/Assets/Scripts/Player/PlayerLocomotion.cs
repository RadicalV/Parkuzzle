using Unity.Netcode;
using UnityEngine;

public class PlayerLocomotion : NetworkBehaviour
{
    InputManager inputManager;
    CapsuleCollider capsuleCollider;
    private Transform playerModel;
    private Vector3 defaultPlayerModelPos;

    [Header("Camera reference")]
    public Transform cameraObject;
    private Vector3 defaultCameraPosition;

    [Header("General Movement Stats")]
    public float currentVelocity; // To monitor speed
    public float maxVelocity = 50f;
    public float maxSpeed = 6f;
    public float friction = 6f;
    public float gravity = 20f;

    [Header("Player Ground Movement Stats")]
    public float acceleration = 14f;
    public float stopSpeed = 1.905f;

    [Header("Player Jumping Stats")]
    public bool autoBhop = true;
    public float jumpForce = 6.5f;

    [Header("Player Air Movement Stats")]
    public float airAcceleration = 12f;
    public float airCap = 0.4f;
    public float airFriction = 0.25f;

    [Header("Player Crouch Movement Stats")]
    public float crouchSpeed = 2.108f;
    public float crouchAcceleration = 8f;
    private float crouchTransitionSpeed = 10f;
    private float currentHeight;
    private float standingHeight;
    private bool crouching => standingHeight - currentHeight > 0.05f;

    private PlayerManager _playerManager;
    private float _deltaTime;
    private GameObject groundObject;

    //animation states
    private bool isJumping = false;
    private bool isGrounded = false;
    private bool isFalling = false;
    private bool isMoving = false;
    private bool isCrouching = false;
    private float forward = 0;
    private float strafe = 0;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) this.enabled = false;
        else
        {
            inputManager = InputManager.Instance;
            capsuleCollider = GetComponent<CapsuleCollider>();
            playerModel = transform.GetComponentInChildren<PlayerAnimator>().transform;
            defaultCameraPosition = cameraObject.localPosition;
            defaultPlayerModelPos = playerModel.localPosition;
            standingHeight = currentHeight = capsuleCollider.height;
        }
    }

    public void HandleAllMovement(PlayerManager playerManager, float deltaTime)
    {
        _playerManager = playerManager;
        _deltaTime = deltaTime;

        if (groundObject != null)
        {
            HandleGroundMovement();

            if (inputManager.jumpInput)
            {
                Jump();
            }
            else
                HandleFriction();
        }
        else
        {
            ApplyGravity();
            HandleAirMovement();
            SurfPhysics.Reflect(ref _playerManager.moveData.velocity, capsuleCollider, _playerManager.moveData.origin, _deltaTime);
        }

        Crouch();

        CheckGrounded();
        ClampVelocity();

        _playerManager.moveData.origin += _playerManager.moveData.velocity * _deltaTime;

        SurfPhysics.ResolveCollisions(capsuleCollider, ref _playerManager.moveData.origin, ref _playerManager.moveData.velocity);

        currentVelocity = Mathf.Sqrt(_playerManager.moveData.velocity.x * _playerManager.moveData.velocity.x + _playerManager.moveData.velocity.z * _playerManager.moveData.velocity.z) * 39.37f;
        _playerManager = null;

        if (PlayerUI.Instance)
            PlayerUI.Instance.UpdateVelocityUI(currentVelocity);
    }

    private void HandleGroundMovement()
    {
        GetWishValues(out Vector3 wishVel, out Vector3 wishDir, out float wishSpeed);

        var maxSpeedAmount = crouching ? crouchSpeed : maxSpeed;

        if ((wishSpeed != 0.0f) && (wishSpeed > maxSpeedAmount))
        {
            wishVel *= maxSpeedAmount / wishSpeed;
            wishSpeed = maxSpeedAmount;
        }

        float currentSpeed = Vector3.Dot(_playerManager.moveData.velocity, wishDir);
        float addSpeed = wishSpeed - currentSpeed;

        if (addSpeed <= 0)
            return;

        float accelerationSpeed = Mathf.Min(acceleration * _deltaTime * wishSpeed * _playerManager.moveData.surfaceFriction, addSpeed);

        for (int i = 0; i < 3; i++)
        {
            _playerManager.moveData.velocity[i] += accelerationSpeed * wishDir[i];
        }
    }

    private void HandleAirMovement()
    {
        GetWishValues(out Vector3 wishVel, out Vector3 wishDir, out float wishSpeed);

        if (wishSpeed > maxSpeed)
        {
            wishVel *= maxSpeed / wishSpeed;
            wishSpeed = maxSpeed;
        }

        var wishspd = wishSpeed;
        wishspd = Mathf.Min(wishspd, airCap);

        var currentspeed = Vector3.Dot(_playerManager.moveData.velocity, wishDir);
        var addspeed = wishspd - currentspeed;

        if (addspeed > 0)
        {
            var accelspeed = airAcceleration * wishSpeed * _deltaTime;
            accelspeed = Mathf.Min(accelspeed, addspeed);

            for (int i = 0; i < 3; i++)
            {
                _playerManager.moveData.velocity[i] += accelspeed * wishDir[i];
            }
        }
        else
        {
            _playerManager.moveData.velocity += Vector3.zero;
        }
    }

    private void GetWishValues(out Vector3 wishVel, out Vector3 wishDir, out float wishSpeed)
    {
        wishVel = Vector3.zero;
        wishDir = Vector3.zero;
        wishSpeed = 0f;

        Vector3 forward = cameraObject.forward,
        right = cameraObject.right;

        forward[1] = 0;
        right[1] = 0;
        forward.Normalize();
        right.Normalize();

        float forwardMove = 0f;
        float sideMove = 0f;

        if (inputManager.verticalMovementInput > 0f)
            forwardMove = crouching ? crouchAcceleration : acceleration;
        else if (inputManager.verticalMovementInput < 0f)
            forwardMove = crouching ? -crouchAcceleration : -acceleration;

        if (inputManager.horizontalMovementInput > 0f)
            sideMove = crouching ? crouchAcceleration : acceleration;
        else if (inputManager.horizontalMovementInput < 0f)
            sideMove = crouching ? -crouchAcceleration : -acceleration;

        this.forward = forwardMove;
        this.strafe = sideMove;

        for (int i = 0; i < 3; i++)
            wishVel[i] = forward[i] * forwardMove + right[i] * sideMove;
        wishVel[1] = 0;

        isMoving = wishVel != Vector3.zero;

        wishSpeed = wishVel.magnitude;
        wishDir = wishVel.normalized;
    }

    private void ClampVelocity()
    {
        for (int i = 0; i < 3; i++)
        {
            _playerManager.moveData.velocity[i] = Mathf.Clamp(_playerManager.moveData.velocity[i], -maxVelocity, maxVelocity);
        }
    }

    private void HandleFriction()
    {
        float speed = _playerManager.moveData.velocity.magnitude;

        if (speed < 0.0001905f)
        {
            return;
        }

        var drop = 0f;
        var control = (speed < stopSpeed) ? stopSpeed : speed;
        drop += control * friction * _playerManager.moveData.surfaceFriction * _deltaTime;

        var newspeed = speed - drop;
        if (newspeed < 0)
            newspeed = 0;

        if (newspeed != speed)
        {
            newspeed /= speed;
            _playerManager.moveData.velocity *= newspeed;
        }
    }

    private void ApplyGravity()
    {
        _playerManager.moveData.velocity.y -= gravity * _deltaTime;
        _playerManager.moveData.velocity.y += _playerManager.moveData.baseVelocity.y * _deltaTime;
    }

    private bool CheckGrounded()
    {
        _playerManager.moveData.surfaceFriction = 1f;
        var movingUp = _playerManager.moveData.velocity.y > 0f;
        var trace = TraceToFloor();

        if (trace.hitCollider == null
            || trace.hitCollider.gameObject.layer == LayerMask.NameToLayer("Trigger")
            || trace.planeNormal.y < SurfPhysics.SurfSlope
            || movingUp)
        {
            SetGround(null);
            isGrounded = false;

            if ((isJumping && _playerManager.moveData.velocity.y < 0) || _playerManager.moveData.velocity.y < -2f)
                isFalling = true;

            if (movingUp)
                _playerManager.moveData.surfaceFriction = airFriction;
            return false;
        }
        else
        {
            SetGround(trace.hitCollider.gameObject);
            isGrounded = true;
            isFalling = false;
            isJumping = false;

            return true;
        }
    }

    private void SetGround(GameObject obj)
    {
        if (obj != null)
        {
            groundObject = obj;
            _playerManager.moveData.velocity.y = 0;
        }
        else
        {
            groundObject = null;
        }
    }

    private Trace TraceToFloor(float distance = 0.05f, float extentModifier = 1.0f)
    {
        var down = _playerManager.moveData.origin;
        down.y -= 0.1f;

        SurfPhysics.GetCapsulePoints(capsuleCollider, _playerManager.moveData.origin, out Vector3 point1, out Vector3 point2);
        return Tracer.TraceCapsule(point1, point2, capsuleCollider.radius, _playerManager.moveData.origin, down, capsuleCollider.contactOffset, SurfPhysics.groundLayerMask);
    }

    private void Jump()
    {
        _playerManager.moveData.velocity.y += jumpForce;
        isJumping = true;
    }

    private void Crouch()
    {
        bool isTryingToCrouch = inputManager.crouchInput;
        isCrouching = isTryingToCrouch;
        var heightTarget = isTryingToCrouch ? _playerManager.moveData.crouchingHeight : standingHeight;

        if (crouching && !isTryingToCrouch)
        {
            var castOrigin = _playerManager.moveData.origin + new Vector3(0, currentHeight / 2, 0);
            if (Physics.Raycast(castOrigin, Vector3.up, out RaycastHit hit, 0.2f))
            {
                var distanceToCeiling = hit.point.y - castOrigin.y;
                heightTarget = Mathf.Max(currentHeight + distanceToCeiling - 0.1f, _playerManager.moveData.crouchingHeight);
            }
        }

        if (!Mathf.Approximately(heightTarget, currentHeight))
        {
            var crouchDelta = _deltaTime * crouchTransitionSpeed;
            currentHeight = Mathf.Lerp(currentHeight, heightTarget, crouchDelta);

            float halfHeightDifference = (standingHeight - currentHeight) / 2;
            Vector3 newCamPos = new Vector3(defaultCameraPosition.x, defaultCameraPosition.y - halfHeightDifference, defaultCameraPosition.z);

            capsuleCollider.height = currentHeight;
            cameraObject.localPosition = newCamPos;

            playerModel.localPosition = new Vector3(defaultPlayerModelPos.x, defaultPlayerModelPos.y + halfHeightDifference, defaultPlayerModelPos.z);

            if (groundObject)
                _playerManager.moveData.origin += halfHeightDifference / 2 * Vector3.down;
        }
    }

    public bool IsJumping() => isJumping;
    public bool IsFalling() => isFalling;
    public bool IsGrounded() => isGrounded;
    public bool IsMoving() => isMoving;
    public bool IsCrouching() => isCrouching;
    public float Forward() => forward;
    public float Strafe() => strafe;
}