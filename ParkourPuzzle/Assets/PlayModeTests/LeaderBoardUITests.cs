using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LeaderBoardUITests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [UnityTest]
    public IEnumerator ShouldRenderLeaderBoardUI()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        OpenLeaderboardMenu();

        Assert.IsTrue(LeaderboardUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldRenderLeaderBoardUIElements()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        OpenLeaderboardMenu();

        GameObject title = GameObject.Find("LeaderboardUI/Title");
        GameObject posText = GameObject.Find("LeaderboardUI/posText");
        GameObject nameText = GameObject.Find("LeaderboardUI/nameText");
        GameObject timeText = GameObject.Find("LeaderboardUI/timeText");
        GameObject leaderboardContainer = GameObject.Find("LeaderboardUI/leaderboardContainer");
        GameObject backButton = GameObject.Find("LeaderboardUI/BackButton");

        Assert.IsNotNull(title);
        Assert.IsNotNull(posText);
        Assert.IsNotNull(nameText);
        Assert.IsNotNull(timeText);
        Assert.IsNotNull(leaderboardContainer);
        Assert.IsNotNull(backButton);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldRenderCorrectText()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        OpenLeaderboardMenu();

        GameObject title = GameObject.Find("LeaderboardUI/Title");
        GameObject posText = GameObject.Find("LeaderboardUI/posText");
        GameObject nameText = GameObject.Find("LeaderboardUI/nameText");
        GameObject timeText = GameObject.Find("LeaderboardUI/timeText");

        Assert.AreEqual("LEADERBOARD", title.GetComponent<TextMeshProUGUI>().text);
        Assert.AreEqual("POS", posText.GetComponent<TextMeshProUGUI>().text);
        Assert.AreEqual("NAME", nameText.GetComponent<TextMeshProUGUI>().text);
        Assert.AreEqual("TIME", timeText.GetComponent<TextMeshProUGUI>().text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldCloseLeaderboardMenuWhenButtonClicked()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        OpenLeaderboardMenu();

        GameObject backButton = GameObject.Find("LeaderboardUI/BackButton");
        backButton.GetComponent<Button>().onClick.Invoke();

        Assert.IsFalse(LeaderboardUI.Instance.gameObject.activeSelf);

        yield return null;
    }

    private static void LoadMap()
    {
        GameObject singleplayerBtn = GameObject.Find("SingleplayerButton");
        singleplayerBtn.GetComponent<Button>().onClick.Invoke();
        GameObject playBtn = GameObject.Find("MapSelectUI/PlayButton");
        playBtn.GetComponent<Button>().onClick.Invoke();
    }

    private static void OpenLeaderboardMenu()
    {
        EscapeMenuUI.Instance.Show();

        GameObject leaderboardBtn = GameObject.Find("LeaderboardButton");
        leaderboardBtn.GetComponent<Button>().onClick.Invoke();
    }
}
