using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SurfGameManager : NetworkBehaviour
{
    public static SurfGameManager Instance { get; private set; }

    private float timer = 0f;
    private bool isTimerRunning = false;

    [SerializeField] private Transform playerPrefab;
    private bool InitialSpawnDone;

    private NetworkList<FixedString64Bytes> playerNames = new NetworkList<FixedString64Bytes>(new List<FixedString64Bytes>());
    private NetworkList<float> values = new NetworkList<float>(new List<float>());

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        EscapeMenuUI.Instance.Hide();
        SettingsUI.Instance.Hide();
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
            NetworkManager.Singleton.SceneManager.OnSynchronizeComplete += OnSynchronizeComplete;
        }
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.SceneManager.OnLoadEventCompleted -= SceneManager_OnLoadEventCompleted;
        NetworkManager.Singleton.SceneManager.OnSynchronizeComplete -= OnSynchronizeComplete;
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            timer += Time.deltaTime;

            PlayerUI.Instance.UpdateTimerUI(timer);
        }
    }

    private void OnSynchronizeComplete(ulong clientId)
    {
        if (InitialSpawnDone)
        {
            Transform playerTransform = Instantiate(playerPrefab);

            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        if (!InitialSpawnDone)
        {
            InitialSpawnDone = true;
            foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
            {
                Transform playerTransform = Instantiate(playerPrefab);

                playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(NetworkManager.LocalClientId, true);
            }
        }
    }

    [ClientRpc]
    public void StartTimerClientRpc(ClientRpcParams clientRpcParams = default)
    {
        isTimerRunning = true;
        timer = 0f;
    }

    [ClientRpc]
    public void RestartTimerClientRpc(ClientRpcParams clientRpcParams = default)
    {
        isTimerRunning = false;
        timer = 0f;
        PlayerUI.Instance.UpdateTimerUI(timer);
    }

    [ClientRpc]
    public void StopTimerClientRpc(string playerName, ClientRpcParams clientRpcParams = default)
    {
        isTimerRunning = false;

        UpdateLeaderboardServerRpc(playerName, timer);
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdateLeaderboardServerRpc(string name, float time)
    {
        if (playerNames.Contains(name))
        {
            if (values[playerNames.IndexOf(name)] > time)
            {
                values[playerNames.IndexOf(name)] = time;
            }
        }
        else
        {
            playerNames.Add(name);
            values.Add(time);
        }
    }

    public Dictionary<string, string> GetLeaderboard()
    {
        Dictionary<string, string> leaderboard = new Dictionary<string, string>();

        for (int i = 0; i < playerNames.Count; i++)
        {
            leaderboard.Add(playerNames[i].ToString(), FormatTime(values[i]));
        }

        return leaderboard;
    }

    private string FormatTime(float time)
    {
        TimeSpan ts = TimeSpan.FromSeconds(time);

        String result = ts.ToString("mm\\:ss\\:ff");

        return result;
    }
}