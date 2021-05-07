using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClydeBehaviour : GhostBehaviour
{
    protected override void SetChaseDirection()
    {
        SetRandomMovementDirection();
    }
}
