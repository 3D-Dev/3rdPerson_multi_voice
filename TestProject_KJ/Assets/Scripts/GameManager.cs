using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _spawnPoint;
    NotificationUIHandler notificationHandler;

    void Start()
    {
        var newPlayer = PhotonNetwork.Instantiate(_playerPrefab.name, _spawnPoint.position, Quaternion.identity);
        
        // If we're the host
        if (PhotonNetwork.IsMasterClient)
        {
            // Start as tagged
            newPlayer.GetComponent<PlayerController>().photonView.RPC("OnTagged", RpcTarget.AllBuffered);            
        }
        if (notificationHandler == null)
            notificationHandler = GameObject.Find("UI").GetComponentInChildren<NotificationUIHandler>();
        if (notificationHandler != null)
            notificationHandler.NotificationReceived("Joined");
    }

    public override void OnPlayerEnteredRoom(Player other)
	{
		Debug.Log("OnPlayerEnteredRoom() " + other.NickName); // not seen if you're the player connecting

		if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

			notificationHandler.NotificationReceived(other.NickName + ": " + "Joined");
		}
	}

	/// <summary>
	/// Called when a Photon Player got disconnected. We need to load a smaller scene.
	/// </summary>
	/// <param name="other">Other.</param>
	public override void OnPlayerLeftRoom(Player other)
	{
		Debug.Log("OnPlayerLeftRoom() " + other.NickName); // seen when other disconnects

		if (PhotonNetwork.IsMasterClient)
		{
			Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

			notificationHandler.NotificationReceived(other.NickName + ": " + "Left");
		}
	}

	/// <summary>
	/// Called when the local player left the room. We need to load the launcher scene.
	/// </summary>
	public override void OnLeftRoom()
	{
		notificationHandler.NotificationReceived(photonView.Owner.NickName + ": " + "Left");
	}	
}
