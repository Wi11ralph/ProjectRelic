using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerButton : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private float activationForce;
    [SerializeField] private float timeToDrop = 0.8f;

    private float time = 0.0f;
    private float startPos;
    private bool buttonClicked = false;
    private bool triggerWait   = false;
    private void Update()
    { 
        if (!buttonClicked && transform.position.y < startPos - activationForce)
        {
            Debug.Log("clicked");
            buttonClicked = true ;
            triggerWait   = false;
            GameEvents.current.DoorwayTriggerEnter(id);
        }
        else if(buttonClicked && transform.position.y > startPos - activationForce)
        {
            Debug.Log("unclicked");
            buttonClicked = false;
            triggerWait   = true ;
        }

        if (triggerWait) time += Time.deltaTime;
        else time = 0.0f;
        if (time >= timeToDrop)
        {
            GameEvents.current.DoorwayTriggerExit(id);
            time = 0.0f;
        }
    }
    private void Start()
    {
        startPos = transform.position.y;
    }
}
