using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dissolver : MonoBehaviour
{
    private Material mat;
    public bool dissolve = false; 
    private void Awake()
    {
        mat = GetComponent<MeshRenderer>().material;
    }
    private void Update()
    {
        if (!dissolve) return;

        mat.SetFloat("amount", mat.GetFloat("amount") + Time.deltaTime);
        if (mat.GetFloat("amount") > 1) Destroy(this.gameObject);
    } 
}

