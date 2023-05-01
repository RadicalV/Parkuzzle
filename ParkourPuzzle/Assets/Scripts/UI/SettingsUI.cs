using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    private const string PLAYER_PREFS_SENSITIVITY = "MouseSensitivity";
    private const string PLAYER_PREFS_MASTER_VOLUME = "MasterVolume";
    private const string PLAYER_PREFS_MUSIC_VOLUME = "MusicVolume";

    public static SettingsUI Instance { get; private set; }

    [Header("Base UI elements")]
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Button backButton;
    [SerializeField] private Button applyButton;
    [SerializeField] private GameObject videoSettings;
    [SerializeField] private GameObject audioSettings;
    [SerializeField] private GameObject controlsSettings;

    [Header("Settings buttons")]
    [SerializeField] private Button videoSettingsButton;
    [SerializeField] private Button audioSettingsButton;
    [SerializeField] private Button controlsSettingsButton;

    [Header("Video settings UI elements")]
    [SerializeField] private Toggle fullscreenTog, vsyncTog;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private List<ResItem> resolutions;

    [Header("Audio settings UI elements")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;

    [Header("Controls settings UI elements")]
    [SerializeField] private Button moveUpButton;
    [SerializeField] private Button moveDownButton;
    [SerializeField] private Button moveLeftButton;
    [SerializeField] private Button moveRightButton;
    [SerializeField] private Button jumpButton;
    [SerializeField] private Button crouchButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TextMeshProUGUI moveUpText;
    [SerializeField] private TextMeshProUGUI moveDownText;
    [SerializeField] private TextMeshProUGUI moveLeftText;
    [SerializeField] private TextMeshProUGUI moveRightText;
    [SerializeField] private TextMeshProUGUI jumpText;
    [SerializeField] private TextMeshProUGUI crouchText;
    [SerializeField] private TextMeshProUGUI sliderValueText;
    [SerializeField] private TextMeshProUGUI restartText;
    [SerializeField] private GameObject pressToRebindKeyTransform;

    private void Awake()
    {
        Instance = this;

        audioSettings.SetActive(false);
        controlsSettings.SetActive(false);
        pressToRebindKeyTransform.SetActive(false);

        var options = new List<string>();

        resolutions.ForEach(res =>
        {
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = res.horizontal.ToString() + 'x' + res.vertical.ToString() });
        });

        backButton.onClick.AddListener(() =>
        {
            Hide();
        });

        videoSettingsButton.onClick.AddListener(() =>
        {
            videoSettings.SetActive(true);
            audioSettings.SetActive(false);
            controlsSettings.SetActive(false);
        });

        audioSettingsButton.onClick.AddListener(() =>
        {
            videoSettings.SetActive(false);
            audioSettings.SetActive(true);
            controlsSettings.SetActive(false);
        });

        controlsSettingsButton.onClick.AddListener(() =>
        {
            videoSettings.SetActive(false);
            audioSettings.SetActive(false);
            controlsSettings.SetActive(true);
        });

        moveUpButton.onClick.AddListener(() =>
        {
            moveUpText.text = "";
            RebindBinding(InputManager.Binding.Move_Up);
        });
        moveDownButton.onClick.AddListener(() =>
        {
            moveDownText.text = "";
            RebindBinding(InputManager.Binding.Move_Down);
        });
        moveLeftButton.onClick.AddListener(() =>
        {
            moveLeftText.text = "";
            RebindBinding(InputManager.Binding.Move_Left);
        });
        moveRightButton.onClick.AddListener(() =>
        {
            moveRightText.text = "";
            RebindBinding(InputManager.Binding.Move_Right);
        });
        jumpButton.onClick.AddListener(() =>
        {
            jumpText.text = "";
            RebindBinding(InputManager.Binding.Jump);
        });
        crouchButton.onClick.AddListener(() =>
        {
            crouchText.text = "";
            RebindBinding(InputManager.Binding.Crouch);
        });
        restartButton.onClick.AddListener(() =>
        {
            restartText.text = "";
            RebindBinding(InputManager.Binding.Restart);
        });
        sensitivitySlider.onValueChanged.AddListener(newValue =>
        {
            sliderValueText.text = (newValue * 10f).ToString("F");
        });
        masterSlider.onValueChanged.AddListener(newValue =>
        {
            audioMixer.SetFloat("master", Mathf.Log10(newValue) * 20);
        });
        musicSlider.onValueChanged.AddListener(newValue =>
        {
            audioMixer.SetFloat("music", Mathf.Log10(newValue) * 20);
        });

        applyButton.onClick.AddListener(() =>
        {
            Screen.fullScreen = fullscreenTog.isOn;

            if (vsyncTog.isOn)
                QualitySettings.vSyncCount = 1;
            else
                QualitySettings.vSyncCount = 0;

            int resIndex = resolutionDropdown.value;
            Screen.SetResolution(resolutions[resIndex].horizontal, resolutions[resIndex].vertical, fullscreenTog.isOn);

            PlayerPrefs.SetFloat(PLAYER_PREFS_SENSITIVITY, sensitivitySlider.value);

            PlayerPrefs.SetFloat(PLAYER_PREFS_MASTER_VOLUME, masterSlider.value);
            PlayerPrefs.SetFloat(PLAYER_PREFS_MUSIC_VOLUME, musicSlider.value);

            PlayerPrefs.Save();
        });
    }

    private void Start()
    {
        settingsMenu.SetActive(false);

        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
            vsyncTog.isOn = false;
        else
            vsyncTog.isOn = true;

        resolutions.ForEach(res =>
        {
            if (Screen.width == res.horizontal && Screen.height == res.vertical)
                resolutionDropdown.value = resolutions.IndexOf(res);
        });

        moveUpText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Up);
        moveDownText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Down);
        moveLeftText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Left);
        moveRightText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Right);
        jumpText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Jump);
        crouchText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Crouch);
        restartText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Restart);

        if (PlayerPrefs.HasKey(PLAYER_PREFS_SENSITIVITY))
            sensitivitySlider.value = PlayerPrefs.GetFloat(PLAYER_PREFS_SENSITIVITY);

        sliderValueText.text = (sensitivitySlider.value * 10).ToString("F");

        if (PlayerPrefs.HasKey(PLAYER_PREFS_MASTER_VOLUME))
        {
            masterSlider.value = PlayerPrefs.GetFloat(PLAYER_PREFS_MASTER_VOLUME);

            audioMixer.SetFloat("master", Mathf.Log10(masterSlider.value) * 20);
        }

        if (PlayerPrefs.HasKey(PLAYER_PREFS_MUSIC_VOLUME))
        {
            musicSlider.value = PlayerPrefs.GetFloat(PLAYER_PREFS_MUSIC_VOLUME);

            audioMixer.SetFloat("music", Mathf.Log10(musicSlider.value) * 20);
        }
    }

    public void Show()
    {
        settingsMenu.SetActive(true);
    }

    public void Hide()
    {
        settingsMenu.SetActive(false);
    }

    private void ShowPressToRebindKey()
    {
        pressToRebindKeyTransform.SetActive(true);
    }

    private void HidePressToRebindKey()
    {
        pressToRebindKeyTransform.SetActive(false);
    }

    private void RebindBinding(InputManager.Binding binding)
    {
        ShowPressToRebindKey();
        InputManager.Instance.RebindBinding(binding, () =>
        {
            HidePressToRebindKey();
            UpdateVisual();
        });
    }

    private void UpdateVisual()
    {
        moveUpText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Up);
        moveDownText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Down);
        moveLeftText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Left);
        moveRightText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Move_Right);
        jumpText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Jump);
        crouchText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Crouch);
        restartText.text = InputManager.Instance.GetBindingText(InputManager.Binding.Restart);
    }

    [System.Serializable]
    internal class ResItem
    {
        public int horizontal;
        public int vertical;
    }
}