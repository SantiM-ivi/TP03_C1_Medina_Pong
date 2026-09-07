using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    [Header("Rotation Settings")]
    [SerializeField] private KeyCode rotateLeftKey = KeyCode.Q;
    [SerializeField] private KeyCode rotateRightKey = KeyCode.E;
    [SerializeField] private float rotationAmount = 10f;

    [Header("Color Settings")]
    [SerializeField] private KeyCode randomColorKey = KeyCode.R;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MovementChange();
        RotationChange();
        ColorChange();
    }

    private void MovementChange()
    {
        Vector2 direction = Vector2.zero;

        if (Input.GetKey(moveUpKey)) direction.y += 1f;
        if (Input.GetKey(moveDownKey)) direction.y -= 1f;
        if (Input.GetKey(moveLeftKey)) direction.x -= 1f;
        if (Input.GetKey(moveRightKey)) direction.x += 1f;

        transform.Translate(direction.normalized * moveSpeed * Time.deltaTime, Space.World);
    }

    private void RotationChange()
    {
        if (Input.GetKeyDown(rotateLeftKey))
        {
            transform.Rotate(Vector3.forward * rotationAmount);
        }

        if (Input.GetKeyDown(rotateRightKey))
        {
            transform.Rotate(Vector3.forward * -rotationAmount);
        }
    }

    private void ColorChange()
    {
        if (Input.GetKeyUp(randomColorKey))
        {
            spriteRenderer.color = new Color(Random.value, Random.value, Random.value);
        }
    }
}