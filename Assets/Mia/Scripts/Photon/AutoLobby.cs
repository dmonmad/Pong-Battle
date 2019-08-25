using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoLobby : MonoBehaviourPunCallbacks
{
    public string DefaultName = "DEFAULTPLAYER";

    public Button ConnectButton;
    public Button JoinRandomButton;
    public Text log;
    public Text PlayersCount;
    public InputField PlayerNameField;
    public Text PlayerName;
    public int playersCount = 0;
    public string serverRegion = "us";

    public byte maxPlayersPerRoom = 4;
    public byte minPlayersPerRoom = 2;
    private bool isLoading = false;


    public void Connect()
    {
        //PhotonNetwork.ConnectToRegion(serverRegion);
        if (!PhotonNetwork.IsConnected)
        {
            if (PhotonNetwork.ConnectUsingSettings())
            {
                log.text += "\nConnected to server " + PhotonNetwork.ServerAddress;
            }
            else
            {
                log.text += "\nThere was a problem connecting to server!";
            }
        }

    }

    public override void OnConnectedToMaster()
    {
        ConnectButton.interactable = false;
        PlayerNameField.interactable = true;
        JoinRandomButton.interactable = true;

    }

    public void JoinRandom()
    {
        if (!PhotonNetwork.JoinRandomRoom())
        {
            log.text += "\nFail joining";
        }
        
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        log.text += "\nNo rooms found. Creating one...";

        if(PhotonNetwork.CreateRoom(null, new Photon.Realtime.RoomOptions() { MaxPlayers = maxPlayersPerRoom }))
        {
            log.text += "\nRoom created!";

        }
        else
        {
            log.text += "\nError creating the room";
        }


    }

    public override void OnJoinedRoom()
    {
        log.text += "\nRoom joined!";
        JoinRandomButton.interactable = false;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            playersCount = PhotonNetwork.CurrentRoom.PlayerCount;
            PlayersCount.text = "Players: " + playersCount + " / " + maxPlayersPerRoom;
            
            if (!isLoading && playersCount >= minPlayersPerRoom)
            {
                PhotonNetwork.player
                LoadMap();
            }

        }

        

    }

    private void LoadMap()
    {
        if (!isLoading)
        {
            isLoading = true;

            PhotonNetwork.LoadLevel("BallBattle");
        }
    }

}
