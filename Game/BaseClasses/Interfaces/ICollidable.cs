using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
/// <summary>
/// Icollidable guarentees that anything implemting it will have a hitbox and a couple of functions that allow for like the object to handle what it does next
/// </summary>
public interface ICollidable
{
    bool activeSelf { get; }
    //pooling.... maybe new interface? but too fragmented
    bool IsTrigger { get; }
    bool ColliderIsActive { get; }
    //idk seems useful might remove turn into trigger? 
    Rectangle BoxCollider { get; }
    //implement this in colliable gameobject class
    //public Rectangle BoxCollider
    //    {
    //        get
    //        {
    //            return new Rectangle((int) transform.position.X, (int) transform.position.Y, (int) transform.scale.X, (int) transform.scale.Y);
    //    }
    //}
    public Transform transform { get; }

    /// <summary>
    /// currently in the Collision component, these methods call the corresponding method, which the gameobject that holds this component 
    /// can subscribe to, and then dictates its own collision logic.
    /// i like this the best because it i changed these to actions, i dont the collision manager to be able to trigger the action on them directly so this 
    /// wraps the method
    /// </summary>
    /// <param name="other"></param>
    void OnCollision(ICollidable other);
    void OnCollisionEnter(ICollidable other);
    void OnCollisionExit(ICollidable other);
    //each object can manage it's reaction to collisions
    Layer layer { get; }
    GameObject owner { get; }
}

