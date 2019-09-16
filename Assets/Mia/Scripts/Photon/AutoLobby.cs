using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoLobby : MonoBehaviourPunCallbacks
{
    public string DefaultName = "DEFAULTPLAYER";

    public Button ConnectButton;
    public Button JoinRandomButton;
    public TextMeshProUGUI log;
    public TextMeshProUGUI PlayersCount;
    public TMP_InputField PlayerNameField;
    public TextMeshProUGUI PlayerName;
    public Image blurPopup;
    public int playersCount = 0;

    public TextMeshProUGUI CountDown;
    public float CountDownTimer;
    public float CountDownLeft;
    public bool CanStartCount = false;
    public bool CanLoadLevel = false;


    public byte maxPlayersPerRoom;
    public byte minPlayersPerRoom;

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
        JoinRandomButton.interactable = false;


        int color = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        ExitGames.Client.Photon.Hashtable colores = new ExitGames.Client.Photon.Hashtable();
        colores.Add("playercolor", color);
        PhotonNetwork.LocalPlayer.SetCustomProperties(colores);
        log.text += color.ToString();

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (CanStartCount)
        {
            CountDownLeft = CountDownTimer;
        }
    }


    public override void OnLeftRoom()
    {
        PhotonNetwork.SetPlayerCustomProperties(null);
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
            
                if (CountDownLeft >= 0)
                {
                    blurPopup.enabled = true;
                    CountDownLeft -= Time.deltaTime;
                    CountDown.text = "Starting in " + (int)CountDownLeft;
                    if (CountDownLeft <= 0)
                    {

                        if (PlayerName.text != "" && PlayerName.text != null && PlayerName.text != " ")
                        {
                            Debug.Log("Setting the name " + PlayerName.text);
                            PhotonNetwork.NickName = PlayerName.text;
                        }
                        else
                        {
                            PhotonNetwork.NickName = DefaultName;
                        }

                    if (PhotonNetwork.IsMasterClient)
                        {
                            LoadMap();

                        }

                    }
                }
        }
        else
        {
            blurPopup.enabled = false;
            CountDownLeft = CountDownTimer;
            CountDown.text = "";
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
