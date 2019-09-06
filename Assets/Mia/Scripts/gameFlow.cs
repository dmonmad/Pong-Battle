using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class gameFlow : MonoBehaviourPun
{

    public bool SpawnBalls;

    public float chooseCorner;
    public float spawnBallTimer = 5;
    public float spawnTimer = 0;
    public GameObject playerPrefab;
    public GameObject cameraPrefab;
    public GameObject bulletReference;


    // Start is called before the first frame update
    void Start()
    {

        spawnPlayer();
        SpawnBalls = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        if (SpawnBalls)
        {

            if (GameObject.FindGameObjectWithTag("player"))
            {
                spawnTimer += Time.deltaTime;
                if (spawnTimer > spawnBallTimer)
                {
                    PhotonNetwork.Instantiate(bulletReference.name, chooseCornerFunc(), new Quaternion(0, 0, 0, 0));
                    spawnTimer = 0;
                    chooseCornerFunc();
                }
            }
        }
    }

    Vector3 chooseCornerFunc()
    {
        chooseCorner = Random.Range(0, 3);
        float startPosX = 0;
        float startPosY = 0;
        if (chooseCorner == 0)
        {
            startPosX = -23.5f;
            startPosY = -23.5f;
        }

        if (chooseCorner == 1)
        {
            startPosX = -23.5f;
            startPosY = 23.5f;
        }

        if (chooseCorner == 2)
        {
            startPosX = 23.5f;
            startPosY = -23.5f;
        }

        if (chooseCorner == 3)
        {
            startPosX = 23.5f;
            startPosY = 23.5f;
        }
        return new Vector3(startPosX, startPosY, 1.2f);
    }


    void spawnPlayer()
    {
        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 1.2f), new Quaternion(0, 0, 0, 0));
        GameObject mycamera = Instantiate(cameraPrefab, new Vector3(0, 0, 1.2f), new Quaternion(0, 0, 0, 0));
        //mycamera.GetComponent<cameraFollow>().target = player.transform.GetChild(0);
    }

}
