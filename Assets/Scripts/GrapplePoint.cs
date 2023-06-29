using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GrapplePoint : MonoBehaviour
{
    [SerializeField] private Grapple grapple;
    [SerializeField] private Transform grapplePoint;
    [SerializeField] private GameObject player;
    [SerializeField] private Camera cam;

    private Vector3 pointToGrapple;
    private float distance;
    private RaycastHit hit;
    private RaycastHit hits;
    private Color col;
    private Vector3[] rayy;

    [System.Serializable]
    private class GrappleSounds
    {
        public AudioSource audio;
        public AudioClip grapple;
        public AudioClip ungrapple;

    }
    [SerializeField] private GrappleSounds gs;

    //private float maxDistance = 100f;
    private SpringJoint joint;
    

    private Material mat;
    private Renderer rend;
   
    private bool IsGrappleable()
    {
        Vector3 pos = new Vector3(
            player.transform.position.x,
            grapple.palm.transform.position.y,
            player.transform.position.z
        );

        Vector3 dir = (grapplePoint.position - pos).normalized;
        Ray ray = new Ray(pos, dir);
        Physics.Raycast(ray, out hit);
        rayy = RayOffsets(pos);
        bool grappleable = true;
        for (int i = 0; i < rayy.Length; i++)
        {
            
            Vector3 dirs = (grapplePoint.position - rayy[i]).normalized;
            Ray rayz = new Ray(rayy[i], dirs);
            Physics.Raycast(rayz, out hits);
            float debugShowForS = 1f;
            if (Vector3.Distance(grapplePoint.position, hits.point) < 0.3f) col = Color.green;
            else
            {
                col = Color.yellow;
                grappleable = false;
                debugShowForS = 60;
            }
            if (Vector3.Distance(grapplePoint.position, rayy[i]) > 12f)
            {
                col = Color.red;
                grappleable = false;
                debugShowForS = 30;
            }
            Debug.DrawRay(rayy[i], dirs * Mathf.Min(hits.distance, 12f), col, Time.deltaTime * debugShowForS);
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
        if (joint && !grappleable) grapple.StopGrapple();
        return grappleable;

        
    }
    void Update()
    {
        if (!Player.natureRelic) return;

        pointToGrapple = cam.WorldToScreenPoint(grapplePoint.position);

        distance = Vector2.Distance(
            new(Input.mousePosition.x, Input.mousePosition.y),
            new(pointToGrapple.x, pointToGrapple.y)
        );

        //Debug.Log(pointToGrapple);
        if (distance < 100f && IsGrappleable())
        {
            if (mat.GetFloat("_OutlineThickness") != 0.002f)
            {
                mat.SetFloat("_OutlineThickness", 0.002f);
                //Debug.Log(mat.GetFloat("_OutlineThickness"));
            }
            if (Input.GetMouseButtonDown(1))
            {
                grapple.StartGrapple(grapplePoint.position);
                gs.audio.PlayOneShot(gs.grapple);
            }

        }
        else if (mat.GetFloat("_OutlineThickness") != 0 && !player.GetComponent<Player>().IsGrappling || grapple.grapplePoint != grapplePoint.position)
        {
            mat.SetFloat("_OutlineThickness", 0);
            //Debug.Log(mat.GetFloat("_OutlineThickness"));
        }
        if (Input.GetMouseButtonUp(1))
        {
            if (player.GetComponent<Player>().IsGrappling) gs.audio.PlayOneShot(gs.ungrapple);
            grapple.StopGrapple(); 
        }

        if (
            player.GetComponent<Player>().IsGrappling &&
            grapple.grapplePoint == grapplePoint.position
        )
            if (!IsGrappleable())
            {
                grapple.StopGrapple();
                gs.audio.PlayOneShot(gs.ungrapple);
            }

    }
    
    
    

    void Start() {
        
        //_OutlineThickness
        rend = GetComponent<MeshRenderer>(); 
        mat = rend.materials[1];

    }
   
    private Vector3[] RayOffsets(Vector3 inn)
    {
        Vector3[] rays = new Vector3[6];
        rays[0] = new Vector3(inn.x - 0.265f, inn.y, inn.z);
        rays[1] = new Vector3(inn.x , inn.y - 0.265f, inn.z);
        rays[2] = new Vector3(inn.x, inn.y, inn.z - 0.265f);
        rays[3] = new Vector3(inn.x + 0.265f, inn.y, inn.z);
        rays[4] = new Vector3(inn.x, inn.y + 0.265f, inn.z);
        rays[5] = new Vector3(inn.x, inn.y, inn.z + 0.265f);
        return rays;
    } 
}