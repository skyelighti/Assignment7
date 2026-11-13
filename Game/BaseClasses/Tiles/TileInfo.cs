using GameBase.Assets;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Tiles;

public class TileInfo
{
    public TextureRegion Region { get; init; }
    public bool IsSolid { get; init; }
    public bool IsTrigger { get; init; }
    public Layer layer { get; init; }
    public Rectangle? CollisionBounds { get; init; }
    //if is solid and no bounds, whole area is collider

}
