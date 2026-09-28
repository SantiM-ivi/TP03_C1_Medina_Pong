using UnityEngine;

[CreateAssetMenu(fileName = "PaddleSettings", menuName = "Pong/Paddle Settings")]
public class PaddleSettings : ScriptableObject
{
    [Header("Movement")]
    [SerializeField] private float moveForce = 2000f;
    [SerializeField] private float maxSpeed = 50f;

    [Header("Colors")]
    [SerializeField] private Color limitColor = Color.black;

    public float MoveForce => moveForce;
    public float MaxSpeed => maxSpeed;
    public Color LimitColor => limitColor;
}
