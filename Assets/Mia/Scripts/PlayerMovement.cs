using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public GameObject pushParticle;

    public float pushForce;
    public float timerPush;
    public float cdPush;

    PhotonView photonView;
    Rigidbody rb;

    public Material[] colors;

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

        SetColor();


        if (isPlayer)
        {
            PlayerNameString = PhotonNetwork.NickName + " : " + photonView.ViewID;
            playerName.text = PlayerNameString;
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

            Instantiate(pushParticle, gameObject.transform.position, Quaternion.identity);
            this.photonView.RPC("PushBack", RpcTarget.Others, idArray);
        }

    }

    void SetColor()
    {
        if (photonView.IsMine)
        {
            ExitGames.Client.Photon.Hashtable e = PhotonNetwork.LocalPlayer.CustomProperties;
            int color = int.Parse(e["playercolor"].ToString());
            gameObject.GetComponent<MeshRenderer>().material = colors[color];
            photonView.RPC("SendColor", RpcTarget.OthersBuffered, color);
        }
        
        
    }


    [PunRPC]
    void DestroyPlayer(PhotonMessageInfo info)
    {

        Instantiate(explodeParticle, info.photonView.gameObject.transform.position, Quaternion.identity);
        Destroy(info.photonView.gameObject);
  
    }

    [PunRPC]
    void PushBack(int[] idArray, PhotonMessageInfo e)
    {

        for(int i=0; i < idArray.Length; i++)
        {
            if(idArray[i] != 0)
            {
                Instantiate(pushParticle, gameObject.transform.position, Quaternion.identity);
                GameObject go = PhotonView.Find(idArray[i]).gameObject;
                //go.transform.position = new Vector3(0, 0, -1.2f);
                Vector3 knockback = go.gameObject.transform.position - gameObject.transform.position;
                go.GetComponent<Rigidbody>().AddForce(knockback.normalized * pushForce, ForceMode.Impulse);
                //Debug.Log("EL NOMBRE DE GO ES " + go.name);
            }
        }
    }

    [PunRPC]
    void SendColor(int color, PhotonMessageInfo e)
    {
        e.photonView.gameObject.GetComponent<MeshRenderer>().material = colors[color];
    }

    void OnDrawGizmos()
    {
        // Display the explosion radius when selected
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, pushRadius);
    }



}
