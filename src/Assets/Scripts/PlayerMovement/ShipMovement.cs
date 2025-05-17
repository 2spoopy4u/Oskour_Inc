using UnityEngine;

public class ShipMovement : PlayerMovementHold
{
    public float jumpForce = 55f;

    public ShipMovement(Rigidbody2D myRigidbody, Transform transform) : base(myRigidbody, transform) {
    }

    public override void Move() {
        this.myRigidbody.linearVelocity += this.GetGravityDirection() * this.jumpForce * Time.deltaTime * Vector2.up;
    }

    public override bool CanMove()
    {
        return _isGrounded;
    }
}