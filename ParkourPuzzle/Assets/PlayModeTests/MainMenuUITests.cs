using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuUITests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [UnityTest]
    public IEnumerator MainMenuUILoaded()
    {
        Assert.AreEqual("MainMenu", SceneManager.GetActiveScene().name);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RendersElements()
    {
        GameObject singeplayerBtn = GameObject.Find("SingleplayerButton");
        GameObject multiplayerBtn = GameObject.Find("MultiplayerButton");
        GameObject settingsBtn = GameObject.Find("SettingsButton");
        GameObject quitBtn = GameObject.Find("QuitButton");
        GameObject logo = GameObject.Find("SurfHopperLogo");

        Assert.IsNotNull(singeplayerBtn);
        Assert.IsNotNull(multiplayerBtn);
        Assert.IsNotNull(settingsBtn);
        Assert.IsNotNull(quitBtn);
        Assert.IsNotNull(logo);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SettingsAndMapSelectShouldBeDisabled()
    {
        Assert.IsFalse(SettingsUI.Instance.gameObject.activeSelf);
        Assert.IsFalse(MapSelectUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RendersSettingsUIWhenButtonClicked()
    {
        GameObject settingsBtn = GameObject.Find("SettingsButton");

        settingsBtn.GetComponent<Button>().onClick.Invoke();

        Assert.IsTrue(SettingsUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RendersMapSelectUIWhenSingleplayerButtonClicked()
    {
        GameObject singleplayerBtn = GameObject.Find("SingleplayerButton");

        singleplayerBtn.GetComponent<Button>().onClick.Invoke();

        Assert.IsTrue(MapSelectUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator LoadsLobbySceneWhenMultiplayerButtonClicked()
    {
        GameObject multiplayerBtn = GameObject.Find("MultiplayerButton");

        multiplayerBtn.GetComponent<Button>().onClick.Invoke();

        yield return new WaitForSeconds(0.01f);

        Assert.AreEqual("LoadingScene", SceneManager.GetActiveScene().name);

        yield return new WaitForSeconds(1f);

        Assert.AreEqual("LobbyScene", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator ShouldLoadSurfUtopiaMapByDefaultWhenPlayButtonClicked()
    {
        GameObject singleplayerBtn = GameObject.Find("SingleplayerButton");
        singleplayerBtn.GetComponent<Button>().onClick.Invoke();

        GameObject playBtn = GameObject.Find("MapSelectUI/PlayButton");
        playBtn.GetComponent<Button>().onClick.Invoke();

        yield return new WaitForSeconds(1f);

        Assert.AreEqual("Surf_Utopia", SceneManager.GetActiveScene().name);
    }

    [UnityTest]
    public IEnumerator ShouldLoadSelectedMapWhenPlayButtonClicked()
    {
        GameObject singleplayerBtn = GameObject.Find("SingleplayerButton");
        singleplayerBtn.GetComponent<Button>().onClick.Invoke();

        GameObject playBtn = GameObject.Find("MapSelectUI/PlayButton");
        GameObject map2Toggle = GameObject.Find("MapSelectUI/mapList/ToggleMap2");

        map2Toggle.GetComponent<Toggle>().onValueChanged.Invoke(true);
        playBtn.GetComponent<Button>().onClick.Invoke();

        yield return new WaitForSeconds(1f);

        Assert.AreEqual("Surf_Kitsune", SceneManager.GetActiveScene().name);
    }
}
