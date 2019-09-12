using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

    public GameObject CollisionParticles;

    public float speedX;
    public float speedY;
    public float startSpeed = 5f;

    public float timeLeft = 5.0f;

    public float velocityx;
    public float velocityy;


    // Start is called before the first frame update
    void Start()
    {
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
        speedX = Random.Range(-5, 3);
        speedY = Random.Range(-3, 5);

        if (speedX == 0)
        {
            speedX = 2;
        }

        if (speedY <= 0)
        {
            speedX = -2;
        }

        if (speedX == speedY)
        {
            speedX = speedX * -1;
        }

        GetComponent<Rigidbody>().velocity = new Vector3(speedX * 2, speedY * 2, 0);

    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(CollisionParticles, collision.contacts[0].point, Quaternion.identity);
    }

}
