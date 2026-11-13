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

    public Vector2 lastDir { get; private set; } = Directions.Down;
    public bool left { get; protected set; } = false;
    public bool right { get; protected set; } = false;
    public bool up { get; protected set; } = false;
    public bool down { get; protected set; } = false;
    public bool isMoving { get; protected set; } = false;
    public float speed = 0;
    protected bool onlyCollideWithTiles = false;

    public ICollidable col; 

    //there has to be a better way of doing this, i want this to be reusable: should i make this abstract and make playercomponent inherit. 
    public MovementComponent(float speed, GameObject o) : base(o)
    {
        this.speed = speed;
        //im thinking of keeping this reusable between enemies and the player
        // enemies should pick a direction to move in, and then move until they hit a wall? chose a new direction, remove last direction from pool, unless there is no other way?
        //A* pathfinding? 
        
    }

    public override void Connect()
    {
        col = owner.GetComponent<ICollidable>();
    }
    public void Update(GameTime gameTime)
    {
        if (!left && !right && !up && !down) {
            //nothing is pressed
            isMoving = false;
            return;
        }
        else
        {
            isMoving = true;
        }
        int x = 0;
        int y = 0;
        if (left) {
            x = -1;
        }
        if (right) {
            x += 1;
        }
        if (up) {
            y = -1;
        }
        if (down) {
            y += 1;
        }
        if (x != 0 && y != 0) {
            //prevent diagonal movement
            x = 0;
            //upwards movement always wins in zelda. 
        }
        lastDir = new Vector2(x, y);
        CollisionManager.Instance.Move(lastDir * speed * (float)gameTime.ElapsedGameTime.TotalSeconds, col, onlyCollideWithTiles);
        OnPositionMoved?.Invoke(owner.transform.position);
    }
    public void SetFacing(Vector2 direction)
    {
        lastDir = direction;
    }
}
