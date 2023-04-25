using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPlatform : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private float height;

    [SerializeField] private GameObject player;
    private Rigidbody pRb;
    private bool playerOnTop = false;
    private float vel =0f;

    // Update is called once per frame

    private void Awake()
    {
        pRb = player.GetComponent<Rigidbody>();
    }
    private void FixedUpdate() {
        float accleration = vel -pRb.velocity.y / Time.fixedDeltaTime;
         float yVel = rigid.velocity.y + Physics.gravity.y;
        if (playerOnTop) yVel -= pRb.mass * accleration;

         //Hovering
         rigid.AddForce(0, -yVel, 0, ForceMode.Acceleration); 
         //Altitude
         rigid.AddForce(0,  height* 5, 0);

        vel = pRb.velocity.y;
     }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player") playerOnTop = true;
        
    }
    private void OnCollisionExit(Collision collision)
    {
        playerOnTop = false;
    }
}
