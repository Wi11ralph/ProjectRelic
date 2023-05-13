using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private float hoverScale;
    [SerializeField] private float hoverSpeed;

    [Header("Images")]
    [SerializeField] private GameObject title;
    [SerializeField] private GameObject character;

    [Header("Play button")]
    [SerializeField] private string firstLevel;
     
    [Header("Sine")]
    [SerializeField] private float SineScale;
    [SerializeField] private float SineRange;
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
     
    public void OnPlayClick()
    {
        SceneManager.LoadScene(firstLevel, LoadSceneMode.Single);
    }
    public void OnQuitClick()
    {
        Application.Quit();
    }

    private void Start()
    {
        startY = character.transform.position.y;
        startPos = background.transform.position;
        targetPos = startPos;
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
        //character bobbing
        character.transform.position = new Vector3(
            character.transform.position.x,
            startY+(SineRange * Mathf.Sin(Time.time * SineSpeed)),
            character.transform.position.z
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

        IfHovering(playButton);
        IfHovering(quitButton);

    }
    private void IfHovering(GameObject button)
    { 
        RectTransform rect = button.GetComponent<RectTransform>(); 
        if (Input.mousePosition.x > rect.position.x - (rect.sizeDelta.x / 2)) Debug.Log("xOver");
        if (
            Input.mousePosition.x > rect.position.x - (rect.sizeDelta.x / 2) && (rect.position.x - (rect.sizeDelta.x / 2)) + rect.sizeDelta.x > Input.mousePosition.x &&
            Input.mousePosition.y > rect.position.y - (rect.sizeDelta.y / 2) && (rect.position.y - (rect.sizeDelta.y / 2)) + rect.sizeDelta.y > Input.mousePosition.y
        ) button.transform.localScale = Vector3.Lerp(button.transform.localScale, new(hoverScale, hoverScale, 1f), Time.deltaTime * hoverSpeed);
        else button.transform.localScale = Vector3.Lerp(button.transform.localScale, new(1f, 1f, 1f), Time.deltaTime * hoverSpeed);
    }
}
