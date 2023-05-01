using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenuUI : MonoBehaviour
{
    public static EscapeMenuUI Instance { get; private set; }

    [SerializeField] private Button closeMenuButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button backToMainButton;
    [SerializeField] private Button leaderboardButton;

    private void Awake()
    {
        Instance = this;

        closeMenuButton.onClick.AddListener(() =>
        {
            Hide();
        });

        settingsButton.onClick.AddListener(() =>
        {
            SettingsUI.Instance.Show();
        });

        leaderboardButton.onClick.AddListener(() =>
        {
            LeaderboardUI.Instance.Show();
        });

        backToMainButton.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            SurfGameLobby.Instance.LeaveLobby();

            if (InputManager.Instance != null)
            {
                Destroy(InputManager.Instance.gameObject);
            }

            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        SettingsUI.Instance.Hide();
        LeaderboardUI.Instance.Hide();

        if (PlayerManager.localInstance != null)
            PlayerManager.localInstance.EnablePlayerMovement();
    }
}