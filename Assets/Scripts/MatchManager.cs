using System.Collections;
using UnityEngine;
using TMPro;


public enum CourtSide
{
    Left,
    Right
}


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


    public void RegisterGoal(CourtSide goalSide)
    {

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


    private IEnumerator RespawnBall(CourtSide concededSide)
    {
        ball.Stop();

    
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
