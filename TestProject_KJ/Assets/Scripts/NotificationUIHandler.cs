using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;


public class NotificationUIHandler : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI[] textGUI;

    Queue messageQueue = new Queue();
    string playerName = "";
    public void NotificationReceived(string message)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            playerName = photonView.Owner.NickName;
        }
        
        messageQueue.Enqueue(message);

        if (messageQueue.Count > 3)
            messageQueue.Dequeue();

        int queueIndex = 0;
        foreach (string item in messageQueue)
        {
            textGUI[queueIndex].text = item;
            if (item == "Joined")
                textGUI[queueIndex].text = playerName + ": " + item;
            if(photonView.IsMine)
            {
                if (item == "Joined")
                    textGUI[queueIndex].text = photonView.Owner.NickName + ": " + item;
            }
            queueIndex++;
        }

    }
}
