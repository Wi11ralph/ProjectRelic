using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerButton : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private float activationForce;
    [SerializeField] private float timeToDrop = 0.8f;
    
    [SerializeField] private AudioSource buttonSounds;
    [System.Serializable]
    private class ClickSounds
    {
        public AudioClip clicked;
        public AudioClip unclicked;
       
    }
    [SerializeField] private ClickSounds clicks;

    private float time = 0.0f;
    private float startPos;
    private bool buttonClicked = false;
    private bool triggerWait   = false;
    private void Update()
    { 
        if (!buttonClicked && transform.position.y < startPos - activationForce)
        {
            //Debug.Log("clicked");
            buttonSounds.PlayOneShot(clicks.clicked);
            try {
            buttonClicked = true ;
            triggerWait   = false;
            GameEvents.current.DoorwayTriggerEnter(id);
            } catch (System.NullReferenceException) { Debug.LogWarning("Please restart the game"); }
        }
        else if(buttonClicked && transform.position.y > startPos - activationForce)
        {
            //Debug.Log("unclicked");
            buttonSounds.PlayOneShot(clicks.unclicked);
            buttonClicked = false;
            triggerWait   = true ;
        }

        if (triggerWait) time += Time.deltaTime;
        else time = 0.0f;
        if (time >= timeToDrop && triggerWait)
        {
            try {
                GameEvents.current.DoorwayTriggerExit(id);
                time = 0.0f;
                triggerWait = false; 
            } catch (System.NullReferenceException) { }
        }
    }
    private void Start()
    {
        startPos = transform.position.y;
    }
}
