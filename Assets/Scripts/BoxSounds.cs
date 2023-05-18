using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSounds : MonoBehaviour
{
    private AudioSource audioSource;
    private Rigidbody rb;
    [SerializeField] private float soundMulti;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionStay(Collision collision)
    {
        if (GetComponent<Dissolver>() != null && GetComponent<Dissolver>().dissolve) return;
        if (collision.gameObject.tag != "Player")
        {
            audioSource.volume = Mathf.Lerp(audioSource.volume,0f,Time.deltaTime*7.5f);
            return;
        }
        audioSource.volume = Mathf.Min(Negate(rb.velocity.x*soundMulti) + Negate(rb.velocity.z*soundMulti), 1f);
    }
    private float Negate(float value)
    {
        return Mathf.Sqrt(value*value);
    } 
}
