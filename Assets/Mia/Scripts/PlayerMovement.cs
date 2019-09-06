﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Photon.Pun;

public class PlayerMovement : MonoBehaviourPun
{

    public float normalSpeedBackup;
    public float normalSpeed;
    public float runSpeed;
    public float topBotSize = 23.5f;
    public float leftRightSize = 23.5f;
    public float pushRadius;
    public TextMesh playerName;
    public string PlayerNameString;

    public GameObject explodeParticle;

    public float timerPush = 7f;
    public float cdPush = 7f;

    PhotonView photonView;
    Rigidbody rb;

    public bool isPlayer = false;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();


        if (photonView.IsMine)
        {
            isPlayer = true;
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Start()
    {
        if (isPlayer)
        {
            PlayerNameString = PhotonNetwork.NickName + " : " + photonView.ViewID;
            playerName.text = PlayerNameString;
            //photonView.RPC("setName", RpcTarget.OthersBuffered, playerName.text);
        }
        else
        {
            playerName.text = photonView.Owner.NickName + " : " + photonView.ViewID;
        }

        //explodeParticle = Resources.Load("Explode") as GameObject;

    }

    // Update is called once per frame 
    void Update()
    {
        if (timerPush < cdPush)
        {
            timerPush += Time.deltaTime;
        }

        if (isPlayer)
        {

            Vector3 movement = new Vector3(0, 0, 0);

            if ((Input.GetKey(KeyCode.W)) && (transform.position.y >= topBotSize == false))
            {
                movement = movement + new Vector3(0, 1, 0);
            }


            if ((Input.GetKey(KeyCode.S)) && (transform.position.y <= -topBotSize == false))
            {
                movement = movement + new Vector3(0, -1, 0);
            }


            if ((Input.GetKey(KeyCode.D)) && (transform.position.x >= leftRightSize == false))
            {

                movement = movement + new Vector3(1, 0, 0);

            }

            if ((Input.GetKey(KeyCode.A)) && (transform.position.x <= -leftRightSize == false))
            {

                movement = movement + new Vector3(-1, 0, 0);

            }

            rb.MovePosition(gameObject.transform.position + (movement * Time.deltaTime * normalSpeed));

            if (Input.GetKeyDown(KeyCode.F) && timerPush >= cdPush)
            {
                timerPush = 0;
                Debug.Log("PULSANDO F");
                Push();
            }

            if (Input.GetKeyDown(KeyCode.M))
            {
                Die();
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Debug.Log("Players: " + PhotonNetwork.PlayerList);
            }

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isPlayer)
        {
            Debug.Log("COLLIDING");

            switch (collision.gameObject.tag)
            {
                case "ball":

                    Die();
                    break;
            }
        }
    }



    void Die()
    {
        Instantiate(explodeParticle, gameObject.transform.position, Quaternion.identity);
        this.photonView.RPC("DestroyPlayer", RpcTarget.OthersBuffered);
        Destroy(this.gameObject);

    }

    void Push()
    {

        int[] idArray = new int[3];

        Debug.Log("BUSCANDO OBJETOS");

        Collider[] inArea = Physics.OverlapSphere(transform.position, pushRadius);

        Debug.Log("Objetos en INAREA "+inArea.Length);

        int contador = 0;

        for (int i = 0; i < inArea.Length; i++)
        {
           Debug.Log(i+". "+inArea[i].gameObject.name);


            if (inArea[i].gameObject.tag == "player" && !inArea[i].gameObject.Equals(gameObject))
            {
                Debug.Log("Añadimos este");
                idArray[contador] = inArea[i].gameObject.GetComponent<PhotonView>().ViewID;
                contador++;
            }
            else
            {
                Debug.Log("Este no");
            }

        }

        if (idArray[0] != 0)
        {


            this.photonView.RPC("PushBack", RpcTarget.Others, idArray);
        }

    }


    [PunRPC]
    void DestroyPlayer(PhotonMessageInfo info)
    {
        
            Instantiate(explodeParticle, info.photonView.gameObject.transform.position, Quaternion.identity);
            Destroy(info.photonView.gameObject);
        
    }

    [PunRPC]
    void PushBack(int[] idArray)
    {
        //Debug.LogWarning("RPC RECIBIDO POR " + this.photonView.ViewID + " QUE CORRESPONDE A " + this.photonView.Owner.NickName + " que ha sido enviado por " + info.photonView.Owner.NickName + " con ID "+ info.photonView.ViewID);
        //if (this.photonView.IsMine && info.photonView.ViewID == this.photonView.ViewID)
        //{

        for(int i=0; i < idArray.Length; i++)
        {
        GameObject go = PhotonView.Find(idArray[i]).gameObject;
        go.transform.position = new Vector3(0, 0, -1.2f);
        }

        //}
            //bool shouldIPush = false;

            //for (int i = 0; i < idArray.Length; i++)
            //{
            //    Debug.Log("Mi ID es " + photonView.ViewID + " y el ID a comparar es " + idArray[i]);
            //    if (idArray[i] == photonView.ViewID)
            //    {
            //        shouldIPush = true;
            //    }
            //    else
            //    {
            //        shouldIPush = false;
            //    }
            //}

            //if (shouldIPush)
            //{
            //    Debug.LogError("AÑADIENDO FUERZA A RB");
            //    gameObject.transform.position = new Vector3(0, 0, -0.5f);

            //}

            //Debug.Log("WTF");
        
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    }



}
