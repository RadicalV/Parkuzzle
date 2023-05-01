using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playSingleplayerButton;
    [SerializeField] private Button playMultiplayerButton;
    [SerializeField] private Button settingsButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playSingleplayerButton.onClick.AddListener(() =>
        {
            MapSelectUI.Instance.Show();
            SettingsUI.Instance.Hide();
        });

        playMultiplayerButton.onClick.AddListener(() =>
        {
            GameMultiplayerManager.playMultiplayer = true;
            Loader.Load(Loader.Scene.LobbyScene);
        });

        settingsButton.onClick.AddListener(() =>
        {
            SettingsUI.Instance.Show();
            MapSelectUI.Instance.Hide();
        });

        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
    }
}