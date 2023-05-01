using UnityEngine;
using UnityEngine.UI;
using static Loader;

public class MapSelectUI : MonoBehaviour
{
    public static MapSelectUI Instance { get; private set; }

    [Header("Buttons")]
    [SerializeField] private Button backButton;
    [SerializeField] private Button playButton;

    [Header("Map toggles")]
    [SerializeField] private Toggle surf_UtopiaToggle;
    [SerializeField] private Toggle surf_KitsuneToggle;

    private Scene selectedMap;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);

        backButton.onClick.AddListener(() =>
        {
            Hide();
        });

        playButton.onClick.AddListener(() =>
        {
            GameMultiplayerManager.playMultiplayer = false;

            GameMultiplayerManager.targetScene = selectedMap;
            Loader.Load(Loader.Scene.LobbyScene);
        });

        surf_UtopiaToggle.onValueChanged.AddListener(value =>
        {
            if (value)
                selectedMap = Loader.Scene.Surf_Utopia;
        });

        surf_KitsuneToggle.onValueChanged.AddListener(value =>
        {
            if (value)
                selectedMap = Loader.Scene.Surf_Kitsune;
        });
    }

    private void Start()
    {
        if (surf_UtopiaToggle.isOn)
            selectedMap = Loader.Scene.Surf_Utopia;
        if (surf_KitsuneToggle.isOn)
            selectedMap = Loader.Scene.Surf_Kitsune;
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