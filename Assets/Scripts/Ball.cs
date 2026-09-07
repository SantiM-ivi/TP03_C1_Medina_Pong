using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class BallMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private Vector2 startDirection = new Vector2(1f, 1f);

    [Header("Speed Ramp")]
    [SerializeField] private float speedIncreasePerHit = 0.5f;
    [SerializeField] private float maxSpeed = 14f;

    private Rigidbody2D rb;
    private float startSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        startSpeed = speed;
    }

    private void Start()
    {
        Launch(startDirection);
    }

    public void Launch(Vector2 direction)
    {
        ResetSpeed();
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {

        if (rb.linearVelocity.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * speed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Paddle"))
        {
            speed = Mathf.Min(speed + speedIncreasePerHit, maxSpeed);
        }
    }

    public void ResetSpeed()
    {
        speed = startSpeed;
    }
}