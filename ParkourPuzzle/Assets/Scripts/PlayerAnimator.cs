using Unity.Netcode;
using UnityEngine;

public class PlayerAnimator : NetworkBehaviour
{
    private const string IS_JUMPING = "IsJumping";
    private const string IS_GROUNDED = "IsGrounded";
    private const string IS_FALLING = "IsFalling";
    private const string IS_MOVING = "IsMoving";
    private const string FORWARD = "forward";
    private const string STRAFE = "strafe";

    [SerializeField]
    private PlayerLocomotion playerLocomotion;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;

        animator.SetBool(IS_FALLING, playerLocomotion.IsFalling());
        animator.SetBool(IS_GROUNDED, playerLocomotion.IsGrounded());
        animator.SetBool(IS_JUMPING, playerLocomotion.IsJumping());
        animator.SetBool(IS_MOVING, playerLocomotion.IsMoving());
        animator.SetFloat(FORWARD, playerLocomotion.Forward());
        animator.SetFloat(STRAFE, playerLocomotion.Strafe());
    }
}