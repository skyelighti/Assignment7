using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Tiles;

public class TileCollider : ICollidable, IObject
{
    public TileInfo TileInfo { get; private set; }

    public Rectangle BoxCollider => CalculateBounds();

    public Transform transform { get; }

    public bool activeSelf { get; private set; } = true;

    public bool ColliderIsActive { get { return TileInfo.IsSolid || TileInfo.IsTrigger; } }

    public bool IsTrigger { get { return TileInfo.IsTrigger; } }

    public Layer layer
    {
        get { return TileInfo.layer; }
    }

    public GameObject owner => null;

    public TileCollider(TileInfo tileInfo, Transform transform)
    {
        TileInfo = tileInfo;
        this.transform = transform;
    }

    public void OnCollision(ICollidable other)
    {
    }
    public void OnCollisionEnter(ICollidable other) { }
    public void OnCollisionExit(ICollidable other) { }

    private Rectangle CalculateBounds()
    {
        Rectangle localBounds;
        if (TileInfo.CollisionBounds != null)
        {
            localBounds = (Rectangle)TileInfo.CollisionBounds;
        }
        else
        {
            localBounds = new Rectangle(0, 0, TileInfo.Region.Width, TileInfo.Region.Height);
        }
        int worldX = (int)(transform.position.X + localBounds.X * transform.scale.X);
        int worldY = (int)(transform.position.Y + localBounds.Y * transform.scale.Y);
        int worldScaleX = (int)(transform.scale.X * localBounds.Width);
        int worldScaleY = (int)(transform.scale.Y * localBounds.Height);
        return new Rectangle(worldX, worldY, worldScaleX, worldScaleY);
    }
    public void SetActive(bool active)
    {
        activeSelf = active;
    }
    public void OnDestroy() { 
    
    }
}
