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

    int playersTurn;

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
        }

        instructions = transform.GetChild(2).GetComponent<Text>();
        if (playersTurn == userID)
            instructions.text = "It's your turn.";
    }

    public void SlotPressed(int slot)
    {
        if (playersTurn == userID)
        {
            networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.GameButtonPressed + "," + slot);
            playSpaces[slot].GetComponentInChildren<Text>().text = playerIcon;
            Debug.Log(slot);
        }
    }

    public void UpdateSlot(int slot, int id)
    {
        if(id == 1)
            playSpaces[slot].GetComponentInChildren<Text>().text = "X";
        else if(id == 2)
            playSpaces[slot].GetComponentInChildren<Text>().text = "O";

        playSpaces[slot].interactable = false;
        EndTurn();

    }

    private void SetupGame()
    {
        if (startingPlayer == userID)
            playerIcon = "X";
        else
            playerIcon = "O";
    }
    private void EndTurn()
    {
        //if (playSpaces[0].GetComponentInChildren<Text>().text == "X" &&
        //    playSpaces[1].GetComponentInChildren<Text>().text == "X" &&
        //    playSpaces[2].GetComponentInChildren<Text>().text == "X")
        //{
        //    GameOver();
        //}
        //
        //if (buttonList[3].text == playerSide && buttonList[4].text == playerSide && buttonList[5].text == playerSide)
        //{
        //    GameOver();
        //}
        //
        //if (buttonList[6].text == playerSide && buttonList[7].text == playerSide && buttonList[8].text == playerSide)
        //{
        //    GameOver();
        //}
        //
        //if (buttonList[0].text == playerSide && buttonList[3].text == playerSide && buttonList[6].text == playerSide)
        //{
        //    GameOver();
        //}
        //
        //if (buttonList[1].text == playerSide && buttonList[4].text == playerSide && buttonList[7].text == playerSide)
        //{
        //    GameOver();
        //}
        //
        //if (buttonList[2].text == playerSide && buttonList[5].text == playerSide && buttonList[8].text == playerSide)
        //{
        //    GameOver();
        //}
        //
        //if (buttonList[0].text == playerSide && buttonList[4].text == playerSide && buttonList[8].text == playerSide)
        //{
        //    GameOver();
        //}
        //
        //if (buttonList[2].text == playerSide && buttonList[4].text == playerSide && buttonList[6].text == playerSide)
        //{
        //    GameOver();
        //}
    }
}

