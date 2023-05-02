using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerUITests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("MainMenu");
    }

    [UnityTest]
    public IEnumerator ShouldRenderPlayerUI()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        Assert.IsTrue(PlayerUI.Instance.gameObject.activeSelf);
        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldRenderPlayerUIElements()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        GameObject velocity = GameObject.Find("PlayerUI/velocity");
        GameObject timer = GameObject.Find("PlayerUI/timer");

        Assert.IsNotNull(velocity);
        Assert.IsNotNull(timer);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldRenderInitialTextValues()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        GameObject velocity = GameObject.Find("PlayerUI/velocity");
        GameObject timer = GameObject.Find("PlayerUI/timer");

        Assert.AreEqual("0", velocity.GetComponent<TextMeshProUGUI>().text);
        Assert.AreEqual("00:00:00", timer.GetComponent<TextMeshProUGUI>().text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldUpdateVelocityUIValue()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        GameObject velocity = GameObject.Find("PlayerUI/velocity");
        PlayerUI.Instance.UpdateVelocityUI(3000);

        Assert.AreEqual("3000", velocity.GetComponent<TextMeshProUGUI>().text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator ShouldUpdateTimerUIValue()
    {
        LoadMap();

        yield return new WaitForSeconds(1f);

        GameObject timer = GameObject.Find("PlayerUI/timer");
        PlayerUI.Instance.UpdateTimerUI(10);

        Assert.AreEqual("00:10:00", timer.GetComponent<TextMeshProUGUI>().text);

        yield return null;
    }

    private static void LoadMap()
    {
        GameObject singleplayerBtn = GameObject.Find("SingleplayerButton");
        singleplayerBtn.GetComponent<Button>().onClick.Invoke();
        GameObject playBtn = GameObject.Find("MapSelectUI/PlayButton");
        playBtn.GetComponent<Button>().onClick.Invoke();
    }
}
