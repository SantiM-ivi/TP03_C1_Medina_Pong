using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PaddleAppearance : MonoBehaviour
{
    [Header("Height")]
    [Tooltip("Multiplicador sobre el alto original del sprite. 1 = tamaño original.")]
    [SerializeField] private float height = 1f;

    private SpriteRenderer spriteRenderer;
    private Vector3 baseScale;

    public float Height
    {
        get => height;
        set => ApplyHeight(value);
    }

    public Color PaddleColor
    {
        get => spriteRenderer.color;
        set => spriteRenderer.color = value;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseScale = transform.localScale;
        ApplyHeight(height);
    }

    private void ApplyHeight(float newHeight)
    {
        height = newHeight;

        // Solo se escala el eje Y: el ancho de la paleta no debe cambiar.
        // El BoxCollider2D escala solo junto con el transform, no hace
        // falta tocar su tamaño a mano.
        Vector3 scale = transform.localScale;
        scale.y = baseScale.y * newHeight;
        transform.localScale = scale;
    }
}
