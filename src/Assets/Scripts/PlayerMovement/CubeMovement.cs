using UnityEngine;

public class CubeMovement : PlayerMovementHold
{
    public int jumpForce = 10;

    public CubeMovement(Rigidbody2D myRigidbody, Transform transform) : base(myRigidbody, transform) {
    }

    public override void Move() {
        if (CanMove())
        {
            this.myRigidbody.linearVelocity = Vector2.up * this.GetGravityDirection() * this.jumpForce;
        }
    }

    public override bool CanMove()
    {
        return _isGrounded;
    }
}