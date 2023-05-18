using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] torches;
    [SerializeField] private int id;
    private static int iD;
    private class Torch
    {
        public GameObject torch;
        public GameObject light;
        public GameObject flame;
        public bool on = false;
        public Torch(GameObject torch_)
        {
            torch = torch_;
            for (int i = 0; i < torch_.transform.childCount; i++)
            {
                if (torch_.transform.GetChild(i).name == "light") light = torch_.transform.GetChild(i).gameObject;
                if (torch_.transform.GetChild(i).name == "flame") flame = torch_.transform.GetChild(i).gameObject;
            }
        }
    }
    private static Torch[] torch;
    // Start is called before the first frame update
    void Start()
    {
        torch = new Torch[torches.Length];
        for(int i = 0; i < torches.Length; i++)
        {
            torch[i] = new Torch(torches[i]);
            torch[i].flame.SetActive(false);
            torch[i].light.SetActive(false);
        }
        iD = id;
    }
    
    public static void FireballCollide(GameObject collision)
    {
        int on = 0;
        for (int i = 0; i < torch.Length; i++)
        {
            
            if (torch[i].torch == collision)
            {
                torch[i].on = true;
                torch[i].flame.SetActive(true);
                torch[i].light.SetActive(true); 
            }
            if (torch[i].on) on++;
            if (on == torch.Length)
            {
                GameEvents.current.DoorwayTriggerEnter(iD);
            }
        }
    }
}
