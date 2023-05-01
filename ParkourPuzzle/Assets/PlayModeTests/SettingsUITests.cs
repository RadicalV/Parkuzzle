using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SettingsUITests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [UnityTest]
    public IEnumerator ShouldRenderElements()
    {
        GameObject settingsBtn = GameObject.Find("SettingsButton");
        settingsBtn.GetComponent<Button>().onClick.Invoke();

        GameObject title = GameObject.Find("SettingsUI/Title");
        GameObject backBtn = GameObject.Find("SettingsUI/BackButton");
        GameObject videoSettingsButton = GameObject.Find("SettingsUI/SettingsButtons/VideoSettingsButton");
        GameObject audioSettingsButton = GameObject.Find("SettingsUI/SettingsButtons/AudioSettingsButton");
        GameObject controlsSettingsButton = GameObject.Find("SettingsUI/SettingsButtons/ControlsSettingsButton");
        GameObject videoSettings = GameObject.Find("SettingsUI/VideoSettings");
        GameObject applyButton = GameObject.Find("SettingsUI/ApplyButton");

        Assert.IsNotNull(title);
        Assert.AreEqual("GAME SETTINGS", title.GetComponent<TextMeshProUGUI>().text);
        Assert.IsNotNull(backBtn);
        Assert.IsNotNull(videoSettingsButton);
        Assert.IsNotNull(audioSettingsButton);
        Assert.IsNotNull(controlsSettingsButton);
        Assert.IsTrue(videoSettings.activeSelf);
        Assert.IsNotNull(applyButton);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldRenderAudioSettingsOnButtonClick()
    {
        GameObject settingsBtn = GameObject.Find("SettingsButton");
        settingsBtn.GetComponent<Button>().onClick.Invoke();

        GameObject audioSettingsButton = GameObject.Find("SettingsUI/SettingsButtons/AudioSettingsButton");
        audioSettingsButton.GetComponent<Button>().onClick.Invoke();

        GameObject audioSettings = GameObject.Find("SettingsUI/AudioSettings");

        Assert.IsTrue(audioSettings.activeSelf);

        GameObject masterVolumeItem = GameObject.Find("SettingsUI/AudioSettings/MasterVolumeItem/label");
        GameObject musicVolumeItem = GameObject.Find("SettingsUI/AudioSettings/MusicVolumeItem/label");

        Assert.AreEqual("Master", masterVolumeItem.GetComponent<TextMeshProUGUI>().text);
        Assert.AreEqual("Music", musicVolumeItem.GetComponent<TextMeshProUGUI>().text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldRenderControlsSettingsOnButtonClick()
    {
        GameObject settingsBtn = GameObject.Find("SettingsButton");
        settingsBtn.GetComponent<Button>().onClick.Invoke();

        GameObject controlsSettingsButton = GameObject.Find("SettingsUI/SettingsButtons/ControlsSettingsButton");
        controlsSettingsButton.GetComponent<Button>().onClick.Invoke();

        GameObject controlsSettings = GameObject.Find("SettingsUI/ControlsSettings");

        Assert.IsTrue(controlsSettings.activeSelf);

        TextMeshProUGUI moveUpText = GameObject.Find("SettingsUI/ControlsSettings/MoveUpText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI moveLeftText = GameObject.Find("SettingsUI/ControlsSettings/MoveLeftText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI moveDownText = GameObject.Find("SettingsUI/ControlsSettings/MoveDownText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI moveRightText = GameObject.Find("SettingsUI/ControlsSettings/MoveRightText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI restartText = GameObject.Find("SettingsUI/ControlsSettings/RestartText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI jumpText = GameObject.Find("SettingsUI/ControlsSettings/JumpText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI crouchText = GameObject.Find("SettingsUI/ControlsSettings/CrouchText").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI mouseSensitivityText = GameObject.Find("SettingsUI/ControlsSettings/MouseSensitivityText").GetComponent<TextMeshProUGUI>();

        Assert.AreEqual("Move Up", moveUpText.text);
        Assert.AreEqual("Move Left", moveLeftText.text);
        Assert.AreEqual("Move Down", moveDownText.text);
        Assert.AreEqual("Move Right", moveRightText.text);
        Assert.AreEqual("Restart", restartText.text);
        Assert.AreEqual("Jump", jumpText.text);
        Assert.AreEqual("Crouch", crouchText.text);
        Assert.AreEqual("Mouse sensitivity", mouseSensitivityText.text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldRenderVideoSettingsOnButtonClick()
    {
        GameObject settingsBtn = GameObject.Find("SettingsButton");
        settingsBtn.GetComponent<Button>().onClick.Invoke();

        GameObject videoSettingsButton = GameObject.Find("SettingsUI/SettingsButtons/VideoSettingsButton");
        videoSettingsButton.GetComponent<Button>().onClick.Invoke();

        GameObject videoSettings = GameObject.Find("SettingsUI/VideoSettings");

        Assert.IsTrue(videoSettings.activeSelf);

        TextMeshProUGUI fullscreenItem = GameObject.Find("SettingsUI/VideoSettings/FullscreenItem/label").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI vsyncItem = GameObject.Find("SettingsUI/VideoSettings/VsyncItem/label").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI resolutionItem = GameObject.Find("SettingsUI/VideoSettings/ResolutionItem/label").GetComponent<TextMeshProUGUI>();

        Assert.AreEqual("Fullscreen", fullscreenItem.text);
        Assert.AreEqual("Vsync", vsyncItem.text);
        Assert.AreEqual("Resolution", resolutionItem.text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldHideSettingsOnBackBtnClick()
    {
        GameObject settingsBtn = GameObject.Find("SettingsButton");
        settingsBtn.GetComponent<Button>().onClick.Invoke();

        Assert.IsTrue(SettingsUI.Instance.gameObject.activeSelf);

        GameObject backBtn = GameObject.Find("SettingsUI/BackButton");
        backBtn.GetComponent<Button>().onClick.Invoke();

        Assert.IsFalse(SettingsUI.Instance.gameObject.activeSelf);

        yield return null;
    }
}
