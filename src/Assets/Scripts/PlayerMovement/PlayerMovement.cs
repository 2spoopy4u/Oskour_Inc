using System;
using UnityEngine;

public abstract class PlayerMovement
{
    public Rigidbody2D myRigidbody;
    public Transform transform;
    protected bool _isGrounded;

    public PlayerMovement(Rigidbody2D myRigidbody, Transform transform)
    {
        this.myRigidbody = myRigidbody;
        this.transform = transform;
    }

    public abstract void CallMove();

    public abstract void Move();

    public virtual bool CanMove()
    {
        return true;
    }

    public void SetGrounded(bool isGrounded)
    {
        _isGrounded = isGrounded;
    }

    public void SwitchGravity()
    {
        myRigidbody.gravityScale *= -1;
        FlipSprite();
    }

    public void FlipSprite()
    {
        Vector3 scale = transform.localScale;

        scale.y = Mathf.Abs(scale.y) * (myRigidbody.gravityScale < 0 ? -1 : 1);

        transform.localScale = scale;
    }

    public void SetGravity(GravityDirection gravityDirection)
    {
        float absoluteGravityScale = Mathf.Abs(myRigidbody.gravityScale);
        myRigidbody.gravityScale = absoluteGravityScale * (int)gravityDirection;
        FlipSprite();
    }

    public float GetGravityDirection()
    {
        return Mathf.Sign(myRigidbody.gravityScale);
    }
}