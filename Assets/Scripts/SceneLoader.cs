using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    //private string scenePath = "Assets/Scenes/"; 
    [SerializeField] GameObject player;

    [SerializeField] private Vector3 spawnPoint;
    [SerializeField] GameObject cam;
    [SerializeField] private Animator transition;

    public static Vector3 pos = new (0,0,0);
    public static Vector3 camPos = new (0,0,0);
    public static Vector3 camOffset = new(0, 0, 0);
    public static Quaternion rot = new (0,0,0,0);
    public static float pzoom = 5f;
    public static float currentZoom = 5f;

    private bool fireR;
    private bool airR;
    private bool natureR;
    private List<int> keys;
    private void Start()
    {
        fireR = Player.fireRelic;
        airR = Player.airRelic;
        natureR = Player.natureRelic;
        try
        {
            keys = player.GetComponent<Player>().keys;
        } catch(UnityEngine.UnassignedReferenceException)
        {}

    }
    public enum SpawnType
    {
        reset, //teleports you to beginning, retains cam pos
        nextLevel, //teleports you, retains cam offset
        nextRoom //retains transforms
    }
    public static SpawnType spawnT = SpawnType.reset;

    /*
    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 30), "Change Scene")) LoadScene(scene, SpawnType.nextRoom);
    }
    */
    private void LateUpdate()
    {
        transition.SetBool("Loaded",true);
    }
    public void LoadScene(string s, SpawnType spawnType, Vector3? position = null,float? zoom = 5)
    {  
        camPos = cam.transform.position;
        camOffset = camPos - player.transform.position;
        if (spawnType != SpawnType.reset ) 
        {
            rot = player.transform.rotation;
            currentZoom = cam.GetComponentInChildren<Camera>().orthographicSize;
            if (spawnType == SpawnType.nextRoom) pos = player.transform.position;
            else
            {
                pos   = (Vector3) position;
                pzoom = (float)       zoom;
            }
        }  else
        {
            Player.fireRelic = fireR;
            Player.airRelic = airR;
            Player.natureRelic = natureR;
            player.GetComponent<Player>().keys = keys;
        }
        spawnT = spawnType; 
        StartCoroutine(LoadLevel(s));
         
    }

    public IEnumerator LoadLevel(string levelName)
    { 
        transition.SetTrigger("Start");
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

}
