using Microsoft.Xna.Framework;
using System;

namespace GameBase;

public class EnemyComponent : Component
{
    HitboxComponent hitbox;

    public EnemyComponent(GameObject o) : base(o)
    {
    }

    public override void Connect()
    {
        hitbox = owner.GetComponent<HitboxComponent>();
        hitbox.CollisionEnter += OnCollisionEnter;
    }

    private void OnCollisionEnter(ICollidable other)
    {
        if (!activeSelf || other.owner == null || !other.owner.GetTag(Tag.Player))
        {
            return;
        }

        PlayerMovementComponent player = other.owner.GetComponent<PlayerMovementComponent>();
        if (!player.activeSelf) { return; }

        if (player.velocity.Y > 0 && player.previousBounds.Bottom <= hitbox.BoxCollider.Top)
        {
            other.transform.position.Y = hitbox.BoxCollider.Top - other.BoxCollider.Height;
            player.Bounce(240f);
            owner.ownerScene.RemoveObject(owner);
        }
        else
        {
            player.Die();
        }
    }

    public override void OnDestroy()
    {
        hitbox.CollisionEnter -= OnCollisionEnter;
    }
}
