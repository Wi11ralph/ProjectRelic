using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    //private string scenePath = "Assets/Scenes/";
    [SerializeField] private string scene = "Room2";
    [SerializeField] GameObject player;
    [SerializeField] GameObject cam;
    public static Vector3 pos = new (0,0,0);
    public static Vector3 camPos = new (0,0,0);
    public static Quaternion rot = new (0,0,0,0);
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Change Scene")) LoadScene();
    }
    void LoadScene()
    {
        pos = player.transform.position;
        camPos = cam.transform.position;
        rot = player.transform.rotation;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

}
