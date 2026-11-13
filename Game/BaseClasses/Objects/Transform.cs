using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Transform
{
    public Vector2 position;
    public Vector2 scale;
    public float rotation;
    public Vector2 DefaultSpriteSize = new Vector2(64, 64);
    public Transform(Vector2 p, float r, Vector2 s)
    {
        position = p;
        rotation = r;
        scale = s;
    }
    public Transform()
    {
        position = Vector2.Zero;
        rotation = 0f;
        scale = DefaultSpriteSize;
        //defaults
    }
    public Transform(Vector2 p, Vector2 s)
    {
        position = p;
        rotation = 0f;
        scale = s;
    }
    public Transform(Vector2 p, float r)
    {
        position = p;
        rotation = r;
        scale = DefaultSpriteSize;

    }
    public Transform(Vector2 p)
    {
        position = p;
        rotation = 0f;
        scale = DefaultSpriteSize;

    }

}
