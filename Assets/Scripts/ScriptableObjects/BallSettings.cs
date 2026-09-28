using UnityEngine;

[CreateAssetMenu(fileName = "BallSettings", menuName = "Pong/Ball Settings")]
public class BallSettings : ScriptableObject
{
    [Header("Speed")]
    [SerializeField] private float startSpeed = 6f;
    [SerializeField] private float speedIncreasePerHit = 0.5f;
    [SerializeField] private float maxSpeed = 14f;

    [Header("Bounce")]
    [Tooltip("Cuanto mas alto, mas inclinada sale la pelota al pegar en la punta de la paleta.")]
    [SerializeField] private float maxBounceAngle = 0.8f;

    [Tooltip("Componente vertical minima tras chocar una pared, evita rebotes infinitos casi horizontales.")]
    [SerializeField] private float minVerticalComponent = 0.15f;

    public float StartSpeed => startSpeed;
    public float SpeedIncreasePerHit => speedIncreasePerHit;
    public float MaxSpeed => maxSpeed;
    public float MaxBounceAngle => maxBounceAngle;
    public float MinVerticalComponent => minVerticalComponent;
}
