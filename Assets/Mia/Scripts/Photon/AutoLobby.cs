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

    public Text CountDown;
    public float CountDownTimer = 10;
    public bool CanStartCount = false;
    public bool CanLoadLevel = false;


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
        PlayerNameField.interactable = true;
        JoinRandomButton.interactable = true;
        PhotonNetwork.AutomaticallySyncScene = true;
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
        int roomNumber = PhotonNetwork.CountOfRooms + 1;
        if (PhotonNetwork.CreateRoom("Room " + roomNumber, new Photon.Realtime.RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = maxPlayersPerRoom }))
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
        if (PhotonNetwork.IsMasterClient)
        {
            log.text += "\n##### You are the Master Client";
        }
        else
        {
            log.text += "\n---- YOU ARENT MASTER CLIENT";
        }
        PlayerNameField.interactable = true;
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
                CanStartCount = true;
            }
            else
            {
                CanStartCount = false;
            }

        }

        if (CanStartCount)
        {
           
            if(CountDownTimer >= 0)
            {
                CountDownTimer -= Time.deltaTime;
                CountDown.text = "Starting in " + (int)CountDownTimer;
                if (CountDownTimer <= 0)
                {
                    PhotonNetwork.NickName = PlayerName.text;
                    if (PhotonNetwork.IsMasterClient)
                    {
                        LoadMap();
                    }

                }
            }

        }
        else
        {
            CountDownTimer = 10;
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
