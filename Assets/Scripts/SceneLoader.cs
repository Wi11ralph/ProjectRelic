using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    //private string scenePath = "Assets/Scenes/";
    [SerializeField] private string scene = "Room2";
    [SerializeField] GameObject player;
    public static Vector3 pos = new (0,0,0);
    public static Quaternion rot = new (0,0,0,0);
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Change Scene")) LoadScene();
    }
    void LoadScene()
    {
        Debug.Log("b4");
        Debug.Log("Position:" + pos + "Rotation:" + rot);
        pos = player.transform.position;
        rot = player.transform.rotation;
        Debug.Log("Position:" +pos+ "Rotation:"+ rot);
        Debug.Log("Scene loading: " + scene);
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

}
