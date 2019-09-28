using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

    public GameObject CollisionParticles;

    public float speedX;
    public float speedY;

    public float velocityx;
    public float velocityy;
    public float SpeedMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        SpeedMultiplier = 75;
        startMovement();
        
    }

    // Update is called once per frame
    void Update()
    {

        velocityx = GetComponent<Rigidbody>().velocity.x;
        velocityy = GetComponent<Rigidbody>().velocity.y;

    }

    void startMovement()
    {
        int option = Random.Range(1, 2);

        if(option == 1)
        {
            speedX = Random.Range(-10, -5);
        }
        else
        {
            speedX = Random.Range(5, 10);
        }

        int option2 = Random.Range(1, 2);

        if (option2 == 1)
        {
            speedY = Random.Range(-10, -5);
        }
        else
        {
            speedY = Random.Range(5, 10);
        }



        Debug.Log("Final vector " + new Vector3(speedX * SpeedMultiplier, speedY * SpeedMultiplier, 0));


        //if (speedX == 0)
        //{
        //    speedX = 2;
        //}

        //if (speedY <= 0)
        //{
        //    speedX = -2;
        //}

        if (speedX == speedY)
        {
            speedX = speedX * -1;
        }

        GetComponent<Rigidbody>().AddForce(new Vector3(speedX * SpeedMultiplier, speedY * SpeedMultiplier, 0), ForceMode.Force);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(CollisionParticles, collision.contacts[0].point, Quaternion.identity);
    }

}
