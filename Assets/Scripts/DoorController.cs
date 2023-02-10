using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class DoorController : MonoBehaviour
{
    
    [SerializeField] private int id;
    [SerializeField] private Vector3 startPos;
    [SerializeField] private Vector3 openPos;
    [SerializeField] private float openTime;
    [SerializeField] private float closeTime;

    //private bool closeWait;
    //private bool openWait;
    private void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit += OnDoorwayClose;

        startPos = transform.position;
    }
    private void OnDoorwayOpen(int id)
    {
        if (id != this.id) return;
        if (LeanTween.isTweening()) LeanTween.cancel(gameObject); 
        LeanTween.moveLocal(gameObject, openPos, openTime).setEaseOutQuad();
        
    }
    private void OnDoorwayClose(int id) {

        if (id != this.id) return;
        if (LeanTween.isTweening()) LeanTween.cancel(gameObject);
        LeanTween.moveLocal(gameObject, startPos, closeTime).setEaseOutQuad();
        
    }

   
}
