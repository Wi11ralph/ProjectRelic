using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private ParticleSystem Orb;
    [SerializeField] private ParticleSystem Orb2;
    [SerializeField] private ParticleSystem OuterGlow;
    [SerializeField] private ParticleSystem Smoke;
    [SerializeField] private Light pl;

    private float timeAlive;
    private void Start()
    {
        timeAlive = 0f;
    }
    private void Awake()
    {
        Orb = this.gameObject.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame

    [System.Obsolete]
    void Update()
    {
        timeAlive += Time.deltaTime; 
        if (timeAlive < 3f) return;
        
        LerpAlpha(Orb,5f);
        LerpAlpha(Orb2,5f);
        LerpAlpha(OuterGlow,5f);
        LerpAlpha(Smoke,1.25f);

        pl.intensity = Mathf.Lerp(pl.intensity, 0f, 1.5f * Time.unscaledDeltaTime);

        if (timeAlive > 6f) Destroy(this.gameObject);
    }
    [System.Obsolete]
    private void LerpAlpha(ParticleSystem p,float t)
    {
        p.startColor = new(
            p.startColor.r,
            p.startColor.g,
            p.startColor.b,
            Mathf.Lerp(p.startColor.a, 0f, t * Time.unscaledDeltaTime)
        );
    }
}
