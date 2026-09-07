using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PaddleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveForce = 40f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;

    [Header("Court Limits")]
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    private Rigidbody2D rb;
    private float inputDirection;

    // SettingsPanel.cs sigue usando esta propiedad para el slider de velocidad.
    // Ahora controla el tope de velocidad, no la fuerza directamente.
    public float MoveSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // El input se lee en Update (más responsive) y se aplica en FixedUpdate (física).
        inputDirection = 0f;
        if (Input.GetKey(moveUpKey)) inputDirection += 1f;
        if (Input.GetKey(moveDownKey)) inputDirection -= 1f;
    }

    private void FixedUpdate()
    {
        rb.AddForce(Vector2.up * inputDirection * moveForce);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        ClampToCourt();
    }

    private void ClampToCourt()
    {
        Vector2 position = rb.position;

        if (position.y > maxY)
        {
            position.y = maxY;
            rb.position = position;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
        else if (position.y < minY)
        {
            position.y = minY;
            rb.position = position;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }
}