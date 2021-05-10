using UnityEngine;

public class PlayerMovementBehaviour : MovementBehaviour
{
    public void Start()
    {
        SetMovementDirection(Vector2.left);
    }

    protected override void SetMovementDirection(Vector2 movementDirection)
    {
        base.SetMovementDirection(movementDirection);
        SetScale();
        SetRotation();
    }

    private void SetScale()
    {
        transform.localScale = MovementDirection == Vector2.right
            ? new Vector3(-1, 1, 1) // flip sprite across x axis if facing right direction
            : Vector3.one; // keep sprite scale normal for all other directions
    }

    private void SetRotation()
    {
        if (MovementDirection == Vector2.left ||
            MovementDirection == Vector2.right ||
            MovementDirection == Vector2.zero)
        {
            transform.rotation = Quaternion.identity;
        }
        else if (MovementDirection == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (MovementDirection == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
    }
}