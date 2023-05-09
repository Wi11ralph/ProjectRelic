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
    private Color orange;
    [SerializeField] private float spring = 10f;
    [Header("Path")]
    [SerializeField] private float speed;
    [SerializeField] private Transform startPathPos;
    [SerializeField] private Transform endPathPos;
    private Vector3 pathPos;

    private void Awake()
    {
        startPos = transform.position;
        
        orange.r = 255;
        orange.g = 165;
        orange.b = 0;
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
    private void PathFollow()
    {
         
        startPos = new Vector3(
            Mathf.MoveTowards(transform.position.x, endPathPos.position.x, speed * Time.deltaTime),
            startPos.y,
            Mathf.MoveTowards(transform.position.z, endPathPos.position.z, speed * Time.deltaTime)
         );
         
        for (int i = 0; i < 4; i++)
        {
            points[i].transform.position = Vector3.MoveTowards(transform.position, endPathPos.position, speed * Time.deltaTime);
        }

        //Debug.DrawLine(transform.position, endPathPos.position,Color.red,Time.deltaTime);
        transform.position = new Vector3(
            startPos.x,
            transform.position.y,
            startPos.z 
        );

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
            int firstPoint   = (i) % 4;
            int secoundPoint = (i+1) % 4;
            Debug.Log(firstPoint + " " + secoundPoint);
            if(secoundPoint != 0) Debug.DrawLine(points[firstPoint].transform.position, points[secoundPoint].transform.position, Color.cyan,0.5f);
            points[i].transform.position = connectors[i] + startPos;

            
            Debug.DrawLine(points[firstPoint].transform.position, Vector3Multiply(connections[firstPoint], transform.localScale) + transform.position, Color.blue, 0.5f);

            Debug.DrawLine(Vector3Multiply(connections[firstPoint],transform.localScale) + transform.position, Vector3Multiply(connections[secoundPoint],transform.localScale)+ transform.position, Color.cyan, 0.5f);
            joints[i].anchor = connections[i];
            if (points[secoundPoint].transform.position == transform.position) Debug.Log("true");
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
        PathFollow();
        SetConnectors();
    }


}
