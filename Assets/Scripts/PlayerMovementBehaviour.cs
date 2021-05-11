using UnityEngine;

public class PlayerMovementBehaviour : MovementBehaviour
{
    protected override void OnEnable()
    {
        base.OnEnable();
        LevelManager.OnLevelStart += SetStartDirection;
        LevelManager.OnLevelReset += SetStartDirection;
    }

    protected override void OnDisable()
    {
        base.OnEnable();
        LevelManager.OnLevelStart -= SetStartDirection;
        LevelManager.OnLevelReset -= SetStartDirection;
    }

    private void SetStartDirection() => SetDirectionInternal(Vector2.left);

    public override void SetDirection(Vector2 movementDirection)
    {
        if (IsMovementEnabled)
        {
            SetDirectionInternal(movementDirection);
        }
    }

    private void SetDirectionInternal(Vector2 movementDirection)
    {
        base.SetDirection(movementDirection);
        SetScale();
        SetRotation();
    }

    private void SetScale()
    {
        transform.localScale = Direction == Vector2.right
            ? new Vector3(-1, 1, 1) // flip sprite across x axis if facing right direction
            : Vector3.one; // keep sprite scale normal for all other directions
    }

    private void SetRotation()
    {
        if (Direction == Vector2.left ||
            Direction == Vector2.right ||
            Direction == Vector2.zero)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (Direction == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (Direction == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }

    private new void Update()
    {
        if (IsMovementEnabled && (Vector2) PlayerTransform.position == LastKnownPosition)
        {
            base.SetDirection(Vector2.zero);
        }
        base.Update();
    }
}