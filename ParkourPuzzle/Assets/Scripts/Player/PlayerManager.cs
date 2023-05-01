using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    public static PlayerManager localInstance { get; private set; }

    private PlayerLocomotion playerLocomotion;
    private PlayerAiming playerAiming;
    public MoveData moveData { get; } = new MoveData();
    private Vector3 startPosition;
    private Vector3 respawnPosition;
    private Quaternion originalRotation;
    private Quaternion respawnRotation;
    private int tickRate = 64;
    public List<Transform> spawnPositionList;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) this.enabled = false;
        else
        {
            localInstance = this;
            transform.GetComponentInChildren<PlayerAnimator>().transform.GetChild(0).gameObject.SetActive(false);
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerAiming = GetComponentInChildren<PlayerAiming>();

            try
            {
                Transform spawnPoints = GameObject.Find("SpawnPoints").transform;
                if (spawnPoints)
                {
                    foreach (Transform child in spawnPoints)
                    {
                        spawnPositionList.Add(child);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }

            if (spawnPositionList.Count > 0)
            {
                Transform spawnPosition = spawnPositionList[UnityEngine.Random.Range(0, spawnPositionList.Count)];

                transform.position = spawnPosition.position;
                transform.rotation = spawnPosition.rotation;
            }

            moveData.origin = transform.position;
            startPosition = transform.position;
            respawnPosition = startPosition;
            originalRotation = transform.localRotation;
            respawnRotation = originalRotation;
            gameObject.name = GameMultiplayerManager.Instance.GetPlayerName();
        }
    }

    private void Start()
    {
        Time.fixedDeltaTime = 1f / tickRate;
    }

    private void Update()
    {
        InputManager.Instance.HandleAllInputs();

        if (InputManager.Instance.pauseInput && !EscapeMenuUI.Instance.gameObject.activeSelf)
        {
            EscapeMenuUI.Instance.Show();
            playerAiming.enabled = false;
            InputManager.Instance.DisableMovement();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            InputManager.Instance.pauseInput = false;
        }
        else if (InputManager.Instance.pauseInput && EscapeMenuUI.Instance.gameObject.activeSelf)
        {
            EscapeMenuUI.Instance.Hide();
        }

        if (InputManager.Instance.restartInput)
            Restart();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement(this, Time.fixedDeltaTime);
        transform.position = moveData.origin;
    }

    private void SetRespawnData(Vector3 position, Quaternion rotation)
    {
        respawnPosition = position;
        respawnRotation = rotation;
    }

    [ClientRpc]
    public void SetRespawnDataClientRpc(Vector3 position, Quaternion rotation, ClientRpcParams clientRpcParams = default)
    {
        SetRespawnData(position, rotation);
    }

    private void SetPlayerRotation(float yRotation)
    {
        playerAiming.SetYRotation(yRotation);
    }

    [ClientRpc]
    public void SetPlayerRotationClientRpc(float yRotation, ClientRpcParams clientRpcParams = default)
    {
        SetPlayerRotation(yRotation);
    }

    private void TeleportPlayer(Vector3 velocity)
    {
        moveData.origin = respawnPosition;
        moveData.velocity = velocity;
    }

    [ClientRpc]
    public void TeleportPlayerClientRpc(float destinationRotationY, ClientRpcParams clientRpcParams = default)
    {
        Vector3 newVelocity = new Vector3(Mathf.Sin(destinationRotationY), 0f,
        Mathf.Cos(destinationRotationY)) * Mathf.Sqrt(moveData.velocity.x * moveData.velocity.x + moveData.velocity.z * moveData.velocity.z);
        TeleportPlayer(newVelocity);
    }

    private void Respawn()
    {
        moveData.origin = respawnPosition;
        SetPlayerRotation(respawnRotation.eulerAngles.y);
        moveData.velocity = Vector3.zero;
    }

    [ClientRpc]
    public void RespawnPlayerClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Respawn();
    }

    private void Restart()
    {
        respawnPosition = startPosition;
        respawnRotation = originalRotation;
        Respawn();
    }

    [ClientRpc]
    public void RestartClientRpc(ClientRpcParams clientRpcParams = default)
    {
        Restart();
    }

    public void EnablePlayerMovement()
    {
        playerAiming.enabled = true;
        InputManager.Instance.EnableMovement();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        InputManager.Instance.pauseInput = false;
    }
}