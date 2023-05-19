using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Pause : MonoBehaviour
{
    private bool input;
    private bool mouse;

    public static bool active;
    public static bool wait;
    private bool menuCommitted = false;

    public static float currentValue;
    private float scaleTime = 1f;

    [SerializeField] private CanvasGroup tint;
    [SerializeField] private GameObject t;

    [SerializeField] private GameObject Relics;
    private GameObject fire;
    private GameObject nature;
    private GameObject air;

    [SerializeField] private AudioSource select;
    #pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    [SerializeField] private AudioClip audio;
    #pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    [SerializeField] private AudioSource ambienceAudio;
    private enum Button
    {
        Continue,
        Restart,
        Menu,
        None
    }
    public class Bttn
    {
        private GameObject txt;
        private GameObject L1;
        private GameObject L2;

        private Vector3 txtStartSize;
        private Vector3 L1StartPos;
        private Vector3 L2StartPos;
        
        private float top;
        private float bottom;

        
        public Bttn (GameObject buton,float yMax,float yMin)
        {
            top    = yMax;
            bottom = yMin;
         
            for(int i=0; i < buton.transform.childCount; i++)
            {
                if(buton.transform.GetChild(i).name == "L1")
                {
                    L1 = buton.transform.GetChild(i).gameObject;
                    L1StartPos = L1.transform.localPosition;
                }
                if (buton.transform.GetChild(i).name == "L2")
                {
                    L2 = buton.transform.GetChild(i).gameObject;
                    L2StartPos = L2.transform.localPosition;
                }
                if (buton.transform.GetChild(i).name == "txt")
                {
                    txt = buton.transform.GetChild(i).gameObject;
                    txtStartSize = buton.transform.GetChild(i).localScale;
                }
            }
        }
        private void Offset(float amount)
        {
            txt.transform.localScale = Vector3.Lerp(txt.transform.localScale, new Vector3(1.3f, 0.95f, 1f), 5f * Time.unscaledDeltaTime);
            L1.transform.localPosition = Vector3.Lerp(L1.transform.localPosition, L1StartPos - new Vector3(-amount, 0f, 0f), 5f * Time.unscaledDeltaTime);
            L2.transform.localPosition = Vector3.Lerp(L2.transform.localPosition, L2StartPos - new Vector3( amount, 0f, 0f), 5f * Time.unscaledDeltaTime);
        }
        private void Reset()
        {
            txt.transform.localScale = Vector3.Lerp(txt.transform.localScale, txtStartSize, 5f * Time.unscaledDeltaTime);
            L1.transform.localPosition = Vector3.Lerp(L1.transform.localPosition, L1StartPos, 8f * Time.unscaledDeltaTime);
            L2.transform.localPosition = Vector3.Lerp(L2.transform.localPosition, L2StartPos, 8f * Time.unscaledDeltaTime);
        }
        public bool Hover(bool horizontalCheck)
        {
            if (bottom <= Input.mousePosition.y && Input.mousePosition.y <= top && horizontalCheck)
            { 
                Offset(50f);
                return true;
            }
            else
            {
                Reset();
                return false;
            }
        }
    }
    private Button bttn;
    private Button lastBttn;

    

    [SerializeField] private GameObject bt1;
    [SerializeField] private GameObject bt2;
    [SerializeField] private GameObject bt3;
    [SerializeField] private SceneLoader sc;

    private Bttn button1;
    private Bttn button2;
    private Bttn button3;
    private bool xLined()
    {
        if (235 + t.transform.localPosition.x <= Input.mousePosition.x && Input.mousePosition.x <= 760 + t.transform.localPosition.x) return true;
        else return false;
    }
    private void Start()
    {
        tint.alpha = currentValue;
        
        button1 = new Bttn(bt1,660,560);
        button2 = new Bttn(bt2,485,385);
        button3 = new Bttn(bt3,310,210);

        active = false;
        tint.alpha = 0;

        air = Relics.transform.GetChild(0).gameObject;
        fire = Relics.transform.GetChild(1).gameObject;
        nature = Relics.transform.GetChild(2).gameObject;
    }
    private bool firstFrame = true;
    private void LateUpdate()
    {
        if (!firstFrame) return;
        firstFrame = false;
        t.transform.localPosition = new(-600f, 0f, 0f);
    }
    void Update()
    { 
        //
        air.SetActive(Player.airRelic);
        fire.SetActive(Player.fireRelic);
        nature.SetActive(Player.natureRelic);

        ambienceAudio.volume = Time.timeScale;
        IfHover();
        if (mouse && active) Click();
        GatherInput();

        if (input && !menuCommitted) Pauser();
        else wait = false;

        if (active && !firstFrame)
        {
            scaleTime = Mathf.Lerp(scaleTime, 0f, 3f * Time.unscaledDeltaTime);
            if (scaleTime <= 0.009) scaleTime = 0;
            if (!menuCommitted)
            {
                t.transform.localPosition = new(Mathf.Lerp(t.transform.localPosition.x, 0f, 5f * Time.unscaledDeltaTime), 0f, 0f);
                Relics.transform.localPosition = new(570f, Mathf.Lerp(Relics.transform.localPosition.y, 48f, 5f * Time.unscaledDeltaTime), 0f);
            }
        } else if(!firstFrame)
        {
            scaleTime = Mathf.Lerp(scaleTime, 1f, 12f * Time.unscaledDeltaTime);
            if (scaleTime >= 0.99999) {
                scaleTime = 1;
                menuCommitted = false;
            }
            t.transform.localPosition = new(Mathf.Lerp(t.transform.localPosition.x, -600f, 5f * Time.unscaledDeltaTime), 0f, 0f);
            Relics.transform.localPosition = new(570f, Mathf.Lerp(Relics.transform.localPosition.y, 1350f, 5f * Time.unscaledDeltaTime), 0f);
        }
        if (menuCommitted)
        {
            t.transform.localPosition = new(Mathf.Lerp(t.transform.localPosition.x, -600f, 5f * Time.unscaledDeltaTime), 0f, 0f);
            Relics.transform.localPosition = new(570f, Mathf.Lerp(Relics.transform.localPosition.y, 1350f, 5f * Time.unscaledDeltaTime), 0f);
        }
        Time.timeScale = scaleTime;
    }
    private void IfHover()
    {
        if (menuCommitted) return;
        lastBttn = bttn;

        bttn = Button.None;
        try
        {
            if (button1.Hover(xLined())) bttn = Button.Continue;
            if (button2.Hover(xLined())) bttn = Button.Restart;
            if (button3.Hover(xLined())) bttn = Button.Menu;
        } catch (System.NullReferenceException)
        {
            button1 = new Bttn(bt1, 660, 560);
            button2 = new Bttn(bt2, 485, 385);
            button3 = new Bttn(bt3, 310, 210);
        }
        if (bttn != lastBttn && bttn != Button.None && active) select.PlayOneShot(audio);
    }
    private void Click()
    {
        if (menuCommitted) return;
        switch (bttn)
        {
            case Button.Continue:
                Pauser();
                break;
            case Button.Restart:
                Scene sm = SceneManager.GetActiveScene();
                sc.LoadScene(sm.name,SceneLoader.SpawnType.reset);
                //SceneManager.LoadScene(sm.name);
                break;
            case Button.Menu:
                sc.StartCoroutine(sc.LoadLevel("Menu"));
                break;
            case Button.None:
                return;
        }
        menuCommitted = true;
    }
    private void Pauser()
    {
        
        if (!wait)
        {
            active = !active;
            if (!active)
            {
                StartCoroutine(FadeCanvasGroup(tint, tint.alpha, 0, .5f));
                //Cursor.visible = false;
                //Time.timeScale = 1;
            }
            else
            {
                StartCoroutine(FadeCanvasGroup(tint, tint.alpha, 1, .5f));
                //Cursor.visible = true;
                //Time.timeScale = 0;
            }
        }
        wait = true;
    }

    private void GatherInput()
    {
        input = Input.GetKeyDown(KeyCode.Escape);
        mouse = Input.GetButtonDown("Fire1");
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

            currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1) break;

            yield return new WaitForEndOfFrame();
        }
    }
}
