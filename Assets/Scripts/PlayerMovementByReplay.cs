using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementByReplay : PlayerMovementBehaviour
{
    private Queue<Tuple<float, Vector2>> actions;
    private bool _hasNextAction;
    private float _actionTime;
    private Vector2 _actionDirection;
    
    private new void Start()
    {
        GetNextAction();
        base.Start();
    }

    private new void Update()
    {
        if (_hasNextAction && _actionTime >= Time.timeSinceLevelLoad)
        {
            SetDirection(_actionDirection);
            GetNextAction();
        }
    }

    private void GetNextAction()
    {
        if (actions.Count > 0)
        {
            (_actionTime, _actionDirection) = actions.Dequeue();
            _hasNextAction = true;
        }
        else
        {
            _hasNextAction = false;
        }
    }
}
