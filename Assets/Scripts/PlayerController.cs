using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int _score;

    private void OnEnable()
    {
        DotBehaviour.OnDotEaten += IncreaseScore;
        GhostBehaviour.OnGhostEaten += IncreaseScore;
    }

    private void OnDisable()
    {
        DotBehaviour.OnDotEaten -= IncreaseScore;
        GhostBehaviour.OnGhostEaten -= IncreaseScore;
    }

    private void IncreaseScore(int amount) => _score += amount;
}