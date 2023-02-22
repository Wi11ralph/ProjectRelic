using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallShooter : MonoBehaviour
{
    [SerializeField] private GameObject fireball;

    [SerializeField] private float shootForce, upwardForce;

    [SerializeField] private float timeBetweenShooting, timeBetweenShots;
    [SerializeField] private int maxBalls;
    [SerializeField] private bool allowHold;

    private int ballsLeft, ballsShot;

    private bool shooting, readyToShoot;

    [SerializeField] private GameObject player;
    [SerializeField] private Transform attackPoint;

    public bool allowInvoke = true;

    private void Awake()
    {
        ballsLeft = maxBalls;
        readyToShoot = true;
    }
    private void Update()
    {
        MyInput();
    }
    private void MyInput()
    {
        if (Pause.active) return;
        if (allowHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if(readyToShoot && shooting && ballsLeft > 0 )
        {
            ballsShot = 0;

            Shoot();
        }
    }
    private void Shoot()
    {

        readyToShoot = false;
        ballsLeft--;
        ballsShot++;

        GameObject currentBall = Instantiate(fireball, attackPoint.position, Quaternion.identity);
        currentBall.transform.forward = player.transform.forward;

        currentBall.GetComponent<Rigidbody>().AddForce(attackPoint.transform.forward * shootForce, ForceMode.Impulse);
        currentBall.GetComponent<Rigidbody>().AddForce(attackPoint.transform.up * upwardForce, ForceMode.Impulse);

        if(allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
            Debug.Log("invoke");
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }
}
