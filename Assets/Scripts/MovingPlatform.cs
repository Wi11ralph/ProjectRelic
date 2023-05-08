using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float connectorOffset;
    [SerializeField] private float connectionOffset;
    [SerializeField] private float height;
    [HideInInspector] public GameObject[] points;
    private Vector3[] connectors = new Vector3[4];
    private Vector3[] connections = new Vector3[4];
    private SpringJoint[] joints = new SpringJoint[4];
    private Vector3 startPos;
    [SerializeField] private float spring = 10f;

    private void Awake()
    {
        startPos = transform.position;
    }
    private Vector3[] CreateOffsets(float o,float y)
    {
        Vector3[] connections = {
            new(o ,y, o),
            new(-o,y, o),
            new(-o,y,-o),
            new(o ,y,-o)
        };
        return connections;
    }
    public void InstantiateConnectors()
    {
        if (!Application.isPlaying) startPos = transform.position;
        
            connectors  = CreateOffsets(connectorOffset,  transform.position.y   + height) ;
            connections = CreateOffsets(connectionOffset, transform.position.y)/*- height*/;

        while (GetComponents<SpringJoint>().Length != 0) DestroyImmediate(GetComponents<SpringJoint>()[0]); 
        for (int i = 0; i < 4; i++)
        {
            if (points[i] != null) DestroyImmediate(points[i]);
            //if (joints[i] != null) DestroyImmediate(joints[i]); 
            GameObject newObj = new GameObject();
            points[i] = newObj;

            Rigidbody rb = points[i].AddComponent<Rigidbody>(); 
            rb.isKinematic = true;

            joints[i] = this.gameObject.AddComponent<SpringJoint>();
            joints[i].autoConfigureConnectedAnchor = false;
            joints[i].connectedBody = rb;
            joints[i].maxDistance = height;
            joints[i].minDistance = height;
            joints[i].spring = spring;
            joints[i].anchor = connections[i];

            points[i].name = "point " + (i+1).ToString();
            //points[i].transform.parent = transform;
            points[i].transform.position = connectors[i] + startPos; 
        }
    }
    public void SetConnectors()
    {
        if (!Application.isPlaying) startPos = transform.position;

        connectors =  CreateOffsets(connectorOffset,  startPos.y   + height) ;
        connections = CreateOffsets(connectionOffset, startPos.y)/*- height*/;

        for (int i = 0; i < 4; i++)
        {
            joints = GetComponents<SpringJoint>();
            int firstPoint   = (i + 1) % 4;
            int secoundPoint = (i + 2) % 4;

            Debug.DrawLine(points[firstPoint].transform.position, points[secoundPoint].transform.position, Color.cyan,Time.deltaTime);
            points[i].transform.position = connectors[i] + startPos;
            
            Debug.DrawLine(Vector3Multiply(connections[firstPoint],transform.localScale) + startPos, Vector3Multiply(connections[secoundPoint],transform.localScale)+ startPos, Color.cyan, Time.deltaTime);
            joints[i].anchor = connections[i];

            joints[i].spring = spring;
        }
    }
    private Vector3 Vector3Multiply(Vector3 a, Vector3 b)
    {
        return new Vector3(
            a.x *b.x,
            a.y *b.y,
            a.z *b.z
        );

    }
    // Update is called once per frame
    void Update()
    {
        
    }


}
