using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PaddleAppearance))]
public class PaddleMovement : MonoBehaviour
{
    private const float LimitTolerance = 0.01f;
    private const string WallTag = "Wall";

    [Header("Settings")]
    [SerializeField] private PaddleSettings settings;

    [Header("Controls")]
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;

    [Header("Court Limits")]
    [Tooltip("Lado de la cancha de este jugador. Define cual limite en X es el borde de pantalla (el arco) y cual es el medio.")]
    [SerializeField] private CourtSide courtSide = CourtSide.Left;
    [SerializeField] private float minX = -11f;
    [SerializeField] private float maxX = -0.5f;
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    private Rigidbody2D rb;
    private PaddleAppearance appearance;
    private Vector2 inputDirection;
    private float maxSpeed;
    private int wallContacts;
    private bool isAtCourtLimit;

    public float MoveSpeed
    {
        get => maxSpeed;
        set => maxSpeed = value;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        appearance = GetComponent<PaddleAppearance>();

        rb.gravityScale = 0f;
       
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        maxSpeed = settings.MaxSpeed;
    }

    private void Update()
    {
        inputDirection = Vector2.zero;

        if (Input.GetKey(moveUpKey)) inputDirection.y += 1f;
        if (Input.GetKey(moveDownKey)) inputDirection.y -= 1f;
        if (Input.GetKey(moveRightKey)) inputDirection.x += 1f;
        if (Input.GetKey(moveLeftKey)) inputDirection.x -= 1f;
    }

    private void FixedUpdate()
    {
        rb.AddForce(inputDirection * settings.MoveForce);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        ClampToCourt();
    }

    private void ClampToCourt()
    {
        Vector2 position = rb.position;
        Vector2 velocity = rb.linearVelocity;

        if (position.y > maxY) { position.y = maxY; velocity.y = 0f; }
        else if (position.y < minY) { position.y = minY; velocity.y = 0f; }

        if (position.x > maxX) { position.x = maxX; velocity.x = 0f; }
        else if (position.x < minX) { position.x = minX; velocity.x = 0f; }

        rb.position = position;
        rb.linearVelocity = velocity;

        isAtCourtLimit = IsTouchingScreenLimit(position);
        RefreshLimitColor();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(WallTag)) return;

        wallContacts++;
        RefreshLimitColor();
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(WallTag)) return;

        wallContacts = Mathf.Max(0, wallContacts - 1);
        RefreshLimitColor();
    }

   
    private void RefreshLimitColor()
    {
        appearance.SetTouchingLimit(wallContacts > 0 || isAtCourtLimit);
    }

    
    private bool IsTouchingScreenLimit(Vector2 position)
    {
        bool touchesVertical = position.y >= maxY - LimitTolerance || position.y <= minY + LimitTolerance;

        bool touchesGoalSide = courtSide == CourtSide.Left
            ? position.x <= minX + LimitTolerance
            : position.x >= maxX - LimitTolerance;

        return touchesVertical || touchesGoalSide;
    }
}