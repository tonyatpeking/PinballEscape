using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineStretchy : MonoBehaviour {

    public GameObject m_fingerStart;
    public GameObject m_fingerEnd;
    public Vector3 m_startPos;
    public Vector3 m_endPos;

    float m_initialGOScaleX;

    float m_initiaUnlitScaleX;
    float m_initialUnlitTiledSizeX;
    float m_initiaLitScaleX;
    float m_initialLitTiledSizeX;
    float m_initiaGlowScaleX;
    float m_initialGlowTiledSizeX;


    SpriteRenderer m_unlitRenderer;
    SpriteRenderer m_litRenderer;
    SpriteRenderer m_glowRenderer;

    public SpriteRenderer m_fingerALit;
    public SpriteRenderer m_fingerAUnlit;

    public SpriteRenderer m_fingerBLit;
    public SpriteRenderer m_fingerBUnlit;

    bool m_isFading = false;
    float m_timeOfStartFlashFade;
    public float m_timeToFlash;
    public float m_timeToFade;
    public AnimationCurve m_flashCurve;
    public AnimationCurve m_flashScaleCurve;
    public AnimationCurve m_fadeCurve;

    public ParticleSystem m_ps;
    public float m_particlesPerLength;

    public void FlashStretchFade()
    {
        int numParticles = (int)(m_particlesPerLength * transform.localScale.x);
        m_ps.Emit(numParticles);
        m_timeOfStartFlashFade = Time.time;
        m_isFading = true;
    }

    private void UpdateFlashStretchFade()
    {
        if (!m_isFading)
            return;

        float timeSinceStartFlash = Time.time - m_timeOfStartFlashFade;
        if (timeSinceStartFlash > m_timeToFade )
        {
            m_isFading = false;
            SetFullFade();
        }
        else
        {
            float lerpTFade = timeSinceStartFlash / m_timeToFade;
            lerpTFade = Mathf.Clamp01(lerpTFade);
            float fadeAlpha = m_fadeCurve.Evaluate(lerpTFade);

            float lerpTFlash = timeSinceStartFlash / m_timeToFlash;
            lerpTFlash = Mathf.Clamp01(lerpTFlash);
            float flashAlpha = m_flashCurve.Evaluate(lerpTFlash);

            Color flashColor = new Color(1, 1, 1, flashAlpha);
            Color fadeColor = new Color(1, 1, 1, fadeAlpha);

            m_unlitRenderer.color = fadeColor;
            m_litRenderer.color = flashColor;
            m_glowRenderer.color = flashColor;

            m_fingerAUnlit.color = fadeColor;
            m_fingerALit.color = flashColor;
            m_fingerBUnlit.color = fadeColor;
            m_fingerBLit.color = flashColor;

            float scaleY = m_flashScaleCurve.Evaluate(lerpTFlash);
            SetScaleY(m_unlitRenderer.transform, scaleY);
            SetScaleY(m_litRenderer.transform, scaleY);
            SetScaleY(m_glowRenderer.transform, scaleY);

            Vector3 scale = new Vector3(scaleY, scaleY, 1);

            m_fingerAUnlit.transform.localScale = scale;
            m_fingerALit.transform.localScale = scale;
            m_fingerBUnlit.transform.localScale = scale;
            m_fingerBLit.transform.localScale = scale;
        }
    }

    void SetScaleY(Transform tf, float scaleY)
    {
        Vector3 scale = tf.localScale;
        scale.y = scaleY;
        tf.localScale = scale;
    }

    public void SetFullFade()
    {
        Color transparent = new Color(1, 1, 1, 0);
        Color faded = new Color(1, 1, 1, m_fadeCurve.Evaluate(1));

        m_unlitRenderer.color = faded;
        m_litRenderer.color = transparent;
        m_glowRenderer.color = transparent;

        m_fingerAUnlit.color = faded;
        m_fingerALit.color = transparent;
        m_fingerBUnlit.color = faded;
        m_fingerBLit.color = transparent;

        SetScaleY(m_unlitRenderer.transform, 1);
        SetScaleY(m_litRenderer.transform, 1);
        SetScaleY(m_glowRenderer.transform, 1);

        Vector3 scale = new Vector3(1, 1, 1);

        m_fingerAUnlit.transform.localScale = scale;
        m_fingerALit.transform.localScale = scale;
        m_fingerBUnlit.transform.localScale = scale;
        m_fingerBLit.transform.localScale = scale;
    }

    public void SetUnfade()
    {
        m_unlitRenderer.color = Color.white;
        m_fingerAUnlit.color = Color.white;
        m_fingerBUnlit.color = Color.white;
    }

    void Awake()
    {
        m_ps = GetComponent<ParticleSystem>();

        m_initialGOScaleX = transform.localScale.x;

        m_unlitRenderer = transform.Find("Unlit").GetComponent<SpriteRenderer>();
        m_initiaUnlitScaleX = m_unlitRenderer.transform.localScale.x;
        m_initialUnlitTiledSizeX = m_unlitRenderer.size.x;

        m_litRenderer = transform.Find("Lit").GetComponent<SpriteRenderer>();
        m_initiaLitScaleX = m_litRenderer.transform.localScale.x;
        m_initialLitTiledSizeX = m_litRenderer.size.x;

        m_glowRenderer = transform.Find("Glow").GetComponent<SpriteRenderer>();
        m_initiaGlowScaleX = m_glowRenderer.transform.localScale.x;
        m_initialGlowTiledSizeX = m_glowRenderer.size.x;

        SetFullFade();
        SetUnfade();
    }

    // special resize so tiled sprite works
    void ResizeSprite( SpriteRenderer renderer, float initialScaleX, float initialTiledSizeX, float fixedSideIncrease )
    {
        float xScaleRatioOfInitial = transform.localScale.x / m_initialGOScaleX;
        if (xScaleRatioOfInitial < 0.00001f)
        {
            xScaleRatioOfInitial = 0.001f;
        }

        Transform spriteTransform = renderer.transform;

        Vector3 newSpriteTransformScale = spriteTransform.localScale;
        newSpriteTransformScale.x = initialScaleX / xScaleRatioOfInitial;
        spriteTransform.localScale = newSpriteTransformScale;

        Vector2 newSpriteSize = renderer.size;
        newSpriteSize.x = initialTiledSizeX * xScaleRatioOfInitial + fixedSideIncrease;
        renderer.size = newSpriteSize;
    }

    public void SetStartAndEnd( Vector3 start, Vector3 end )
    {
        m_startPos = start;
        m_endPos = end;
    }

	void Start () {
        m_startPos = transform.position;
        m_endPos = transform.position;
    }

	void Update () {
        // Transform
        transform.position = (m_startPos + m_endPos) / 2f;

        // Rotation
        Vector2 normalizedDirVector = (m_startPos - m_endPos).normalized;
        float angle = Mathf.Atan2(normalizedDirVector.y, normalizedDirVector.x);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);

        m_fingerStart.transform.rotation = transform.rotation;
        m_fingerEnd.transform.rotation = transform.rotation;

        // Scale
        float width = (m_startPos - m_endPos).magnitude;
        transform.localScale = new Vector3(width, transform.localScale.y, transform.localScale.z);

        UpdateFlashStretchFade();
    }

    public void StretchToStartAndEnd(Vector3 start, Vector3 end)
    {
        SetStartAndEnd(start, end);

        // Transform
        transform.position = (m_startPos + m_endPos) / 2f;

        // Rotation
        Vector2 normalizedDirVector = (m_startPos - m_endPos).normalized;
        float angle = Mathf.Atan2(normalizedDirVector.y, normalizedDirVector.x);
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * angle);

        // Scale
        float width = (m_startPos - m_endPos).magnitude;
        transform.localScale = new Vector3(width, transform.localScale.y , transform.localScale.z);

        // Child sprite renderer scale
        ResizeSprite( m_unlitRenderer, m_initiaUnlitScaleX, m_initialUnlitTiledSizeX, 0);
        ResizeSprite( m_litRenderer, m_initiaLitScaleX, m_initialLitTiledSizeX, 0);
        ResizeSprite( m_glowRenderer, m_initiaGlowScaleX, m_initialGlowTiledSizeX, 1.4f);
    }

}
