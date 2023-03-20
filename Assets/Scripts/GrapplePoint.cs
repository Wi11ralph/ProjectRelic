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
    private RaycastHit hits;
    private Color col;
    private Vector3[] rayy;
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
        rayy = RayOffsets(pos);
        
        for(int i =0; i < rayy.Length; i++)
        {
            Vector3 dirs = (grapplePoint.position - rayy[i]).normalized;
            Ray rayz = new Ray(rayy[i], dirs);
            Physics.Raycast(rayz, out hits);
            if (Vector3.Distance(grapplePoint.position, hits.point) < 0.3f) col = Color.green;
            else col = Color.yellow;
            if (Vector3.Distance(grapplePoint.position, rayy[i]) > 6.5f) col = Color.red;
            Debug.DrawRay(rayy[i], dirs * Mathf.Min(hits.distance, 6.5f), col, Time.deltaTime);
        } 

        if (Vector3.Distance(grapplePoint.position,hit.point) < 0.3f) col = Color.green;
        else col = Color.yellow;
        if (Vector3.Distance(grapplePoint.position, pos) > 6.5f) col = Color.red;
        
        Debug.DrawRay(pos, dir * Mathf.Min(hit.distance, 6.5f), col, Time.deltaTime);
        pointToGrapple = cam.WorldToScreenPoint(grapplePoint.position);
        //Debug.Log(pointToGrapple);
        if (distance < 100f)
        {
            //Debug.Log("Within distance of cursor: " + distance);
        }
        else
        {//Debug.Log("Out of distance of cursor: " + distance);
        }
    }
    private Vector3[] RayOffsets(Vector3 inn)
    {
        Vector3[] rays = new Vector3[6];
        rays[0] = new Vector3(inn.x - 0.5f, inn.y, inn.z);
        rays[1] = new Vector3(inn.x , inn.y - 0.5f, inn.z);
        rays[2] = new Vector3(inn.x, inn.y, inn.z - 0.5f);
        rays[3] = new Vector3(inn.x + 0.5f, inn.y, inn.z);
        rays[4] = new Vector3(inn.x, inn.y + 0.5f, inn.z);
        rays[5] = new Vector3(inn.x, inn.y, inn.z + 0.5f);
        return rays;
    } 
}