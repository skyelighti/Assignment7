using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameBase;

public class Hitbox : ICollidable, IObject
{

    public bool IsTrigger { get; private set; }

    public bool ColliderIsActive { get; private set; } = true;

    public Rectangle BoxCollider
    {
        get
        {
            Rectangle bounds = customBounds ?? new Rectangle(0, 0,(int)transform.scale.X, (int)transform.scale.Y);

            return new Rectangle((int)transform.position.X + bounds.X,(int)transform.position.Y + bounds.Y,bounds.Width,bounds.Height);
        }
    }

    Rectangle? customBounds;

    public Transform transform { get; private set; }
    public event Action<ICollidable> CollisionEnter;
    public event Action<ICollidable> CollisionExit;
    public event Action<ICollidable> CollisionStay;
    public GameObject owner { get; init; }

    public Layer layer { get; private set; } = Layer.Default;

    public bool activeSelf { get; private set; } = true;

    public Hitbox(Transform t, GameObject o = null)
    {
        transform = t;
        if (o != null) {
            owner = o;
            layer = o.layer;
        }
       
    }
    public Hitbox(GameObject o) {
        owner = o;
        layer = o.layer;
        transform = o.transform;
    }
    public void OnCollision(ICollidable other)
    {
        CollisionStay?.Invoke(other);
    }

    public void OnCollisionEnter(ICollidable other)
    {

        CollisionEnter?.Invoke(other);
    }

    public void OnCollisionExit(ICollidable other)
    {
        CollisionExit?.Invoke(other);
    }

    public bool SetTrigger(bool state) {
        IsTrigger = state;
        return IsTrigger;
    }
    public void SetActive(bool state)
    {
        activeSelf = state;
    }
    public bool SetColliderActive(bool state)
    {
        ColliderIsActive = state;
        return ColliderIsActive;
    }
    public void SetLayer(Layer layer) {
        this.layer = layer;
    }
    public void UpdateTransform(Transform t)
    {
        transform = t;
    }

    public void OnDestroy()
    {
        //nothing to cleanup
    }
    public void SetBounds(Rectangle bounds)
    {
        customBounds = bounds;
    }
}
