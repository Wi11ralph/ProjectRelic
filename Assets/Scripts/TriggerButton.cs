using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TriggerButton : MonoBehaviour
{
    [SerializeField] private int id;
    [SerializeField] private float activationForce;
    private float startPos;
    private bool buttonClicked = false;
    private void Update()
    {
        if (transform.position.y < startPos - activationForce) 
             buttonClicked = true  ;
        else buttonClicked = false ;

        if (buttonClicked) GameEvents.current.DoorwayTriggerEnter(id);
        else GameEvents.current.DoorwayTriggerExit(id);
    }
    private void Start()
    {
        startPos = transform.position.y;
    }
}
