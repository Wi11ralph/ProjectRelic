using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{



    
    [SerializeField] private CanvasGroup tipCG;
    /*
    [System.Serializable]
    private class Tip
    {
        public GameObject popup;
        private GameObject text;
        private GameObject image;

    }
    [SerializeField] private static Tip[] tips = new Tip[3];
    */
    [SerializeField] private GameObject[] popups = new GameObject[3];
    private static GameObject[] tips = new GameObject[3];
    //private CanvasGroup cG;
    private CanvasGroup canvas;
    private static float time = 20f;
    //private static 
    // Start is called before the first frame update
    void Start()
    {
        canvas = tipCG.GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        for(int i = 0; i < tips.Length; i++)
        {
            Debug.Log(i);
            tips[i] = popups[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time < 10f) canvas.alpha = Mathf.Lerp(canvas.alpha, 4, 2f * Time.unscaledDeltaTime);
        if (time < 20f) canvas.alpha = Mathf.Lerp(canvas.alpha, 0, 5.5f * Time.unscaledDeltaTime);
        if (time == 30f) {
            canvas.alpha = 0;
            SetActive(101);
        }
    }
    public static void TriggerTip(Item.itemOption tipType) {
        time = 0f;
        
        switch (tipType)
        {
            case Item.itemOption.air:
                SetActive(0);
                break;
            case Item.itemOption.fire:
                SetActive(1);
                break;
            case Item.itemOption.nature:
                SetActive(2);
                break;
            default:
                SetActive(101);
                break;
        }
    }
    
    private static void SetActive(int e)
    { 
        for(int i = 0; i < tips.Length; i++)
        {
            tips[i].SetActive(false);
        }
        if(e < tips.Length) tips[e].SetActive(true);
    }

}
