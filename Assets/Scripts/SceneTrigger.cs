using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    [SerializeField] private SceneLoader sceneLoader;
    [SerializeField] private string sceneName;
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 spawnPosition;
    [SerializeField] private float spawnZoom;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player) return;
        sceneLoader.LoadScene(sceneName,SceneLoader.SpawnType.nextLevel, spawnPosition,spawnZoom);
    }
}
