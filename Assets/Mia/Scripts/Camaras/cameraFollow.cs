using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public bool attached;

    public float smoothSpeed = 0.5f;

    
    void Start()
    {
        

    }

    
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("camPos"))
        {
            target = GameObject.FindGameObjectWithTag("camPos").GetComponent<Transform>();
        }
        
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = target.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}
