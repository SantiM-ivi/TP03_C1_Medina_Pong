using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public enum CourtSide
{
    Left,
    Right
}

public class MatchManager : MonoBehaviour
{
    private const float CourtCenterX = 0f;

    [Header("References")]
    [SerializeField] private GameSettings settings;
    [SerializeField] private BallMovement ball;

    [Header("Score UI")]
    [SerializeField] private TMP_Text leftScoreText;
    [SerializeField] private TMP_Text rightScoreText;
    [SerializeField] private TMP_Text timerText;

    [Header("End Of Match UI")]
    [SerializeField] private GameObject endMatchPanel;
    [SerializeField] private TMP_Text winnerText;
    [SerializeField] private string mainMenuSceneName = "Menu";

    private int leftScore;
    private int rightScore;
    private float timeRemaining;
    private bool isBallInPlay;
    private bool isMatchOver;
    private Coroutine respawnRoutine;

    private void Start()
    {
        if (endMatchPanel != null) endMatchPanel.SetActive(false);

        StartRound();
        UpdateScoreUI();
    }

    private void Update()
    {
        if (!isBallInPlay || isMatchOver) return;

        timeRemaining -= Time.deltaTime;
        UpdateTimerUI();

        if (timeRemaining <= 0f)
        {
            OnTimeExpired();
        }
    }

    public void RegisterGoal(CourtSide concededSide)
    {
        if (isMatchOver || respawnRoutine != null) return;

        isBallInPlay = false;

        if (concededSide == CourtSide.Left) rightScore++;
        else leftScore++;

        UpdateScoreUI();

        if (HasPlayerWon(out CourtSide winner))
        {
            EndMatch(winner);
            return;
        }

        respawnRoutine = StartCoroutine(RespawnBall(concededSide));
    }

    public void RestartMatch()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }

    // Si se acaba el tiempo, se le hace un gol al jugador que tiene la pelota de su lado.
    private void OnTimeExpired()
    {
        CourtSide ballSide = ball.transform.position.x < CourtCenterX ? CourtSide.Left : CourtSide.Right;
        RegisterGoal(ballSide);
    }

    private bool HasPlayerWon(out CourtSide winner)
    {
        winner = leftScore >= rightScore ? CourtSide.Left : CourtSide.Right;
        return leftScore >= settings.PointsToWin || rightScore >= settings.PointsToWin;
    }

    private void EndMatch(CourtSide winner)
    {
        isMatchOver = true;
        ball.Stop();

        if (winnerText != null)
        {
            winnerText.text = winner == CourtSide.Left ? "Gana el Jugador Izquierdo" : "Gana el Jugador Derecho";
        }

        if (endMatchPanel != null) endMatchPanel.SetActive(true);
    }

    private void StartRound()
    {
        timeRemaining = settings.TimeToScore;
        isBallInPlay = true;
        UpdateTimerUI();
    }

    private IEnumerator RespawnBall(CourtSide concededSide)
    {
        ball.Stop();

        yield return new WaitForSeconds(settings.RespawnDelay);

        float horizontalDirection = concededSide == CourtSide.Left ? -1f : 1f;
        float verticalDirection = Random.Range(-0.5f, 0.5f);

        ball.ResetBall(new Vector2(horizontalDirection, verticalDirection));

        StartRound();
        respawnRoutine = null;
    }

    private void UpdateScoreUI()
    {
        leftScoreText.text = leftScore.ToString();
        rightScoreText.text = rightScore.ToString();
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;

        timerText.text = Mathf.CeilToInt(Mathf.Max(timeRemaining, 0f)).ToString();
    }
}
