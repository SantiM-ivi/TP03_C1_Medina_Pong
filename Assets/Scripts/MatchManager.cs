using System.Collections;
using UnityEngine;
using TMPro;

/// <summary>
/// Lado de la cancha. Se usa para identificar arcos y jugadores.
/// </summary>
public enum CourtSide
{
    Left,
    Right
}

/// <summary>
/// Lleva el puntaje de la partida y reinicia la pelota despues de cada gol.
/// </summary>
public class MatchManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BallMovement ball;

    [Header("Score UI")]
    [SerializeField] private TMP_Text leftScoreText;
    [SerializeField] private TMP_Text rightScoreText;

    [Header("Match Settings")]
    [Tooltip("Segundos de pausa entre el gol y el relanzamiento de la pelota.")]
    [SerializeField] private float respawnDelay = 1f;

    private int leftScore;
    private int rightScore;
    private Coroutine respawnRoutine;

    private void Start()
    {
        UpdateScoreUI();
    }

    /// <summary>
    /// Llamado por GoalTrigger. El punto es para el jugador del lado opuesto al arco.
    /// </summary>
    public void RegisterGoal(CourtSide goalSide)
    {
        // Evita contar dos veces si la pelota toca ambos colliders del arco en el mismo frame.
        if (respawnRoutine != null)
        {
            return;
        }

        if (goalSide == CourtSide.Left)
        {
            rightScore++;
        }
        else
        {
            leftScore++;
        }

        UpdateScoreUI();

        respawnRoutine = StartCoroutine(RespawnBall(goalSide));
    }

    /// <summary>
    /// Devuelve la pelota al centro y la relanza hacia el jugador que recibio el gol.
    /// </summary>
    private IEnumerator RespawnBall(CourtSide concededSide)
    {
        ball.Stop();

        // WaitForSeconds respeta Time.timeScale, asi que la espera tambien
        // se congela correctamente si el jugador abre el menu de pausa.
        yield return new WaitForSeconds(respawnDelay);

        float horizontalDirection = concededSide == CourtSide.Left ? -1f : 1f;
        float verticalDirection = Random.Range(-0.5f, 0.5f);

        ball.ResetBall(new Vector2(horizontalDirection, verticalDirection));

        respawnRoutine = null;
    }

    private void UpdateScoreUI()
    {
        leftScoreText.text = leftScore.ToString();
        rightScoreText.text = rightScore.ToString();
    }
}
