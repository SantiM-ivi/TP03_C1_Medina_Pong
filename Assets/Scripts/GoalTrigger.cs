using UnityEngine;


[RequireComponent(typeof(Collider2D))]
public class GoalTrigger : MonoBehaviour
{
    private const string BallTag = "Ball";

    [Header("Goal Settings")]
    [Tooltip("Lado de la cancha en el que esta ubicado este arco.")]
    [SerializeField] private CourtSide side = CourtSide.Left;

    [SerializeField] private MatchManager matchManager;

    private void Reset()
    {
    
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(BallTag))
        {
            return;
        }

        matchManager.RegisterGoal(side);
    }
}
