using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    [SerializeField] private int id;

    private float time = 0.0f;
    [SerializeField] private float timeToDrop = 0.8f;
    private bool collideWait = false;
    private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.DoorwayTriggerEnter(id);
        collideWait = false;
    }
    private void OnTriggerExit(Collider other){collideWait = true;}
    private void OnTriggerStay(Collider other){collideWait = false;}

    private void Update()
    {
        if(collideWait) time += Time.deltaTime;
        else time = 0.0f;
        if (time >= timeToDrop)
        {
            GameEvents.current.DoorwayTriggerExit(id);
            time = 0.0f;
        }
    }
}
