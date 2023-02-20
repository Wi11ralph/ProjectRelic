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
    public static Vector3 camPos = new(-4.17f, 4.45f, -4.15f);
    public static Quaternion rot = new (0,0,0,0);
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Change Scene")) LoadScene();
    }
    void LoadScene()
    {
        pos = player.transform.position;
        rot = player.transform.rotation;
        camPos = cam.transform.position;
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

}
