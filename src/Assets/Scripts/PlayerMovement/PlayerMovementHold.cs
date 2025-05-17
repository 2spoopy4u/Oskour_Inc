using UnityEngine;

public abstract class PlayerMovementHold : PlayerMovement
{
    public PlayerMovementHold(Rigidbody2D myRigidbody, Transform transform) : base(myRigidbody, transform)
    {
    }

    public override void CallMove()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            this.Move();
        } else if (Input.GetMouseButton(0))
        {
            this.Move();
        }
    }
}