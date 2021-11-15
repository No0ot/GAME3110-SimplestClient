using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    GameObject submitButton, userNameInput, passwordInput, createToggle, loginToggle, joinButton, gameButton, mainMenuButton, observerButton;
    GameObject loginPanel, mainMenuPanel, queuepanel, gamePanel, endPanel;

    public ChatScript chatManager;
    GameObject networkedClient;

    private static UImanager instance;
    public static UImanager Instance { get { return instance; } }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach(GameObject go in allObjects)
        {
            if (go.name == "UsernameInput")
                userNameInput = go;
            else if (go.name == "PasswordInput")
                passwordInput = go;
            else if (go.name == "CreateToggle")
                createToggle = go;
            else if (go.name == "LoginToggle")
                loginToggle = go;
            else if (go.name == "SubmitButton")
                submitButton = go;
            else if (go.name == "NetworkedClient")
                networkedClient = go;
            else if (go.name == "JoinGameRoomButton")
                joinButton = go;
            else if (go.name == "LoginMenuPanel")
                loginPanel = go;
            else if (go.name == "MainMenuPanel")
                mainMenuPanel = go;
            else if (go.name == "GamePanel")
                gamePanel = go;
            else if (go.name == "QueuePanel")
                queuepanel = go;
            else if (go.name == "EndPanel")
                endPanel = go;
            else if (go.name == "GameButton")
                gameButton = go;
            else if (go.name == "JoinAsObserverButton")
                observerButton = go;
            else if (go.name == "ReturnToMainMenuButton")
                mainMenuButton = go;
            else if (go.name == "ChatPanel")
                chatManager = go.GetComponent<ChatScript>();
        }
        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        joinButton.GetComponent<Button>().onClick.AddListener(JoinGameButtonPressed);
        observerButton.GetComponent<Button>().onClick.AddListener(WatchAsObserverButtonPressed);
        mainMenuButton.GetComponent<Button>().onClick.AddListener(MainMenuButtonPressed);

        ChangeState(GameStates.LoginMenu);
    }

    public void SubmitButtonPressed()
    {
        string n = userNameInput.GetComponent<InputField>().text;
        string p = passwordInput.GetComponent<InputField>().text;

        string msg;
        if(createToggle.GetComponent<Toggle>().isOn)
            msg = ClientToServerSignifiers.CreateAccount + "," + n + "," + p;
        else
            msg = ClientToServerSignifiers.LoginAccount + "," + n + "," + p;

        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(msg);
    }

    public void JoinGameButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinQueue + "");
        ChangeState(GameStates.WaitingInQueue);
    }

    public void WatchAsObserverButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.JoinAsObserver + "");
        ChangeState(GameStates.WaitingInQueue);
    }

    public void MainMenuButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.LeaveRoom + "");
        ChangeState(GameStates.MainMenu);
    }

    public void GameButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.GameButtonPressed + "");
        //ChangeState(GameStates.Game);
    }

    public void ChangeState(int newState)
    {
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(false);
        gamePanel.SetActive(false);
        queuepanel.SetActive(false);
        endPanel.SetActive(false);

        switch (newState)
        {
            case GameStates.LoginMenu:
                loginPanel.SetActive(true);
                break;
            case GameStates.MainMenu:
                mainMenuPanel.SetActive(true);
                break;
            case GameStates.WaitingInQueue:
                queuepanel.SetActive(true);
                break;
            case GameStates.Game:
                gamePanel.SetActive(true);
                break;
            case GameStates.End:
                gamePanel.SetActive(true);
                endPanel.SetActive(true);
                break;
        }
    }
}

static public class GameStates
{
    public const int LoginMenu = 1;
    public const int MainMenu = 2;
    public const int WaitingInQueue = 3;
    public const int Game = 4;
    public const int End = 5;
}
