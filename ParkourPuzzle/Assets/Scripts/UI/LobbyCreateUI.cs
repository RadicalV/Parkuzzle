using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Loader;

public class LobbyCreateUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button createLobbyButton;
    [SerializeField] private TMP_InputField lobbyNameInputField;
    [SerializeField] private TMP_Dropdown mapSelectDropdown;

    private List<Scene> surfMaps = new List<Scene> { Scene.Surf_Utopia, Scene.Surf_Kitsune };

    private void Awake()
    {
        surfMaps.ForEach(map =>
        {
            mapSelectDropdown.options.Add(new TMP_Dropdown.OptionData() { text = map.ToString() });
        });

        backButton.onClick.AddListener(() =>
        {
            Hide();
        });
        createLobbyButton.onClick.AddListener(() =>
        {
            GameMultiplayerManager.targetScene = surfMaps[mapSelectDropdown.value];
            SurfGameLobby.Instance.CreateLobby(lobbyNameInputField.text);
        });
    }

    private void Start()
    {
        mapSelectDropdown.value = 0;
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}