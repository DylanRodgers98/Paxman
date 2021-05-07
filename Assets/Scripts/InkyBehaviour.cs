using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkyBehaviour : GhostBehaviour
{
    protected override void SetChaseDirection()
    {
        SetRandomMovementDirection();
    }
}
