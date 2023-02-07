using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DoorController : MonoBehaviour
{
    [SerializeField] private int id;
    private void Start()
    {
        GameEvents.current.onDoorwayTriggerEnter += OnDoorwayOpen;
        GameEvents.current.onDoorwayTriggerExit += OnDoorwayClose;
    }
    private void OnDoorwayOpen(int id)
    {
        if (id != this.id) return;
        LeanTween.moveLocalY(gameObject, 6f, 0.5f).setEaseOutQuad();
    }
    private void OnDoorwayClose(int id) {

        if (id != this.id) return;
        LeanTween.moveLocalY(gameObject, 2f, 0.5f).setEaseOutQuad();
    }

   
}
