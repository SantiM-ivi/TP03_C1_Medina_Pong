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
    [SerializeField] private float speedIncreasePerWallBounce = 0.25f;
    [SerializeField] private float maxSpeed = 14f;

    [Header("Bounce")]
    [SerializeField] private float maxBounceAngle = 0.8f;
    [SerializeField] private float minVerticalComponent = 0.15f;
    [SerializeField] private float directionJitterDegrees = 6f;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private float startSpeed;
    private Vector2 velocityBeforeStep;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

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
        rb.linearVelocity = Vector2.zero;
    }

    private void FixedUpdate()
    {
        velocityBeforeStep = rb.linearVelocity;

        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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

        Bounds paddleBounds = collision.collider.bounds;
        float paddleHalfHeight = paddleBounds.extents.y;
        float verticalOffset = (transform.position.y - paddleBounds.center.y) / paddleHalfHeight;
        verticalOffset = Mathf.Clamp(verticalOffset, -1f, 1f);

        float horizontalDirection = Mathf.Sign(transform.position.x - paddleBounds.center.x);

        float verticalDirection = verticalOffset * maxBounceAngle;
        verticalDirection = EnsureMinimumVertical(verticalDirection);

        Vector2 direction = new Vector2(horizontalDirection, verticalDirection);
        direction = ApplyRandomJitter(direction);

        SetVelocity(direction);
    }

    private void BounceOffWall(Collision2D collision)
    {
        Vector2 normal = collision.GetContact(0).normal;
        Vector2 reflected = Vector2.Reflect(velocityBeforeStep, normal).normalized;

        reflected.y = EnsureMinimumVertical(reflected.y);

        speed = Mathf.Min(speed + speedIncreasePerWallBounce, maxSpeed);

        reflected = ApplyRandomJitter(reflected);

        SetVelocity(reflected);
    }

    private float EnsureMinimumVertical(float verticalComponent)
    {
        if (Mathf.Abs(verticalComponent) >= minVerticalComponent)
        {
            return verticalComponent;
        }

        float previousVertical = velocityBeforeStep.y;
        float fallbackSign = previousVertical != 0f
            ? Mathf.Sign(previousVertical)
            : (Random.value < 0.5f ? 1f : -1f);

        return minVerticalComponent * fallbackSign;
    }

    private Vector2 ApplyRandomJitter(Vector2 direction)
    {
        float angle = Random.Range(-directionJitterDegrees, directionJitterDegrees);
        return Quaternion.Euler(0f, 0f, angle) * direction;
    }

    private void SetVelocity(Vector2 direction)
    {
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * speed * rb.mass, ForceMode2D.Impulse);
    }
}