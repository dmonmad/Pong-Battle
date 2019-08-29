using System.Collections;
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

    public float timerPush = 7f;
    public float cdPush = 7f;

    Rigidbody rb;

    public bool isPlayer = false;

    private void Awake()
    {

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
        

    }

    // Update is called once per frame 
    void Update()
    {
        if (timerPush < cdPush)
        {
            timerPush += Time.deltaTime;
        }
        //if (!isLocalPlayer)
        //{
        //    return;
        //}
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
        Destroy(this.gameObject);
        photonView.RPC("DestroyPlayer", RpcTarget.OthersBuffered);
    }

    void Push()
    {

        int[] idArray = new int[4];

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

        Debug.Log("Objetos en Array antes de enviar " + idArray.Length);

        if (idArray != null)
        {
            
            photonView.RPC("pushBack", RpcTarget.All, idArray, gameObject.transform.position);
        }
                
    }
    
    [PunRPC]
    void DestroyPlayer(PhotonMessageInfo info)
    {
        Destroy(info.photonView.gameObject);

    }

    [PunRPC]
    void pushBack(int[] idArray, Vector3 position)
    {

        Debug.Log("RPC RECIBIDO POR "+photonView.ViewID+" QUE CORRESPONDE A "+gameObject.name+" Y EL ARRAY TIENE "+idArray.Length+" objetos");

        bool shouldIPush = false;

        for (int i = 0; i < idArray.Length; i++)
        {
            Debug.Log("Mi ID es " + photonView.ViewID + " y el ID a comparar es " + idArray[i]);
            if (idArray[i] == photonView.ViewID)
            {
                shouldIPush = true;
            }
        }
        
        if (shouldIPush)
        {
            Debug.LogError("AÑADIENDO FUERZA A RB");
            gameObject.transform.position = new Vector3(0, 0, -0.5f);

        }

        Debug.Log("WTF");
    }


    [PunRPC]
    void setName(PhotonMessageInfo info, string name)
    {
        PhotonView.Find(info.photonView.ViewID).gameObject.GetComponent<PlayerMovement>().PlayerNameString = name; ;
        //info.photonView.gameObject.GetComponent<PlayerMovement>().PlayerNameString = name;
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    }



}
