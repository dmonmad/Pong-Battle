using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoLobby : MonoBehaviourPunCallbacks
{
    public string DefaultName;

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


    public void Start()
    {
        Connect();
        SetName();
    }

    public void Connect()
    {
        bool connected = false;
        int i = 0;

        while (!connected & i <= 4)
        {
            if (!PhotonNetwork.IsConnected)
            {

                log.text += "\nTrying connecting to a server";
                if (PhotonNetwork.ConnectUsingSettings())
                {
                    log.text += "\nConnected to server " + PhotonNetwork.ServerAddress;
                    log.text += "\nAllowing Join button and Name input";
                    connected = true;
                }
                else
                {
                    log.text += "\nThere was a problem connecting to server!";
                }
            }
            else
            {
                connected = true;
            }

            if (i == 4){
                log.text += "\nIt looks like there're no servers up right now!";
            }
        }

        

    }

    public override void OnConnectedToMaster()
    {
        PlayerNameField.interactable = true;
        JoinRandomButton.interactable = true;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void JoinRandom()
    {

        log.text += "\nSearching for a room";

        if (!PhotonNetwork.JoinRandomRoom())
        {
            log.text += "\nFail joining. There're no available rooms to enter";
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
            log.text += "\n##### MC";

        }
       
        JoinRandomButton.interactable = false;


        int color = PhotonNetwork.CurrentRoom.PlayerCount - 1;
        ExitGames.Client.Photon.Hashtable colores = new ExitGames.Client.Photon.Hashtable();
        colores.Add("playercolor", color);
        PhotonNetwork.LocalPlayer.SetCustomProperties(colores);

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

                        if (!string.IsNullOrEmpty(PlayerNameField.text))
                        {
                            Debug.Log("Setting the name " + PlayerNameField.text);
                            PhotonNetwork.NickName = PlayerNameField.text;
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

    private void SetName()
    {
        PlayerName.text = PhotonNetwork.NickName;
    }
}
