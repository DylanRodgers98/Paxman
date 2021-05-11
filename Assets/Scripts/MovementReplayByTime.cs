using UnityEngine;

public class MovementReplayByTime : MovementReplay
{
    private float _currentTime;

    protected override void Start()
    {
        base.Start();
        _currentTime = 0.0f;
        GetNextHistoricalDirection();
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        while (CurrentHistoricalDirection?.Time <= _currentTime)
        {
            CurrentDirection = new Vector2(CurrentHistoricalDirection.X, CurrentHistoricalDirection.Y);
            MovementBehaviour.SetDirection(CurrentDirection);
            GetNextHistoricalDirection();
        }
    }

    private void GetNextHistoricalDirection()
    {
        CurrentHistoricalDirection = Directions.Count > 0 ? Directions.Dequeue() : null;
    }
}
