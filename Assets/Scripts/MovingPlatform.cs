using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [System.Serializable]
    private class WayPoints
    {
        public Transform wayPointPosition; 
        public float speed;
        public float waitTime;

        [HideInInspector]
        public Vector3 pos()
        {
            return wayPointPosition.position;
        }
        
    }
    [Header("Path")]
    [SerializeField] private WayPoints[] wayPoints = new WayPoints[1];

    [Header("Physics")]
    [SerializeField] private float height;
    [SerializeField] private float spring = 10f;

    [Header("Connections/connectors")]
    [SerializeField] private float connectorOffset;
    [SerializeField] private float connectionOffset;
    

    [HideInInspector] public GameObject[] points;
    private Vector3[] connectors = new Vector3[4];
    private Vector3[] connections = new Vector3[4];
    private SpringJoint[] joints = new SpringJoint[4];
    private Vector3 startPos;
    private Color orange;
    
    
    private Vector3 currentPathTarget; 
    private int pathPos = 0;
    private bool wait = false;
    private float waitTime = 0;

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
        int thisPos = (pathPos+1) % wayPoints.Length;
        int lastPos = pathPos % wayPoints.Length;

        if(wait)
        {

            waitTime += Time.deltaTime;
            if (waitTime >= wayPoints[lastPos].waitTime)
            {
                wait = false;
                waitTime = 0;
            }
            else return;
        } 

        Vector2 pathXZ = Vector2.MoveTowards(
            new Vector2(transform.position.x, transform.position.z),
            new Vector2(wayPoints[thisPos].pos().x, wayPoints[thisPos].pos().z),
            wayPoints[lastPos].speed * Time.deltaTime
        );

        startPos = new Vector3(
            pathXZ.x,
            Mathf.MoveTowards(startPos.y, wayPoints[thisPos].pos().y, wayPoints[lastPos].speed * Time.deltaTime),
            pathXZ.y
         );
        
        /*
        for (int i = 0; i < 4; i++)
        {
            points[i].transform.position = Vector3.MoveTowards(transform.position, wayPoints[thisPos].pos(), wayPoints[lastPos].speed * Time.deltaTime);
        }
        */
        Debug.DrawLine(startPos, wayPoints[thisPos].pos(), Color.red,Time.deltaTime);
        transform.position = new Vector3(
            startPos.x,
            transform.position.y,
            startPos.z 
        );
        //if at the end of path
        if (Vector3.Distance(startPos, wayPoints[thisPos].pos()) < wayPoints[lastPos].speed * Time.deltaTime)
        {
            pathPos++;
            wait = true;
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        int thisPos = (pathPos + 1) % wayPoints.Length;
        int lastPos = pathPos % wayPoints.Length;

        if (collision.gameObject.GetComponent<Rigidbody>() && collision.gameObject.GetComponent<Player>() && !wait)
        {
            Vector2 pathXZ = new Vector2(transform.position.x, transform.position.z) - Vector2.MoveTowards(
                new Vector2(transform.position.x, transform.position.z),
                new Vector2(wayPoints[thisPos].pos().x, wayPoints[thisPos].pos().z),
                wayPoints[lastPos].speed * Time.deltaTime
            );

            collision.transform.position += new Vector3(
                -1*pathXZ.x,
                0f,
                -1*pathXZ.y
            );
        }
        
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
            points[i].transform.position = connectors[i] + startPos;
             
            joints[i].anchor = connections[i]; 
            joints[i].spring = spring;
        }
    }
    private void OnDrawGizmos()
    {
        for (int i = 0; i < 4; i++) {

            int firstPoint = (i) % 4;
            int secoundPoint = (i + 1) % 4; 
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(points[firstPoint].transform.position, points[secoundPoint].transform.position/*, Color.cyan,0.5f*/);
            Gizmos.DrawLine(Vector3Multiply(connections[firstPoint], transform.localScale) + transform.position, Vector3Multiply(connections[secoundPoint], transform.localScale) + transform.position/*, Color.cyan, 0.5f*/);
            Gizmos.color = new Color { r = 0.988f, g = 0.55686f, b = 0.247058f, a = 1 };
            Gizmos.DrawLine(points[firstPoint].transform.position, Vector3Multiply(connections[firstPoint], transform.localScale) + transform.position/*, Color.blue, 0.5f*/);
            
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
