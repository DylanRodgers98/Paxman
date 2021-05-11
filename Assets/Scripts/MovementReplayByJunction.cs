using UnityEngine;

public class MovementReplayByJunction : MovementReplay
{
    private bool _isDirectionChosen;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Junction") && !_isDirectionChosen)
        {
            CurrentHistoricalDirection = Directions.Dequeue();
            CurrentDirection = new Vector2(CurrentHistoricalDirection.X, CurrentHistoricalDirection.Y);
            MovementBehaviour.SetDirection(CurrentDirection);
            _isDirectionChosen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Junction"))
        {
            _isDirectionChosen = false;
        }
    }
}
