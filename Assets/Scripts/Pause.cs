using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Pause : MonoBehaviour
{
    private bool input;
    public static bool active;
    private bool wait;
    private float scaleTime = 1f;
    private enum Button
    {
        Continue,
        Restart,
        Menu,
        None
    }
    private Button bttn;

    [SerializeField] private CanvasGroup tint;
    [SerializeField] private GameObject t;

    [SerializeField] private GameObject button1;
    [SerializeField] private GameObject button2;
    [SerializeField] private GameObject button3;

    void Update()
    {
        IfHover();
        GatherInput();

        if (input) Pauser();
        else wait = false;

        if (active)
        {
            scaleTime = Mathf.Lerp(scaleTime, 0f, 3f * Time.unscaledDeltaTime);
            if (scaleTime <= 0.009) scaleTime = 0;
            t.transform.localPosition = new (Mathf.Lerp(t.transform.localPosition.x, 0f, 5f * Time.unscaledDeltaTime),0f,0f);
        } else
        {
            scaleTime = Mathf.Lerp(scaleTime, 1f, 12f * Time.unscaledDeltaTime);
            if (scaleTime >= 0.99999) scaleTime = 1;
            t.transform.localPosition = new(Mathf.Lerp(t.transform.localPosition.x, -600f, 5f * Time.unscaledDeltaTime), 0f, 0f);
        }
        Debug.Log(t.transform.localPosition);

        //Debug.Log(scaleTime);
        //Debug.Log(Time.timeScale);
        Time.timeScale = scaleTime;
    }
    private void IfHover()
    {
        if (235 + t.transform.localPosition.x <= Input.mousePosition.x && Input.mousePosition.x <= 760 + t.transform.localPosition.x)

        {
            if (560 <= Input.mousePosition.y && Input.mousePosition.y <= 660)
            {
                //button 1
                bttn = Button.Continue;
            }
            else if (385 <= Input.mousePosition.y && Input.mousePosition.y <= 485)
            {
                //button 2
                bttn = Button.Restart;
            }
            else if (210 <= Input.mousePosition.y && Input.mousePosition.y <= 310)
            {
                //button 3
                bttn = Button.Menu;

            } else bttn = Button.None;
        } else bttn = Button.None;
              
    }
    private void Pauser()
    {
        
        if (!wait)
        {
            active = !active;
            if (!active)
            {
                StartCoroutine(FadeCanvasGroup(tint, tint.alpha, 0, .5f));
                Cursor.visible = false;
                //Time.timeScale = 1;
            }
            else
            {
                StartCoroutine(FadeCanvasGroup(tint, tint.alpha, 1, .5f));
                Cursor.visible = true;
                //Time.timeScale = 0;
            }
        }
        wait = true;
    }

    private void GatherInput()
    {
        input = Input.GetKeyDown(KeyCode.Escape);
    }
    public IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float lerpTime = 1)
    {
        float _timeStartedLerping = Time.unscaledTime;
        float timeSinceStarted = Time.unscaledTime - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;
        while (true)
        {
            timeSinceStarted = Time.unscaledTime - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
}
