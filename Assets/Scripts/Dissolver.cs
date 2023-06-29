using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    [SerializeField] private GameObject itemtospawn;
    [SerializeField] private AudioClip dissolveSound;
    [SerializeField] private float animationTime =1f;

    private bool spawnitem = true;
    private Material mat;
    public bool dissolve = false;
    private bool trigger = true;
    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        if (!dissolve) return;
        Debug.Log("hi");
        if(trigger)
        {
            AudioSource aSor = GetComponent<AudioSource>();
            trigger = false;
            try
            {
                aSor.loop = false;
                aSor.volume = 1f;
                aSor.PlayOneShot(dissolveSound);
            } catch(UnityEngine.MissingComponentException) { }
            
        }
        
        float amount = mat.GetFloat("amount");
        Debug.Log(amount);
        mat.SetFloat("amount", amount + Time.deltaTime * animationTime);

        if (amount <= 0.5 && spawnitem) spawn();
        if (amount > 1) Destroy(this.gameObject);
    }

    private void spawn()
    {
        spawnitem = false;
        try
        {
            itemtospawn = Instantiate(itemtospawn);

            itemtospawn.transform.position = new Vector3(
                this.transform.position.x,
                this.transform.position.y + 0.2f,
                this.transform.position.z
            );
        }
        catch (UnityEngine.UnassignedReferenceException) { }
    }
}

