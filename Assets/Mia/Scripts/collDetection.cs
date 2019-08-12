using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collDetection : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {

        switch (collision.gameObject.tag)
        {
            case "ball":

                Destroy(this.gameObject);
                break;
        }
    }
}
