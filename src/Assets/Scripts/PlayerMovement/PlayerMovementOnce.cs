using UnityEngine;

public abstract class PlayerMovementOnce : PlayerMovement
{
    public PlayerMovementOnce(Rigidbody2D myRigidbody, Transform transform) : base(myRigidbody, transform)
    {
    }

    public override void CallMove()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.Move();
        } else if (Input.GetMouseButtonDown(0))
        {
            this.Move();
        }
    }
}