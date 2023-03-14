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
    void Update()
    {
        distance = Vector2.Distance(
            new(Input.mousePosition.x, Input.mousePosition.y),
            new(pointToGrapple.x, pointToGrapple.y)
        );

        pointToGrapple = cam.WorldToScreenPoint(grapplePoint.position);
        Debug.Log(pointToGrapple);
        if(distance < 100f) {
            Debug.Log("Within distance: " + distance);
        } else Debug.Log("Out of distance: " + distance);
    }
}