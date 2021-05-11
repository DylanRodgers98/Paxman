using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementBehaviour))]
public abstract class MovementReplay : MonoBehaviour
{
    protected HistoricalDirection CurrentHistoricalDirection;
    protected Vector2 CurrentDirection;
    
    public Queue<HistoricalDirection> Directions { protected get; set; }
    protected MovementBehaviour MovementBehaviour { get; private set; }
    
    protected virtual void Start()
    {
        MovementBehaviour = GetComponent<MovementBehaviour>();
    }
}
