using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlinkyBehaviour : GhostBehaviour
{
    protected override void SetChaseDirection()
    {
        SetRandomMovementDirection();
    }
}
