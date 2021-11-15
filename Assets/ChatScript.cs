using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatScript : MonoBehaviour
{
    GameObject networkedClient;

    public Text chatBox;
    List<string> chatMessages;

    private void Start()
    {
        chatMessages = new List<string>();
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject go in allObjects)
        {
            if (go.name == "NetworkedClient")
                networkedClient = go;
        }
    }


    public void PrefixedMessageButtonpressed(Button pressedButton)
    {
        string msg = pressedButton.GetComponentInChildren<Text>().text;
        networkedClient.GetComponent<NetworkedClient>().SendMessageToHost(ClientToServerSignifiers.ChatMessageSent + "," + msg);
    }

    public void UpdateChatLog(string playername, string chatMsg)
    {
        string newMsg = playername + ": " + chatMsg;
        chatMessages.Add(newMsg);
        if(chatMessages.Count > 9)
        {
            chatMessages.RemoveAt(0);
        }
        chatBox.text = "";
        foreach (string msg in chatMessages)
        {
            chatBox.text += msg + "\n";
        }
    }
}
