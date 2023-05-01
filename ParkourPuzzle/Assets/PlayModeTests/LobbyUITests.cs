using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LobbyUITests
{
    [SetUp]
    public void Setup()
    {
        SceneManager.LoadScene("LobbyScene");
    }

    [UnityTest]
    public IEnumerator LobbySceneLoaded()
    {
        Assert.AreEqual("LobbyScene", SceneManager.GetActiveScene().name);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RendersElements()
    {
        GameObject backToMainBtn = GameObject.Find("BackToMainButton");
        GameObject quickJoinBtn = GameObject.Find("QuickJoinButton");
        GameObject createLobbyBtn = GameObject.Find("CreateLobbyButton");
        GameObject playerNameInput = GameObject.Find("PlayerNameInputField");
        GameObject lobbyList = GameObject.Find("LobbyListContainer");

        Assert.IsNotNull(backToMainBtn);
        Assert.IsNotNull(quickJoinBtn);
        Assert.IsNotNull(createLobbyBtn);
        Assert.IsNotNull(playerNameInput);
        Assert.IsNotNull(lobbyList);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RendersLobbyCreateUIOnButtonClick()
    {
        GameObject createLobbyBtn = GameObject.Find("CreateLobbyButton");

        createLobbyBtn.GetComponent<Button>().onClick.Invoke();

        GameObject lobbyCreateUI = GameObject.Find("LobbyCreateUI");

        Assert.IsTrue(lobbyCreateUI.activeSelf);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RendersLobbyCreateUIElements()
    {
        GameObject createLobby = GameObject.Find("CreateLobbyButton");

        createLobby.GetComponent<Button>().onClick.Invoke();

        GameObject label = GameObject.Find("LobbyCreateUI/label");
        GameObject lobbyNameInput = GameObject.Find("LobbyCreateUI/LobbyNameInputField");
        GameObject backBtn = GameObject.Find("LobbyCreateUI/BackButton");
        GameObject createLobbyBtn = GameObject.Find("LobbyCreateUI/CreateLobbyButton");
        GameObject mapSelectItem = GameObject.Find("LobbyCreateUI/MapSelectItem");

        Assert.IsNotNull(label);
        Assert.IsNotNull(lobbyNameInput);
        Assert.IsNotNull(backBtn);
        Assert.IsNotNull(createLobbyBtn);
        Assert.IsNotNull(mapSelectItem);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RendersLobbyMessageUIOnJoinStartedEvent()
    {
        GameObject quickJoinBtn = GameObject.Find("QuickJoinButton");
        quickJoinBtn.GetComponent<Button>().onClick.Invoke();

        GameObject lobbyMessageUI = GameObject.Find("LobbyMessageUI");

        Assert.IsTrue(lobbyMessageUI.activeSelf);

        TextMeshProUGUI text = GameObject.Find("LobbyMessageUI/message").GetComponent<TextMeshProUGUI>();

        Assert.AreEqual("Joining Lobby...", text.text);

        yield return null;
    }

    [UnityTest]
    public IEnumerator RendersLobbyMessageUIOnCreateLobbyStartedEvent()
    {
        GameObject createLobbyBtn = GameObject.Find("CreateLobbyButton");
        createLobbyBtn.GetComponent<Button>().onClick.Invoke();
        GameObject createLobbyBtn2 = GameObject.Find("LobbyCreateUI/CreateLobbyButton");
        createLobbyBtn2.GetComponent<Button>().onClick.Invoke();

        GameObject lobbyMessageUI = GameObject.Find("LobbyMessageUI");

        Assert.IsTrue(lobbyMessageUI.activeSelf);

        TextMeshProUGUI text = GameObject.Find("LobbyMessageUI/message").GetComponent<TextMeshProUGUI>();

        Assert.AreEqual("Creating Lobby...", text.text);

        yield return null;
    }
}
