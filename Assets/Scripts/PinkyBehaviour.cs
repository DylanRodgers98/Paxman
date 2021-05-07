using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkyBehaviour : GhostBehaviour
{
    protected override void SetChaseDirection()
    {
        SetRandomMovementDirection();
    }
}
