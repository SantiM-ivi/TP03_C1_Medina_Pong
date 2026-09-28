using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Pong/Game Settings")]
public class GameSettings : ScriptableObject
{
    [Header("Match Rules")]
    [Tooltip("Puntos necesarios para ganar. 3 = mejor de 5.")]
    [SerializeField] private int pointsToWin = 3;

    [Tooltip("Segundos maximos para convertir un gol antes de que se le haga un gol al jugador que tiene la pelota de su lado.")]
    [SerializeField] private float timeToScore = 20f;

    [Tooltip("Segundos de pausa entre el gol y el relanzamiento de la pelota.")]
    [SerializeField] private float respawnDelay = 1f;

    public int PointsToWin => pointsToWin;
    public float TimeToScore => timeToScore;
    public float RespawnDelay => respawnDelay;
}
