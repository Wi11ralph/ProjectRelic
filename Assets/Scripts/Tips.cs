using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tips : MonoBehaviour
{


    [SerializeField] private GameObject tips;
    //private CanvasGroup cG;
    private static CanvasGroup canvas;
    private static float time = 20f;
    //private static 
    // Start is called before the first frame update
    void Start()
    {
        canvas = tips.GetComponent<CanvasGroup>();
        canvas.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time == 5f) canvas.alpha = 0;
    }
    public static void TriggerTip(Item.itemOption tipType) {
        time = 0f;
        canvas.alpha = 1;
    }

}
