using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BallMovement : MonoBehaviour
{
    private const string PaddleTag = "Paddle";
    private const string WallTag = "Wall";

    [Header("Movement Settings")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private Vector2 startDirection = new Vector2(1f, 1f);

    [Header("Speed Ramp")]
    [SerializeField] private float speedIncreasePerHit = 0.5f;
    [SerializeField] private float maxSpeed = 14f;

    [Header("Bounce")]
    [Tooltip("Cuanto mas alto, mas inclinada sale la pelota al pegar en la punta de la paleta.")]
    [SerializeField] private float maxBounceAngle = 0.8f;

    [Tooltip("Componente vertical minima tras chocar una pared, evita rebotes infinitos casi horizontales.")]
    [SerializeField] private float minVerticalComponent = 0.15f;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private float startSpeed;
    private Vector2 lastDirection;
    private bool isStopped;
    private Vector2 velocityBeforeStep;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;

        startSpeed = speed;
        startPosition = transform.position;
    }

    private void Start()
    {
        Launch(startDirection);
    }

    public void Launch(Vector2 direction)
    {
        ResetSpeed();
        SetVelocity(direction);
    }


    public void ResetBall(Vector2 direction)
    {
        transform.position = startPosition;
        Launch(direction);
    }

    public void ResetSpeed()
    {
        speed = startSpeed;
    }


    public void Stop()
    {
        isStopped = true;
        rb.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        if (isStopped) return;

        velocityBeforeStep = rb.linearVelocity;

        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
        else
        {
            rb.linearVelocity = lastDirection * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log($"[BallMovement] Colision con '{collision.gameObject.name}' tag='{collision.gameObject.tag}'");

        if (collision.gameObject.CompareTag(PaddleTag))
        {
            BounceOffPaddle(collision);
        }
        else if (collision.gameObject.CompareTag(WallTag))
        {
            BounceOffWall(collision);
        }
    }


    private void BounceOffPaddle(Collision2D collision)
    {
        speed = Mathf.Min(speed + speedIncreasePerHit, maxSpeed);


        float paddleHalfHeight = collision.collider.bounds.extents.y;
        float verticalOffset = (transform.position.y - collision.transform.position.y) / paddleHalfHeight;
        verticalOffset = Mathf.Clamp(verticalOffset, -1f, 1f);


        float horizontalDirection = Mathf.Sign(transform.position.x - collision.transform.position.x);

        Vector2 bounceDir = new Vector2(horizontalDirection, verticalOffset * maxBounceAngle);

        SetVelocity(bounceDir);
    }


    private void BounceOffWall(Collision2D collision)
    {
        speed = Mathf.Min(speed + speedIncreasePerHit, maxSpeed);

        Vector2 normal = collision.GetContact(0).normal;
        Vector2 reflected = Vector2.Reflect(velocityBeforeStep, normal).normalized;

        if (Mathf.Abs(reflected.y) < minVerticalComponent)
        {
            reflected.y = minVerticalComponent * Mathf.Sign(reflected.y == 0f ? 1f : reflected.y);
        }

        SetVelocity(reflected);
    }


    private void SetVelocity(Vector2 direction)
    {
        direction = direction.normalized;
        if (direction.sqrMagnitude < 0.0001f)
        {
            direction = lastDirection.sqrMagnitude > 0.0001f ? lastDirection : Vector2.right;
        }

        lastDirection = direction;
        isStopped = false;

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction * speed * rb.mass, ForceMode2D.Impulse);
    }
}