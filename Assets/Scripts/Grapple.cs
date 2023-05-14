using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Grapple : MonoBehaviour
{
    [SerializeField] private GameObject player;
    public GameObject palm;
    // Start is called before the first frame update
    private Vector3 currentGrapplePosition; 
    [HideInInspector] public Vector3 grapplePoint;
    private SpringJoint joint;
    private LineRenderer lr;

    private void Awake()
    {
        lr = player.GetComponent<LineRenderer>();
    }
    public void StartGrapple(Vector3 gP)
    {
        player.GetComponent<Player>().IsGrappling = true;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = gP;
        
        grapplePoint = gP;

        float distanceFromPoint = Vector3.Distance(palm.transform.position, gP);

        //The distance grapple will try to keep from grapple point. 
        joint.maxDistance = distanceFromPoint * 0.5f;
        joint.minDistance = distanceFromPoint * 0.1f;

        //Adjust these values to fit your game.
        joint.spring = 30.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = palm.transform.position;
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
            currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, grapplePoint, Time.deltaTime * 20f);

            lr.SetPosition(0, palm.transform.position);
            lr.SetPosition(1, currentGrapplePosition);
        }
    }
    private void Update()
    {
        if (!joint) return;
        joint.anchor = palm.transform.position - player.transform.position;
    }
    public void StopGrapple()
    {
        player.GetComponent<Player>().IsGrappling = false;
        lr.positionCount = 0;
        Destroy(joint);
    }
}
