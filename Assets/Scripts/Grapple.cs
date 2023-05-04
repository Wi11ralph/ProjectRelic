using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Grapple : MonoBehaviour
{
    [SerializeField] private GameObject player;
    // Start is called before the first frame update
    private Vector3 currentGrapplePosition;
    private Vector3 grapplePoint;
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

        float distanceFromPoint = Vector3.Distance(player.transform.position, gP);

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

            lr.SetPosition(0, player.transform.position);
            lr.SetPosition(1, currentGrapplePosition);
        }
    }
    public void StopGrapple()
    {
        player.GetComponent<Player>().IsGrappling = false;
        lr.positionCount = 0;
        Destroy(joint);
    }
}
