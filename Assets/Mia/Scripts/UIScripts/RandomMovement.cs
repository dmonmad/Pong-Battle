using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMovement : MonoBehaviour
{

    public float width;
    public float height;
    public float speed;
    public float posX;
    public float posY;
    float borde = 50f;
    public Vector3 direction;
    RectTransform transform;

    void Awake()
    {
        transform = GetComponent<RectTransform>();
        posX = transform.position.x;
        posY = transform.position.y;
    }

    void Start()
    {
        //transform = GetComponent<RectTransform>();

        width = Screen.currentResolution.width;
        height = Screen.currentResolution.height;




        direction = GetRandomDirection();

    }

    // Update is called once per frame
    void Update()
    {

        posX = transform.position.x;
        posY = transform.position.y;

        if (posX < width/2 && posY < height/2 || posX > -width / 2 && posY > -height/2 || posX > -width / 2 && posY < height / 2 || posX < width / 2 && posY > -height / 2)
        {
            transform.position += direction * speed;
        }


    }

    Vector3 GetRandomDirection()
    {
        Vector3 newDirection;

        newDirection = new Vector3(Random.Range(-5, 5), Random.Range(-5, 5), 0);


        return newDirection;
    }
}
