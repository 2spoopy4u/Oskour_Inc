using UnityEngine;

public class BallMovement : PlayerMovementOnce
{
    public int jumpForce = 7;
    private int gravityDirection = 1;

    public BallMovement(Rigidbody2D myRigidbody, Transform transform) : base(myRigidbody, transform) {
    }

    public override void Move() {
        if (CanMove())
        {
            this.gravityDirection *= -1;
            this.myRigidbody.gravityScale *= -1;
            this.myRigidbody.linearVelocity = Vector2.down * this.jumpForce * GetGravityDirection();

            _isGrounded = false; // force le grounded à false
            FlipSprite();
        }
    }

    public override bool CanMove()
    {
        return _isGrounded;
    }
}