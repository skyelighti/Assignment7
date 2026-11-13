using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public abstract class MovementComponent : Component, IUpdatable
{
    public event Action<Vector2> OnPositionMoved;

    public Vector2 lastDir { get; private set; } = Directions.Right;
    public bool left { get; protected set; } = false;
    public bool right { get; protected set; } = false;
    public bool isMoving { get; protected set; } = false;
    public bool isGrounded { get; private set; }
    public Vector2 velocity;
    public Rectangle previousBounds { get; private set; }
    public float speed = 0;
    public float acceleration = 900f;
    public float friction = 1200f;
    public float gravity = 1000f;
    public float jumpSpeed = 390f;
    public float maxFallSpeed = 600f;
    protected bool onlyCollideWithTiles = true;

    public ICollidable col; 

    //shared movement and physics
    public MovementComponent(float speed, GameObject o) : base(o)
    {
        this.speed = speed;
    }

    public override void Connect()
    {
        col = owner.GetComponent<ICollidable>();
        previousBounds = col.BoxCollider;
    }
    public virtual void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        previousBounds = col.BoxCollider;
        int x = 0;
        if (left) {
            x = -1;
        }
        if (right) {
            x += 1;
        }
        if (x != 0)
        {
            velocity.X += x * acceleration * dt;
            velocity.X = MathHelper.Clamp(velocity.X, -speed, speed);
            lastDir = new Vector2(x, 0);
        }
        else if (velocity.X > 0)
        {
            velocity.X = Math.Max(0, velocity.X - friction * dt);
        }
        else if (velocity.X < 0)
        {
            velocity.X = Math.Min(0, velocity.X + friction * dt);
        }

        velocity.Y = Math.Min(velocity.Y + gravity * dt, maxFallSpeed);
        Vector2 wantedMove = velocity * dt;
        Vector2 moved = CollisionManager.Instance.Move(wantedMove, col, onlyCollideWithTiles);
        isGrounded = wantedMove.Y > 0 && moved.Y != wantedMove.Y;

        if (moved.X != wantedMove.X)
        {
            velocity.X = 0;
            //hitboxes use whole pixels
            owner.transform.position.X = (int)owner.transform.position.X;
        }
        if (moved.Y != wantedMove.Y)
        {
            velocity.Y = 0;
            owner.transform.position.Y = (int)owner.transform.position.Y;
        }

        isMoving = moved.X != 0;
        OnPositionMoved?.Invoke(owner.transform.position);
    }
    public void Jump()
    {
        if (isGrounded)
        {
            Bounce(jumpSpeed);
        }
    }
    public void Bounce(float force)
    {
        velocity.Y = -force;
        isGrounded = false;
    }
    public void SetFacing(Vector2 direction)
    {
        lastDir = direction;
    }
}
