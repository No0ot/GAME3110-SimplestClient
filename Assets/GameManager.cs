using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameObject networkedClient;
    public List<Button> playSpaces;
    Text instructions;

    public int userID = 0;
    public int opponentID = 0;
    public int startingPlayer;

    string playerIcon;

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
        if (playersTurn == userID)
            instructions.text = "It's your turn.";
    }

    public void SlotPressed(int slot)
    {
        if (playersTurn == userID)
        {
            //playSpaces[slot].GetComponentInChildren<Text>().text = playerIcon;
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.GameButtonPressed + "," + slot + "," + playerIcon);
            Debug.Log(slot);
        }
    }

    public void UpdateSlot(int slot, string playericon)
    {
        playSpaces[slot].GetComponentInChildren<Text>().text = playericon;

        playSpaces[slot].interactable = false;
        EndTurn();

    }

    public void SetupGame()
    {
        if (startingPlayer == userID)
            playerIcon = "X";
        else
            playerIcon = "O";
    }
    private void EndTurn()
    {
        //Check Row 1
        if (playSpaces[0].GetComponentInChildren<Text>().text == playerIcon &&
            playSpaces[1].GetComponentInChildren<Text>().text == playerIcon &&
            playSpaces[2].GetComponentInChildren<Text>().text == playerIcon)
        {
            GameOver();
        }
        //Check Row 2
        if (playSpaces[3].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[4].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[5].GetComponentInChildren<Text>().text == playerIcon)
        {
            GameOver();
        }
        //Check Row 3
        if (playSpaces[6].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[7].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[8].GetComponentInChildren<Text>().text == playerIcon)
        {
            GameOver();
        }
        //Check Col 1
        if (playSpaces[0].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[3].GetComponentInChildren<Text>().text == playerIcon &&
            playSpaces[6].GetComponentInChildren<Text>().text == playerIcon)
        {
            GameOver();
        }
        //Check Col 2
        if (playSpaces[1].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[4].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[7].GetComponentInChildren<Text>().text == playerIcon)
        {
            GameOver();
        }
        //Check Col 3
        if (playSpaces[2].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[5].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[8].GetComponentInChildren<Text>().text == playerIcon)
        {
            GameOver();
        }
        //Check Left to Right Diagonal
        if (playSpaces[0].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[4].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[8].GetComponentInChildren<Text>().text == playerIcon)
        {
            GameOver();
        }
        // Check Right to Left Diagonal
        if (playSpaces[2].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[4].GetComponentInChildren<Text>().text == playerIcon && 
            playSpaces[6].GetComponentInChildren<Text>().text == playerIcon)
        {
            GameOver();
        }

        playersTurn = (playersTurn == 1) ? 2 : 1;
    }

    private void GameOver()
    {
        Debug.Log("'" + playerIcon + "'" + " Wins");
        foreach (Button but in playSpaces)
        {
            but.interactable = false;
        }
    }
}

