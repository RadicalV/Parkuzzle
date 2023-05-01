using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";
    PlayerControls playerControls;

    public static InputManager Instance { get; private set; }

    [Header("Player Movement")]
    public float verticalMovementInput;
    public float horizontalMovementInput;
    private Vector2 movementInput;

    [Header("Camera Rotation")]
    public float verticalCameraInput;
    public float horizontalCameraInput;
    private Vector2 cameraInput;

    [Header("Button Inputs")]
    public bool jumpInput;
    public bool crouchInput;
    public bool pauseInput;
    public bool restartInput;

    public event EventHandler OnBindingRebind;


    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Jump,
        Crouch,
        Pause,
        Restart
    }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();

            if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
            {
                playerControls.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
            }

            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Jump.performed += i => jumpInput = true;
            playerControls.PlayerMovement.Jump.canceled += i => jumpInput = false;
            playerControls.PlayerMovement.Crouch.performed += i => crouchInput = true;
            playerControls.PlayerMovement.Crouch.canceled += i => crouchInput = false;
            playerControls.PlayerMovement.Pause.performed += i => pauseInput = true;
            playerControls.PlayerMovement.Pause.canceled += i => pauseInput = false;
            playerControls.PlayerMovement.Restart.performed += i => restartInput = true;
            playerControls.PlayerMovement.Restart.canceled += i => restartInput = false;
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
        HandleCameraInput();
    }

    private void HandleMovementInput()
    {
        verticalMovementInput = movementInput.y;
        horizontalMovementInput = movementInput.x;
    }

    private void HandleCameraInput()
    {
        verticalCameraInput = cameraInput.y;
        horizontalCameraInput = cameraInput.x;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerControls.PlayerMovement.Movement.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerControls.PlayerMovement.Movement.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerControls.PlayerMovement.Movement.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerControls.PlayerMovement.Movement.bindings[4].ToDisplayString();
            case Binding.Jump:
                return playerControls.PlayerMovement.Jump.bindings[0].ToDisplayString();
            case Binding.Crouch:
                return playerControls.PlayerMovement.Crouch.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerControls.PlayerMovement.Pause.bindings[0].ToDisplayString();
            case Binding.Restart:
                return playerControls.PlayerMovement.Restart.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerControls.PlayerMovement.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputAction = playerControls.PlayerMovement.Movement;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputAction = playerControls.PlayerMovement.Movement;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputAction = playerControls.PlayerMovement.Movement;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputAction = playerControls.PlayerMovement.Movement;
                bindingIndex = 4;
                break;
            case Binding.Jump:
                inputAction = playerControls.PlayerMovement.Jump;
                bindingIndex = 0;
                break;
            case Binding.Crouch:
                inputAction = playerControls.PlayerMovement.Crouch;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = playerControls.PlayerMovement.Pause;
                bindingIndex = 0;
                break;
            case Binding.Restart:
                inputAction = playerControls.PlayerMovement.Restart;
                bindingIndex = 0;
                break;
        }

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerControls.PlayerMovement.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerControls.SaveBindingOverridesAsJson());

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }

    public void DisableMovement()
    {
        playerControls.PlayerMovement.Movement.Disable();
        movementInput = Vector2.zero;
        playerControls.PlayerMovement.Jump.Disable();
        playerControls.PlayerMovement.Crouch.Disable();
        playerControls.PlayerMovement.Restart.Disable();
    }

    public void EnableMovement()
    {
        playerControls.PlayerMovement.Movement.Enable();
        playerControls.PlayerMovement.Jump.Enable();
        playerControls.PlayerMovement.Crouch.Enable();
        playerControls.PlayerMovement.Restart.Enable();
    }
}