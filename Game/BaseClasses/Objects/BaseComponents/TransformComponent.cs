using Microsoft.CodeAnalysis;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class TransformComponent: Component
{
    public Transform transform{ get; init; }
    public TransformComponent(Vector2 p, float r, Vector2 s, GameObject o): base(o)
    {
        transform = new Transform(p, r, s);
    }
    public TransformComponent(GameObject o): base(o)
    {
        transform = new Transform();
        //defaults
    }
    public TransformComponent(Vector2 p, Vector2 s, GameObject o): base(o)
    {
        transform = new Transform(p, s);
    }
    public TransformComponent(Vector2 p, float r, GameObject o): base(o)
    {
        transform = new Transform(p, r);

    }
    public TransformComponent(Vector2 p, GameObject o): base(o)
    {
        transform = new Transform(p);
    }

    public override void OnDestroy()
    {
    }

    public override void Connect()
    {
    }
}
