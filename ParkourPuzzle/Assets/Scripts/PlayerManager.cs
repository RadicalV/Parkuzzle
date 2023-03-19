using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerLocomotion playerLocomotion;
    public MoveData moveData { get; } = new MoveData();
    private Vector3 startPosition;
    private Quaternion originalRotation;
    private int tickRate = 128;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();

        Time.fixedDeltaTime = 1f / tickRate;
    }

    private void Start()
    {
        moveData.origin = transform.position;
        startPosition = transform.position;
        originalRotation = transform.localRotation;
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement(this, Time.fixedDeltaTime);
        transform.position = moveData.origin;
    }
}
