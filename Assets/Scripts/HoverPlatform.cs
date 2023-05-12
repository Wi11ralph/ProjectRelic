using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverPlatform : MonoBehaviour
{
    [SerializeField] private Transform[] connectors;
    [SerializeField] private float height;
    private float startHeight;
    private bool floating;
    private float floatT = 0f;

    [SerializeField] private Material outline;

    private RaycastHit hit;
    private void Awake()
    {
        startHeight = connectors[0].position.y;
    }
    private void Update()
    { 
        if (Player.airRelic && IsHovering()) {
            floatT = 0f;
            outline.SetFloat("sc", 1.05f);
            if (Input.GetKeyDown(KeyCode.E)) floating = !floating;
        } else {
            outline.SetFloat("sc", 0f); 
            if (Input.GetKeyDown(KeyCode.E)) floating = false;
        }

        if (floating) SetHeight(Mathf.Lerp(connectors[0].position.y, height, Time.deltaTime * 1.5f));
        else SetHeight(Mathf.Lerp(connectors[0].position.y, startHeight, Time.deltaTime * 1.5f));

        if (floating)  {
            floatT += Time.deltaTime;
            if (floatT > 5f) floating = false;
        }
    }
    private void SetHeight(float heightPos)
    {
        for (int i = 0; connectors.Length > i; i++)
        {
            connectors[i].position = new Vector3(
                connectors[i].position.x,
                heightPos,
                connectors[i].position.z
              );
        }
    }
    private bool IsHovering()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        try {
            if (hit.collider.gameObject == this.gameObject) return true;
            else return false;
        }
        catch (System.NullReferenceException) {
            return false;
        }
    }
}
