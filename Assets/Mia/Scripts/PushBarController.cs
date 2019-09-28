using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBarController : MonoBehaviour
{

    RectTransform rt;

    public void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
    }

    public void Activate()
    {
        reduceBar();
        StartCoroutine(IncreaseBar(rt, new Vector3(1f, rt.localScale.y, rt.localScale.z), 2.9f));
        //shouldIncrease = true;
    }


    private void Update()
    {
        //if (shouldIncrease)
        //{
        //    Vector3 oldScale = gameObject.GetComponent<RectTransform>().localScale;
        //    Debug.Log("OLDSCALE "+ oldScale.ToString());
        //    Vector3 newScale = Vector3.Lerp(oldScale, new Vector3(1f,oldScale.y,oldScale.z) , 3f);
        //    Debug.Log("NEWSCALE " + newScale.ToString());
            
        //    if (oldScale.x < 1f)
        //    {
        //        Debug.Log("Aumentando");
        //        gameObject.GetComponent<RectTransform>().localScale = newScale;
        //    }
        //    else
        //    {
        //        Debug.Log("Saliendo");
        //        shouldIncrease = false;
        //    }

        //}
    }

    public void reduceBar()
    {
        rt.localScale = new Vector3(0.1f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);

    }

    public IEnumerator IncreaseBar(RectTransform recttransform, Vector3 newScale, float timeToMove)
    {
        Vector3 currentScale = recttransform.localScale;
        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
           recttransform.localScale = Vector3.Lerp(currentScale, newScale, t);
            yield return null;
        }
    }

    //IEnumerator ProgressiveScaling(float duration)
    //{

    //    for (float ft = gameObject.transform.lossyScale.x; ft < 1; ft++)
    //    {

    //        yield return null;
    //    }

    //}

}
