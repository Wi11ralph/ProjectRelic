using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    [SerializeField] private GameObject itemtospawn;
    [SerializeField] private float animationTime =1f;

    private bool spawnitem = true;
    private Material mat;
    public bool dissolve = false; 
    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        if (!dissolve) return;

        float amount = mat.GetFloat("amount");

        mat.SetFloat("amount", amount + Time.deltaTime * animationTime);

        if (amount <= 0.5 && spawnitem) spawn();
        if (amount > 1) Destroy(this.gameObject);
    }

    private void spawn()
    {
        spawnitem = false;
        itemtospawn = Instantiate(itemtospawn);
        itemtospawn.transform.position = this.transform.position;
    }
}

