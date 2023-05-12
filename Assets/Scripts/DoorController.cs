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

    [SerializeField] private bool keyMode = false; 
    [SerializeField] private Material outline;
    [SerializeField] private Player player;

    private RaycastHit hit;

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
    private void Update()
    {
        if (!keyMode) return;
        if(IsHovering() && player.keys.Contains(id))
        {
            outline.SetFloat("sc", 1.05f);
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameEvents.current.DoorwayTriggerEnter(id);
                player.keys.Remove(id);
            }

        } else outline.SetFloat("sc", 0f);

    }

    private bool IsHovering()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        try
        {
            if (hit.collider.gameObject == this.gameObject) return true;
            else return false;
        }
        catch (System.NullReferenceException)
        {
            return false;
        }
    }


}
