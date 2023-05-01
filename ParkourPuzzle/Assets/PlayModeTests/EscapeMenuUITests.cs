using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EscapeMenuUITests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [UnityTest]
    public IEnumerator ShouldRenderEscapeMenuUI()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        EscapeMenuUI.Instance.Show();

        Assert.IsTrue(EscapeMenuUI.Instance.gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldRenderEscapeMenuUIElements()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        EscapeMenuUI.Instance.Show();

        GameObject backToMainBtn = GameObject.Find("BackToMainButton");
        GameObject settingsBtn = GameObject.Find("SettingsButton");
        GameObject backToGameBtn = GameObject.Find("BackToGameButton");
        GameObject leaderboardBtn = GameObject.Find("LeaderboardButton");

        Assert.IsNotNull(backToMainBtn);
        Assert.IsNotNull(settingsBtn);
        Assert.IsNotNull(backToGameBtn);
        Assert.IsNotNull(leaderboardBtn);

        Assert.IsFalse(SettingsUI.Instance.gameObject.activeSelf);
        Assert.IsFalse(LeaderboardUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldOpenSettingsUIWhenButtonClicked()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        EscapeMenuUI.Instance.Show();

        GameObject settingsBtn = GameObject.Find("SettingsButton");
        settingsBtn.GetComponent<Button>().onClick.Invoke();

        Assert.IsTrue(SettingsUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldCloseSettingsUIWhenButtonClicked()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        EscapeMenuUI.Instance.Show();

        GameObject settingsBtn = GameObject.Find("SettingsButton");
        settingsBtn.GetComponent<Button>().onClick.Invoke();

        Assert.IsTrue(SettingsUI.Instance.gameObject.activeSelf);

        GameObject closeBtn = GameObject.Find("SettingsUI/BackButton");
        closeBtn.GetComponent<Button>().onClick.Invoke();

        Assert.IsFalse(SettingsUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldCloseEscapeMenuUIOnButtonClick()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        EscapeMenuUI.Instance.Show();

        GameObject backToGameBtn = GameObject.Find("BackToGameButton");
        backToGameBtn.GetComponent<Button>().onClick.Invoke();

        Assert.IsFalse(EscapeMenuUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldOpenLeaderboardUIWhenButtonClicked()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        EscapeMenuUI.Instance.Show();

        GameObject leaderboardBtn = GameObject.Find("LeaderboardButton");
        leaderboardBtn.GetComponent<Button>().onClick.Invoke();

        Assert.IsTrue(LeaderboardUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldLoadMainMenuWhenButtonClicked()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        EscapeMenuUI.Instance.Show();

        GameObject backToMainBtn = GameObject.Find("BackToMainButton");
        backToMainBtn.GetComponent<Button>().onClick.Invoke();

        yield return new WaitForSeconds(1f);

        Assert.AreEqual("MainMenu", SceneManager.GetActiveScene().name);
    }

    private static void LoadMap()
    {
        GameObject singleplayerBtn = GameObject.Find("SingleplayerButton");
        singleplayerBtn.GetComponent<Button>().onClick.Invoke();
        GameObject playBtn = GameObject.Find("MapSelectUI/PlayButton");
        playBtn.GetComponent<Button>().onClick.Invoke();
    }
}
