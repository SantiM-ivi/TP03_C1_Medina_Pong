using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float speed = 6f;
    [SerializeField] private Vector2 startDirection = new Vector2(1f, 1f);

    private Vector2 direction;

    private void Start()
    {
        direction = startDirection.normalized;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction.y = -direction.y;
            Debug.Log(collision.gameObject.name + " - " + collision.gameObject.tag);
        }

        if (collision.gameObject.CompareTag("Paddle"))
        {
            direction.x = -direction.x;
            Debug.Log(collision.gameObject.name + " - " + collision.gameObject.tag);
        }
    }
}
