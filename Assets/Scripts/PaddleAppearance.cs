using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PaddleAppearance : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private PaddleSettings settings;

    [Header("Height")]
    [Tooltip("Multiplicador sobre el alto original del sprite. 1 = tamaño original.")]
    [SerializeField] private float height = 1f;

    [Header("Random Color")]
    [SerializeField, Range(0f, 1f)] private float minSaturation = 0.6f;
    [SerializeField, Range(0f, 1f)] private float minBrightness = 0.8f;

    private SpriteRenderer spriteRenderer;
    private Vector3 baseScale;
    private Color baseColor;
    private bool isTouchingLimit;

    public float Height
    {
        get => height;
        set => ApplyHeight(value);
    }

    // Color "propio" del paddle. Mientras toca un limite se muestra negro, pero este valor se conserva.
    public Color PaddleColor
    {
        get => baseColor;
        set
        {
            baseColor = value;
            RefreshColor();
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
        baseColor = spriteRenderer.color;
        ApplyHeight(height);
    }

    public void SetRandomColor()
    {
        PaddleColor = Random.ColorHSV(0f, 1f, minSaturation, 1f, minBrightness, 1f);
    }

    public void SetTouchingLimit(bool isTouching)
    {
        if (isTouchingLimit == isTouching) return;

        isTouchingLimit = isTouching;
        RefreshColor();
    }

    private void RefreshColor()
    {
        spriteRenderer.color = isTouchingLimit ? settings.LimitColor : baseColor;
    }

    private void ApplyHeight(float newHeight)
    {
        height = newHeight;

        Vector3 scale = transform.localScale;
        scale.y = baseScale.y * newHeight;
        transform.localScale = scale;
    }
}
