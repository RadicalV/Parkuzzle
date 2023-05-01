using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private const string IS_JUMPING = "IsJumping";
    private const string IS_GROUNDED = "IsGrounded";
    private const string IS_FALLING = "IsFalling";
    private const string IS_MOVING = "IsMoving";
    private const string IS_CROUCHING = "IsCrouching";
    private const string FORWARD = "forward";
    private const string STRAFE = "strafe";

    [SerializeField]
    private PlayerLocomotion playerLocomotion;
    private Animator animator;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) this.enabled = false;
        else animator = GetComponent<Animator>();
    }

    void Update()
    {
        animator.SetBool(IS_FALLING, playerLocomotion.IsFalling());
        animator.SetBool(IS_GROUNDED, playerLocomotion.IsGrounded());
        animator.SetBool(IS_JUMPING, playerLocomotion.IsJumping());
        animator.SetBool(IS_MOVING, playerLocomotion.IsMoving());
        animator.SetBool(IS_CROUCHING, playerLocomotion.IsCrouching());
        animator.SetFloat(FORWARD, playerLocomotion.Forward());
        animator.SetFloat(STRAFE, playerLocomotion.Strafe());
    }
}