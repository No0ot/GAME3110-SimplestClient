using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameObject networkedClient;
    public List<Button> playSpaces;
    Text instructions;

    public int player1ID = 0;
    public int player2ID = 0;
    public int startingPlayer;
    int moveCount;

    string playerIcon;

    string yourTurnText = "It's your turn.";
    string opponentTurnText = "It's your opponent's turn.";

    public int playersTurn = 1;

    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == "NetworkedClient")
                networkedClient = go;  
        }
        for(int i = 0; i < transform.GetChild(0).childCount; i++)
        {
            playSpaces.Add(transform.GetChild(0).GetChild(i).GetComponent<Button>());
        }

        foreach(Button but in playSpaces)
        {
            but.GetComponentInChildren<Text>().text = "";
            but.interactable = true;
        }

        instructions = transform.GetChild(2).GetComponent<Text>();
        if (playersTurn == player1ID)
            instructions.text = yourTurnText;
        else
            instructions.text = opponentTurnText;
    }

    public void SlotPressed(int slot)
    {
        if (playersTurn == player1ID)
        {
            //playSpaces[slot].GetComponentInChildren<Text>().text = playerIcon;
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.GameButtonPressed + "," + slot + "," + playerIcon);
            //Debug.Log(slot);
        }
    }

    public void UpdateSlot(int slot, string playericon)
    {
        playSpaces[slot].GetComponentInChildren<Text>().text = playericon;

        playSpaces[slot].interactable = false;
        EndTurn(playericon);

    }

    public void SetupGame()
    {
        if (startingPlayer == player1ID)
            playerIcon = "X";
        else
            playerIcon = "O";
    }
    private void EndTurn(string currentPlayersIcon)
    {
        moveCount++;
        //Check Row 1
        if (playSpaces[0].GetComponentInChildren<Text>().text == currentPlayersIcon &&
            playSpaces[1].GetComponentInChildren<Text>().text == currentPlayersIcon &&
            playSpaces[2].GetComponentInChildren<Text>().text == currentPlayersIcon)
        {
            GameOver(currentPlayersIcon);
        }
        //Check Row 2
        if (playSpaces[3].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[4].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[5].GetComponentInChildren<Text>().text == currentPlayersIcon)
        {
            GameOver(currentPlayersIcon);
        }
        //Check Row 3
        if (playSpaces[6].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[7].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[8].GetComponentInChildren<Text>().text == currentPlayersIcon)
        {
            GameOver(currentPlayersIcon);
        }
        //Check Col 1
        if (playSpaces[0].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[3].GetComponentInChildren<Text>().text == currentPlayersIcon &&
            playSpaces[6].GetComponentInChildren<Text>().text == currentPlayersIcon)
        {
            GameOver(currentPlayersIcon);
        }
        //Check Col 2
        if (playSpaces[1].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[4].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[7].GetComponentInChildren<Text>().text == currentPlayersIcon)
        {
            GameOver(currentPlayersIcon);
        }
        //Check Col 3
        if (playSpaces[2].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[5].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[8].GetComponentInChildren<Text>().text == currentPlayersIcon)
        {
            GameOver(currentPlayersIcon);
        }
        //Check Left to Right Diagonal
        if (playSpaces[0].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[4].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[8].GetComponentInChildren<Text>().text == currentPlayersIcon)
        {
            GameOver(currentPlayersIcon);
        }
        // Check Right to Left Diagonal
        if (playSpaces[2].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[4].GetComponentInChildren<Text>().text == currentPlayersIcon && 
            playSpaces[6].GetComponentInChildren<Text>().text == currentPlayersIcon)
        {
            GameOver(currentPlayersIcon);
        }
        if (moveCount > 9)
            GameOver("draw");

        playersTurn = (playersTurn == 1) ? 2 : 1;
        instructions.text = (instructions.text == yourTurnText) ? opponentTurnText : yourTurnText;
    }

    private void GameOver(string currentPlayersIcon)
    {
        if (currentPlayersIcon != "draw")
        {
            string wintext = "'" + currentPlayersIcon + "'" + " Wins";
            instructions.text = wintext;
        }
        else
            instructions.text = "Game ends in a draw";

        foreach (Button but in playSpaces)
        {
            but.interactable = false;
        }

        UImanager.Instance.ChangeState(GameStates.End);
    }
}

