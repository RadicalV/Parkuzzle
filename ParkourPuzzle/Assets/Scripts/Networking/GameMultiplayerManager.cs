using Unity.Netcode;
using UnityEngine;
using static Loader;

public class GameMultiplayerManager : NetworkBehaviour
{
    public const int MAX_PLAYER_AMOUNT = 16;
    private const string PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER = "PlayerNameMultiplayer";

    public static GameMultiplayerManager Instance { get; private set; }
    public static bool playMultiplayer = true;
    public static Scene targetScene;

    private string playerName;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        playerName = PlayerPrefs.GetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, "PlayerName" + UnityEngine.Random.Range(100, 1000));
    }

    private void Start()
    {
        if (!playMultiplayer)
        {
            StartHost();
            Loader.LoadNetwork(targetScene);
        }
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;

        PlayerPrefs.SetString(PLAYER_PREFS_PLAYER_NAME_MULTIPLAYER, playerName);
    }

    public void StartHost()
    {
        NetworkManager.Singleton.StartHost();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}