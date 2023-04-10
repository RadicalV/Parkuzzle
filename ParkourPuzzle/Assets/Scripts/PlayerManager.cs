using Unity.Netcode;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerAiming playerAiming;
    public MoveData moveData { get; } = new MoveData();
    private Vector3 startPosition;
    private Vector3 respawnPosition;
    private Quaternion originalRotation;
    private Quaternion respawnRotation;
    private int tickRate = 64;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerAiming = GetComponentInChildren<PlayerAiming>();

        Time.fixedDeltaTime = 1f / tickRate;
    }

    private void Start()
    {
        moveData.origin = transform.position;
        startPosition = transform.position;
        respawnPosition = startPosition;
        originalRotation = transform.localRotation;
        respawnRotation = originalRotation;
    }

    private void Update()
    {
        inputManager.HandleAllInputs();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement(this, Time.fixedDeltaTime);
        transform.position = moveData.origin;
    }

    public void SetRespawnData(Vector3 position, Quaternion rotation)
    {
        respawnPosition = position;
        respawnRotation = rotation;
    }

    public void Respawn()
    {
        moveData.origin = respawnPosition;
        SetPlayerRotation(respawnRotation.eulerAngles.y);
        moveData.velocity = Vector3.zero;
    }

    private void Restart()
    {
        respawnPosition = startPosition;
        respawnRotation = originalRotation;
        Respawn();
    }

    public void SetPlayerRotation(float yRotation)
    {
        playerAiming.SetYRotation(yRotation);
    }
}
