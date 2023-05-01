using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class HostDisconnectUI : MonoBehaviour
{
    [SerializeField] private Button backToMainMenuButton;

    private void Awake()
    {
        backToMainMenuButton.onClick.AddListener(() =>
        {
            if (InputManager.Instance != null)
            {
                Destroy(InputManager.Instance.gameObject);
            }

            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    private void Start()
    {
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        Hide();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        if (clientId == NetworkManager.ServerClientId)
        {
            EscapeMenuUI.Instance.Hide();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Show();
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}