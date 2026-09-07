using UnityEngine;

public class PaddleMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;

    [Header("Court Limits")]
    [SerializeField] private float minY = -4f;
    [SerializeField] private float maxY = 4f;

    public float MoveSpeed
    {
        get => moveSpeed;
        set => moveSpeed = value;
    }

    private void Update()
    {
        MovementChange();
    }

    private void MovementChange()
    {
        float direction = 0f;

        if (Input.GetKey(moveUpKey)) direction += 1f;
        if (Input.GetKey(moveDownKey)) direction -= 1f;

        Vector3 newPosition = transform.position + Vector3.up * direction * moveSpeed * Time.deltaTime;
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        transform.position = newPosition;
    }
}
