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

    private float maxDistance = 100f;
    private SpringJoint joint;
    private LineRenderer lr;

    private Material[] mat;
   
    private bool IsGrappleable()
    {
        Vector3 pos = player.transform.position;
        Vector3 dir = (grapplePoint.position - player.transform.position).normalized;
        Ray ray = new Ray(pos, dir);
        Physics.Raycast(ray, out hit);
        rayy = RayOffsets(pos);
        bool grappleable = true;
        for (int i = 0; i < rayy.Length; i++)
        {
            
            Vector3 dirs = (grapplePoint.position - rayy[i]).normalized;
            Ray rayz = new Ray(rayy[i], dirs);
            Physics.Raycast(rayz, out hits);
            if (Vector3.Distance(grapplePoint.position, hits.point) < 0.3f) col = Color.green;
            else
            {
                col = Color.yellow;
                grappleable = false;
            }
            if (Vector3.Distance(grapplePoint.position, rayy[i]) > 6.5f)
            {
                col = Color.red;
                grappleable = false;
            }
            Debug.DrawRay(rayy[i], dirs * Mathf.Min(hits.distance, 6.5f), col, Time.deltaTime);
        }

        if (Vector3.Distance(grapplePoint.position, hit.point) < 0.3f) col = Color.green;
        else
        {
            col = Color.yellow;
            grappleable = false;
        }
        if (Vector3.Distance(grapplePoint.position, pos) > 6.5f)
        {
            col = Color.red;
            grappleable = false;
        } 
        Debug.DrawRay(pos, dir * Mathf.Min(hit.distance, 6.5f), col, Time.deltaTime);
        if (joint && !grappleable) StopGrapple();
        return grappleable;

        
    }
    void Update()
    {
        distance = Vector2.Distance(
            new(Input.mousePosition.x, Input.mousePosition.y),
            new(pointToGrapple.x, pointToGrapple.y)
        );

        pointToGrapple = cam.WorldToScreenPoint(grapplePoint.position);

        //Debug.Log(pointToGrapple);
        if (distance < 100f && IsGrappleable())
        {
            if (mat[1].GetFloat("_OutlineThickness") != 0.6f) mat[1].SetFloat("_OutlineThickness", 0.6f); 
            if (Input.GetMouseButtonDown(1) ) StartGrapple(); 
        } else if (mat[1].GetFloat("_OutlineThickness") != 0 && !joint) mat[1].SetFloat("_OutlineThickness", 0);
        if (Input.GetMouseButtonUp(1)) StopGrapple();
        if (joint) IsGrappleable();

    }
    private Vector3 currentGrapplePosition;
    private void StartGrapple()
    {
        player.GetComponent<Player>().IsGrappling = true;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = grapplePoint.position;

        float distanceFromPoint = Vector3.Distance(player.transform.position, grapplePoint.position);

        //The distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.5f;
        joint.minDistance = distanceFromPoint * 0.1f;

        //Adjust these values to fit your game.
        joint.spring = 15.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = player.transform.position;
    }
    private void StopGrapple()
    {
        player.GetComponent<Player>().IsGrappling = false;
        lr.positionCount = 0;
        Destroy(joint);
    }

    void Awake() {
        lr = player.GetComponent<LineRenderer>();
        //_OutlineThickness
        mat = GetComponent<MeshRenderer>().sharedMaterials;

    }
    private void LateUpdate() { DrawRope(); }
    void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!joint)
        {
            lr.positionCount = 0;
            return;
        }
        else
        {
            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint.position, Time.deltaTime * 20f);

            lr.SetPosition(0, player.transform.position);
            lr.SetPosition(1, currentGrapplePosition);
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