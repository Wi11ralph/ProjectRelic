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
    [SerializeField] private GameObject fade;

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

    private Vector3 titleTarget;
    private Vector3 characterTarget;
    private Vector3 buttonsTarget;
    [Header("AnimPos")]

    [SerializeField] private float titleAnimHeight;
    [SerializeField] private float characterAnimHeight;
    [SerializeField] private float buttonsAnimHeight;
    [SerializeField] private float animSpeed;
    [SerializeField] private float leeway;

    [Header("Ref")]
    [SerializeField] private SceneLoader sc;

    public void OnPlayClick()
    {
        titleTarget = SetTarget(title, titleAnimHeight, titleTarget, true);
        characterTarget = SetTarget(character, characterAnimHeight, characterTarget, true);
        buttonsTarget = SetTarget(buttons, buttonsAnimHeight, buttonsTarget, true);

        Player.fireRelic = false;
        Player.airRelic = false;
        Player.natureRelic = false; 

        StartCoroutine(sc.LoadLevel(firstLevel));
    }
    public void OnQuitClick()
    {
        Application.Quit();
    }

    private void Start()
    {
        Time.timeScale = 1;
        startPos = background.transform.position;
        targetPos = startPos;

        titleTarget     = SetTarget(title    , titleAnimHeight    , titleTarget     );
        characterTarget = SetTarget(character, characterAnimHeight, characterTarget );
        buttonsTarget   = SetTarget(buttons  , buttonsAnimHeight  , buttonsTarget   );
        startY = character.transform.position.y;
    }
    private Vector3 SetTarget(GameObject obj,float height,Vector3 target,bool animOut = false)
    {
        if (!animOut)
        {
            target = obj.transform.position;

            obj.transform.position = new Vector3(
                obj.transform.position.x,
                height,
                obj.transform.position.z
            );
        } else
        {
            target = new Vector3(
                obj.transform.position.x,
                height,
                obj.transform.position.z
            );
        }
        return target;
    }
    private bool firstFrame = true;
    private void LateUpdate()
    {
        if (!firstFrame) return;
        firstFrame = false; 
    }
    // Update is called once per frame
    void Update()
    {
        if (firstFrame) return;
        title.transform.position = Vector3.Lerp(title.transform.position, titleTarget, Time.deltaTime * animSpeed);
        character.transform.position = Vector3.Lerp(character.transform.position, characterTarget, Time.deltaTime * animSpeed);
        buttons.transform.position = Vector3.Lerp(buttons.transform.position, buttonsTarget, Time.deltaTime * animSpeed);

        //title sine scaling
        title.transform.localScale = new Vector3(
            1f + (SineScale * Mathf.Sin(Time.time * SineSpeed)),
            1f + (SineScale * Mathf.Sin(Time.time * SineSpeed)),
            1f
        );
        //end
        //character bobbing
        startY = Mathf.Lerp(startY, characterTarget.y, Time.deltaTime * animSpeed);

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
        if (
            Input.mousePosition.x > rect.position.x - (rect.sizeDelta.x / 2) && (rect.position.x - (rect.sizeDelta.x / 2)) + rect.sizeDelta.x > Input.mousePosition.x &&
            Input.mousePosition.y > rect.position.y - (rect.sizeDelta.y / 2) && (rect.position.y - (rect.sizeDelta.y / 2)) + rect.sizeDelta.y > Input.mousePosition.y
        ) button.transform.localScale = Vector3.Lerp(button.transform.localScale, new(hoverScale, hoverScale, 1f), Time.deltaTime * hoverSpeed);
        else button.transform.localScale = Vector3.Lerp(button.transform.localScale, new(1f, 1f, 1f), Time.deltaTime * hoverSpeed);
    }
}
