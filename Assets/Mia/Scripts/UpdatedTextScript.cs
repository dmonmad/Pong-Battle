using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdatedTextScript : MonoBehaviour
{

    public Text TextName;
    bool TimerActive = false;
    float timer = 0f;
    public float wait;

    private void Update()
    {
        if (timer < wait && TimerActive)
        {
            timer += Time.deltaTime;
        }

        if (timer > wait)
        {
            TextName.text = "";
            TimerActive = false;
        }
    }

    // Update is called once per frame
    public void UpdatedText()
    {
        TimerActive = true;
        TextName.text = "Updated!";
    }

}
