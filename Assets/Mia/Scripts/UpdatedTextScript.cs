using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatedTextScript : MonoBehaviour
{

    public Text TextName;
    public float wait;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdatedText()
    {
        StartCoroutine(Waiting(wait));

    }

    IEnumerator Waiting(float seconds)
    {
        TextName.text = "Updated!";
        yield return new WaitForSeconds(seconds);
        TextName.text = "";

    }


}
