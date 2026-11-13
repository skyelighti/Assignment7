using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class Bullet : Component, IUpdatable
{
    public Vector2 dir;
    public float speed = 50f;
    GameObject shooter;
    public int dmgAmt = 1;
    public float lifetime = 4f;
    float timer = 0f;
    HitboxComponent hitbox;
    Rectangle roomBounds;
    ICollidable collidable;
    public Bullet(GameObject owner) : base(owner)
    {

    }
    public override void Connect()
    {
        hitbox = owner.GetComponent<HitboxComponent>();
        hitbox.CollisionEnter += OnCollisionEnter;
        hitbox.SetTrigger(true);
        collidable = owner.GetComponent<ICollidable>();
    }
    public void Launch(Vector2 direction, GameObject shooter, Rectangle bounds) {
        dir = direction;
        speed = Random.Shared.Next(40, 61);
        this.shooter = shooter;
        roomBounds = bounds;
        owner.transform.position = shooter.transform.position;
        timer = 0f;
        owner.SetActive(true);
    }
    public void Update(GameTime gameTime)
    {
        if (!activeSelf) { return; }

        CollisionManager.Instance.Move(dir * speed * (float)gameTime.ElapsedGameTime.TotalSeconds, collidable);
        if (!roomBounds.Contains(hitbox.BoxCollider)) {
            owner.SetActive(false);
            return;
        }
        if (timer <= lifetime) {
            timer += (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        if(timer >= lifetime)
        {
            owner.SetActive(false);
        }
        
        
    }
    void OnCollisionEnter(ICollidable col)
    {
        if (!activeSelf || col.owner == shooter) { return;}
        if (col.layer == Layer.Player)
        {
            IDamagable damageable = col.owner.GetComponent<IDamagable>();
            if (damageable != null)
            {
                damageable.takeDamage(dmgAmt);
            }
        }
        owner.SetActive(false);
    }
    public override void OnDestroy()
    {
        hitbox.CollisionEnter -= OnCollisionEnter;
    }
}
