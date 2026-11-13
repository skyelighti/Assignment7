using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class HitboxComponent : Component, ICollidable
{
    TransformComponent transformComponent;
    Hitbox hitbox;

    public bool IsTrigger => hitbox.IsTrigger;

    public bool ColliderIsActive => hitbox.ColliderIsActive;

    public Rectangle BoxCollider => hitbox.BoxCollider;

    public Layer layer => owner.layer;
    bool isStatic;

    public Transform transform => transformComponent.transform;
    public event Action<ICollidable> CollisionEnter
    {
        add { hitbox.CollisionEnter += value; }
        remove { hitbox.CollisionEnter -= value; }
    }

    public event Action<ICollidable> CollisionExit
    {
        add { hitbox.CollisionExit += value; }
        remove { hitbox.CollisionExit -= value; }
    }

    public event Action<ICollidable> CollisionStay
    {
        add { hitbox.CollisionStay += value; }
        remove { hitbox.CollisionStay -= value; }
    }

    public HitboxComponent(GameObject o, bool isStatic = false): base(o)
    {
        owner = o;
        var hb = new Hitbox(o);
        hitbox = hb;
        this.isStatic = isStatic;
    }

    public override void Connect()
    {
        transformComponent = owner.GetComponent<TransformComponent>();
        owner.ownerScene.AddCollider(this, isStatic);
    }

    public void OnCollision(ICollidable other)
    {
        hitbox.OnCollision(other);
    }

    public void OnCollisionEnter(ICollidable other)
    {
        hitbox.OnCollisionEnter(other);
    }

    public void OnCollisionExit(ICollidable other)
    {

        hitbox.OnCollisionExit(other);
    }
    public bool SetTrigger(bool state)
    {
        return hitbox.SetTrigger(state);
    }
    public override void SetActive(bool state)
    {
        hitbox.SetActive(state);
        base.SetActive(state);
    }
    public bool SetColliderActive(bool state)
    {
        return hitbox.SetColliderActive(state);
    }
    public void SetBounds(Rectangle bounds)
    {
        hitbox.SetBounds(bounds);
    }
    public override void OnDestroy()
    {
        owner.ownerScene.RemoveCollider(this);
    }
}
