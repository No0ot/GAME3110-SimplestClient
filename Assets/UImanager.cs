using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UImanager : MonoBehaviour
{
    GameObject submitButton, userNameInput, passwordInput, createToggle, loginToggle, joinButton, gameButton;
    GameObject loginPanel, mainMenuPanel, queuepanel, gamePanel;


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
            else if (go.name == "GameButton")
                gameButton = go;
        }
        submitButton.GetComponent<Button>().onClick.AddListener(SubmitButtonPressed);
        joinButton.GetComponent<Button>().onClick.AddListener(JoinGameButtonPressed);
        gameButton.GetComponent<Button>().onClick.AddListener(GameButtonPressed);

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

    public void GameButtonPressed()
    {
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.GameButtonPressed + "");
        ChangeState(GameStates.Game);
    }

    public void ChangeState(int newState)
    {
        mainMenuPanel.SetActive(false);
        loginPanel.SetActive(false);
        gamePanel.SetActive(false);
        queuepanel.SetActive(false);

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
        }
    }
}

static public class GameStates
{
    public const int LoginMenu = 1;
    public const int MainMenu = 2;
    public const int WaitingInQueue = 3;
    public const int Game = 4;
}
