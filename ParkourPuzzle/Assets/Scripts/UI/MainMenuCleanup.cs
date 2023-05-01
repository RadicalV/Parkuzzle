using Unity.Netcode;
using UnityEngine;

public class MainMenuCleanup : MonoBehaviour
{
    private void Awake()
    {
        if (NetworkManager.Singleton != null)
        {
            Destroy(NetworkManager.Singleton.gameObject);
        }

        if (GameMultiplayerManager.Instance != null)
        {
            Destroy(GameMultiplayerManager.Instance.gameObject);
        }

        if (SurfGameLobby.Instance != null)
        {
            Destroy(SurfGameLobby.Instance.gameObject);
        }
    }
}