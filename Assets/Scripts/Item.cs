using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Vector3 realStartPos;
    private Vector3 startPos;

    private Material newMat;
    [SerializeField] private float rotationSpeed = 72f;
    [SerializeField] private float bobbingSpeed = 1.5f;
    [SerializeField] private float bobbingRange = 0.5f; //0.5 up, 0.5 down
    private RaycastHit hit;
    private enum itemOption
    {
        air,
        fire,
        nature,
        key
    }

    [SerializeField] private itemOption selectedItem = itemOption.air;
    // Start is called before the first frame update
    void Start()
    {
        realStartPos = transform.position;
        startPos = realStartPos;

        
        Renderer rend = this.gameObject.GetComponent<Renderer>();

        switch (selectedItem)
        {
            case itemOption.air:
                newMat = Resources.Load("airMat", typeof(Material)) as Material; 
                break;
            case itemOption.fire:
                newMat = Resources.Load("fireMat", typeof(Material)) as Material; 
                break;
            case itemOption.nature:
                newMat = Resources.Load("natureMat", typeof(Material)) as Material;
                break;
        }
        rend.material = newMat;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("You collided with the object!");
            switch(selectedItem)
            {
                case itemOption.air:
                    Player.airRelic = true;
                    break;
                case itemOption.fire:
                    Player.fireRelic = true;
                    break;
                case itemOption.nature:
                    Player.natureRelic = true;
                    break;
                case itemOption.key:
                    Player.keys.Add(1);
                    break;
            }
            Destroy(this.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

        transform.eulerAngles = new Vector3(
            transform.eulerAngles.x,
            transform.eulerAngles.y + (Time.deltaTime * rotationSpeed),
            transform.eulerAngles.z
        );
        transform.position = new Vector3(
            transform.position.x,
            startPos.y +(bobbingRange*Mathf.Sin(Time.time*bobbingSpeed)),
            transform.position.z
        );
        Ray ray = new Ray(startPos, -Vector3.up);
        Physics.Raycast(ray, out hit);
        float dist = startPos.y - (hit.point.y);
        Debug.DrawRay(startPos, Vector3.down * dist, Color.blue);
        //if (startPos.y - 0.25f - hit.point.y >= bobbingRange)
        //{
            startPos = new Vector3(
                startPos.x,
                Mathf.Lerp(startPos.y, hit.point.y + bobbingRange + 0.5f, Time.deltaTime *bobbingSpeed),
                startPos.z
            );
        //}
    }
}
