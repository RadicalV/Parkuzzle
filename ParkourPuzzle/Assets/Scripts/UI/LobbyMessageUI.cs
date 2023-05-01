using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyMessageUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        closeButton.onClick.AddListener(() =>
        {
            Hide();
        });
    }

    private void Start()
    {
        SurfGameLobby.Instance.OnCreateLobbyStarted += SurfGameLobby_OnCreateLobbyStarted;
        SurfGameLobby.Instance.OnCreateLobbyFailed += SurfGameLobby_OnCreateLobbyFailed;
        SurfGameLobby.Instance.OnJoinStarted += SurfGameLobby_OnJoinStarted;
        SurfGameLobby.Instance.OnJoinFailed += SurfGameLobby_OnJoinFailed;
        SurfGameLobby.Instance.OnQuickJoinFailed += SurfGameLobby_OnQuickJoinFailed;

        closeButton.gameObject.SetActive(false);

        if (GameMultiplayerManager.playMultiplayer)
            Hide();
    }

    private void SurfGameLobby_OnCreateLobbyStarted(object sender, EventArgs e)
    {
        ShowMessage("Creating Lobby...");
        messageText.color = new Color(255, 255, 255, 1);
    }

    private void SurfGameLobby_OnCreateLobbyFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to create Lobby!");
        messageText.color = new Color(255, 0, 0, 1);

        closeButton.gameObject.SetActive(true);
    }

    private void SurfGameLobby_OnJoinStarted(object sender, EventArgs e)
    {
        ShowMessage("Joining Lobby...");
        messageText.color = new Color(255, 255, 255, 1);
    }

    private void SurfGameLobby_OnJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Failed to join Lobby!");
        messageText.color = new Color(255, 0, 0, 1);

        closeButton.gameObject.SetActive(true);
    }

    private void SurfGameLobby_OnQuickJoinFailed(object sender, EventArgs e)
    {
        ShowMessage("Could not find a Lobby to Quick Join!");
        messageText.color = new Color(255, 0, 0, 1);

        closeButton.gameObject.SetActive(true);
    }

    private void ShowMessage(string message)
    {
        Show();

        messageText.text = message;
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        SurfGameLobby.Instance.OnCreateLobbyStarted -= SurfGameLobby_OnCreateLobbyStarted;
        SurfGameLobby.Instance.OnCreateLobbyFailed -= SurfGameLobby_OnCreateLobbyFailed;
        SurfGameLobby.Instance.OnJoinStarted -= SurfGameLobby_OnJoinStarted;
        SurfGameLobby.Instance.OnJoinFailed -= SurfGameLobby_OnJoinFailed;
        SurfGameLobby.Instance.OnQuickJoinFailed -= SurfGameLobby_OnQuickJoinFailed;
    }
}