using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    //private string scenePath = "Assets/Scenes/";
    [SerializeField] private string scene = "Room2";
    [SerializeField] GameObject player;
    public static Vector3 pos;
    public static Vector3 rot;
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Change Scene"))
        {
            pos = player.transform.position;
            Debug.Log("Scene loading: " + scene);
            SceneManager.LoadScene(scene, LoadSceneMode.Single);
        }
    }

}
