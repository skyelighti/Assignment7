using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class PixelHitbox : Hitbox, IPixelCollidable
{
    public PixelHitbox(GameObject o) : base(o)
    {

    }

    public Sprite sprite => owner.sprite;
}
