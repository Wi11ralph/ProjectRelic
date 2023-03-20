using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrapplePoint : MonoBehaviour
{
    [SerializeField] private Transform grapplePoint;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;

    private Vector3 pointToGrapple;
    private float distance;
    private RaycastHit hit;
    void Update()
    {
        distance = Vector2.Distance(
            new(Input.mousePosition.x, Input.mousePosition.y),
            new(pointToGrapple.x, pointToGrapple.y)
        );

        
        Vector3 pos = player.transform.position;
        Vector3 dir = (grapplePoint.position - player.transform.position).normalized;
        Ray ray = new Ray(pos, dir);
        Physics.Raycast(ray, out hit);
        if(Vector3.Distance(grapplePoint.position,hit.point) < 0.3f) Debug.DrawRay(pos, dir * Mathf.Min(hit.distance, 6.5f), Color.green, Time.deltaTime);
        else Debug.DrawRay(pos, dir * Mathf.Min(hit.distance, 6.5f), Color.yellow, Time.deltaTime);

        pointToGrapple = cam.WorldToScreenPoint(grapplePoint.position);
        //Debug.Log(pointToGrapple);
        if (distance < 100f)
        {
            //Debug.Log("Within distance: " + distance);
        }
        else {//Debug.Log("Out of distance: " + distance);
        }
    }
}