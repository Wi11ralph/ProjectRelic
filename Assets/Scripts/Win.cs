using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{

    [Header("Images")]
    [SerializeField] private GameObject title;
     
    [Header("Sine")]
    [SerializeField] private float SineScale;
    [SerializeField] private float SineSpeed;

    [Header("Shake")]
    [SerializeField] private GameObject background;
    [SerializeField] private Vector3 magnitude;
    [SerializeField] private float wavelength;
    [SerializeField] private float shakeFrequency;
    [SerializeField] private float lerpingScale;
    private float startY; 
    private float currentX = 0;
    private Vector3 startPos;
    private Vector3 targetPos;
    private float time;

    private Vector3 titleTarget;
    private Vector3 characterTarget;
    private Vector3 buttonsTarget;

    public void OnQuitClick()
    {
        Application.Quit();
    }

    private void Start()
    {
        startPos = background.transform.position;
        targetPos = startPos;
        Time.timeScale = 1;
    }
    // Update is called once per frame
    void Update()
    {

        //title sine scaling
        title.transform.localScale = new Vector3(
            1f + (SineScale * Mathf.Sin(Time.time * SineSpeed)),
            1f + (SineScale * Mathf.Sin(Time.time * SineSpeed)),
            1f
        );
        //end

        //small camera shake
        time += Time.deltaTime;
        if (time > shakeFrequency)
        {
            Vector3 shakeAmount = new Vector3(
                    Mathf.PerlinNoise(currentX, 0) - .5f,
                    Mathf.PerlinNoise(currentX, 7) - .5f,
                    Mathf.PerlinNoise(currentX, 19) - .5f
                );

            targetPos = Vector3.Scale(magnitude, shakeAmount) + startPos;
            time = 0;
        }
        currentX += wavelength; 
        background.transform.position = Vector3.Lerp(background.transform.position, targetPos, Time.deltaTime * lerpingScale);
        //end
    }
}
