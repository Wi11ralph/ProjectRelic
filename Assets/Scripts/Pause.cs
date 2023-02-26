using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Pause : MonoBehaviour
{
    private bool input;
    public static bool active;
    private bool wait;
    private float scaleTime = 1f;

    [SerializeField] private CanvasGroup tint;

    void Update()
    {
        GatherInput();

        if (input) Pauser();
        else wait = false;

        if (active)
        {
            scaleTime = Mathf.Lerp(scaleTime, 0f, 3f * Time.unscaledDeltaTime);
            if (scaleTime <= 0.009) scaleTime = 0;
        } else
        {
            scaleTime = Mathf.Lerp(scaleTime, 1f, 12f * Time.unscaledDeltaTime);
            if (scaleTime >= 0.99999) scaleTime = 1;
        }
        Debug.Log(scaleTime);
        Debug.Log(Time.timeScale);
        Time.timeScale = scaleTime;
    }
    private void Pauser()
    {
        
        if (!wait)
        {
            active = !active;
            if (!active)
            {
                StartCoroutine(FadeCanvasGroup(tint, tint.alpha, 0, .5f));
                //Time.timeScale = 1;
            }
            else
            {
                StartCoroutine(FadeCanvasGroup(tint, tint.alpha, 1, .5f));
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
