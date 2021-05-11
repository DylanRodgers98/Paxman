using UnityEngine;

public class PlayerMovementBehaviour : MovementBehaviour
{
    protected void Start()
    {
        SetDirection(Vector2.left);
    }

    public override void SetDirection(Vector2 movementDirection)
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
        if (Time.timeScale > 0 && LastKnownPosition == (Vector2) PlayerTransform.position)
        {
            base.SetDirection(Vector2.zero);
        }
        base.Update();
    }
}