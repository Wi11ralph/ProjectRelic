using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Pause : MonoBehaviour
{
    private bool input;
    private bool pause;
    private bool wait;

    void Update()
    {
        GatherInput();
        if(input) { 
            if (!wait) pause = !pause;
            wait = true;
        } else wait = false;

        if(pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

    private void GatherInput()
    {
        input = Input.GetKeyDown(KeyCode.Escape);
    }
}
