using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class BulletManager
{
    //only gamescene needs one, cuz only game scene has enemies
    Scene owner;
    string spriteName;
    int size;
    ObjectPool<GameObject> bulletPool;
    public BulletManager(Scene owner, string spriteName, int size = 8) {
        this.owner = owner;
        this.spriteName = spriteName;
        this.size = size;
       bulletPool = new ObjectPool<GameObject>(20, CreateBullet);
    }

    GameObject CreateBullet() {
        GameObject bullet = Prefabs.CreateBullet(owner, Services.Get<SpriteLibrary>().CreateSprite(spriteName), Vector2.Zero);

        bullet.UpdateSize(size, size);
        bullet.ActiveOnSceneEnter = false;
        bullet.SetActive(false);

        owner.AddObject(bullet);
        return bullet;
    }

    public void Fire(Vector2 direction,GameObject shooter, Rectangle roomBounds)
    {
        GameObject bullet = bulletPool.GetPooledObj();

        if (bullet == null) { return; }

        bullet.GetComponent<Bullet>().Launch( direction, shooter, roomBounds);
    }
    public void Clear()
    {
        bulletPool.ResetPool();
    }
}
