using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float connectorOffset;
    [SerializeField] private float height;
    [HideInInspector] public GameObject[] points;
    private Vector3[] connectors = new Vector3[4];

    private Vector3[] CreateConnectors(float y)
    {
        Vector3[] connections = {
            new(connectorOffset ,y, connectorOffset),
            new(-connectorOffset,y, connectorOffset),
            new(-connectorOffset,y,-connectorOffset),
            new(connectorOffset ,y,-connectorOffset)
        };
        return connections;
    }
    public void InstantiateConnectors()
    {
        connectors = CreateConnectors(transform.position.y+height);
        for (int i = 0; i < 4; i++)
        {
            if (points[i] != null) DestroyImmediate(points[i]);
            GameObject newObj = new GameObject();
            points[i] = newObj;

            points[i].name = "point " + (i+1).ToString();
            points[i].transform.parent = transform;
            points[i].transform.localPosition = connectors[i]; 
        }
    }
    public void SetConnectors()
    {
        connectors = CreateConnectors(transform.position.y+height);
        for (int i = 0; i < 4; i++)
        {
            Debug.DrawLine(points[(i+1)%4].transform.position, points[(i+2)%4].transform.position, Color.cyan,Time.deltaTime);
            points[i].transform.localPosition = connectors[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
