using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] connectors;
    [SerializeField] private float height;

    [SerializeField] private Material outline;

    private RaycastHit hit;
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if(hit.collider.gameObject == this.gameObject)
        {
            outline.SetFloat("sc", 1.05f);
            if(Input.GetKeyDown(KeyCode.E))
            {
                for(int i=0; connectors.Length > i; i++)
                { 
                    connectors[i].position = new Vector3(
                        1f,
                        1f,
                        1f
                      );
                }
            }
        } else outline.SetFloat("sc", 0f);
    }
}
