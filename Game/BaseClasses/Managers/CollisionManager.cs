using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GameBase;
public class CollisionManager
{
    /// <summary>
    /// handles collisions and movement that should be blocked by collisions
    /// singleton
    /// </summary>
    public static CollisionManager Instance { get; private set; } = new();
    public List<ICollidable> CollidableObjects { get; private set; } = new();
    //this is for objects that actually move
    public List<ICollidable> StaticCollidableObjects { get; private set; } = new();
    //holy moly the lag
    //just have others add on init?
    private HashSet<ICollidable> additionSet = new();
    private HashSet<ICollidable> staticAdditionSet = new();
    private HashSet<ICollidable> removalSet = new();
    float ascaleX;
    float ascaleY;
    float bscaleY;
    float bscaleX;
    HashSet<(ICollidable x, ICollidable y)> prevCollisions = new HashSet<(ICollidable x, ICollidable y)>();
    HashSet<(ICollidable x, ICollidable y)> currCollisions = new HashSet<(ICollidable x, ICollidable y)>();
    public CollisionManager()
    {
        Instance = this;
        //needs a list of everything with Icolliable & list needs to update on spawn of new objects? 
    }
    public void Update(GameTime gameTime)
    {
        //preform basic collision check, the pixel perfect if needed I think
        //i hate this i think i overcomplicated this hella
        //i miss unity :'(

        currCollisions.Clear();
        for (int i = 0; i < CollidableObjects.Count; i++)
        {
            if (!CollidableObjects[i].activeSelf || !CollidableObjects[i].ColliderIsActive) { continue; }
            for (int j = i + 1; j < CollidableObjects.Count; j++)
            {
                if (!CollidableObjects[j].activeSelf || !CollidableObjects[j].ColliderIsActive) { continue; }
                if (CollidableObjects[i].BoxCollider.Intersects(CollidableObjects[j].BoxCollider) && (!removalSet.Contains(CollidableObjects[i]) && !removalSet.Contains(CollidableObjects[j])))
                {
                    // Tile colliders use boxes; sprites can use the original pixel check.
                    bool collision = true;
                    if (CollidableObjects[i] is IPixelCollidable pixelA && CollidableObjects[j] is IPixelCollidable pixelB)
                    {
                        collision = PixelPerfectCollision(pixelA, pixelB);
                    }
                    if (collision)
                    {
                        if (CollidableObjects[i].owner != null && CollidableObjects[i].owner == CollidableObjects[j].owner)
                        {
                            continue;
                        }
                        currCollisions.Add((CollidableObjects[i], CollidableObjects[j]));

                        CollidableObjects[i].OnCollision(CollidableObjects[j]);
                        CollidableObjects[j].OnCollision(CollidableObjects[i]);
                        // Triggers and fixed tile colliders must not be pushed apart.
                        if (!CollidableObjects[i].IsTrigger && !CollidableObjects[j].IsTrigger &&
                            CollidableObjects[i] is IPixelCollidable && CollidableObjects[j] is IPixelCollidable)
                        {
                            SeperateObjects(CollidableObjects[i], CollidableObjects[j]);
                        }

                    }
                    //replace w piixel perfect if there time

                    if (!CollidableObjects[i].activeSelf) { break; }
                    //prevent bullet taking out more than 1
                }
             
            }
            if (StaticCollidableObjects == null) { continue; }
            for (int x = 0; x < StaticCollidableObjects.Count; x++)
            {
                if (!StaticCollidableObjects[x].activeSelf || !StaticCollidableObjects[x].ColliderIsActive) { continue; }
                if (CollidableObjects[i].BoxCollider.Intersects(StaticCollidableObjects[x].BoxCollider) && (!removalSet.Contains(CollidableObjects[i]) && !removalSet.Contains(StaticCollidableObjects[x])))
                {
                    // Tile colliders use boxes; sprites can use the original pixel check.
                    bool collision = true;
                    if (CollidableObjects[i] is IPixelCollidable pixelA && StaticCollidableObjects[x] is IPixelCollidable pixelB)
                    {
                        collision = PixelPerfectCollision(pixelA, pixelB);
                    }
                    if (collision)
                    {
                        if (CollidableObjects[i].owner != null && CollidableObjects[i].owner == StaticCollidableObjects[x].owner)
                        {
                            continue;
                        }
                        currCollisions.Add((CollidableObjects[i], StaticCollidableObjects[x]));

                        CollidableObjects[i].OnCollision(StaticCollidableObjects[x]);
                        StaticCollidableObjects[x].OnCollision(CollidableObjects[i]);
                        // Triggers and fixed tile colliders must not be pushed apart.
                        if (!CollidableObjects[i].IsTrigger && !StaticCollidableObjects[x].IsTrigger &&
                            CollidableObjects[i] is IPixelCollidable && StaticCollidableObjects[x] is IPixelCollidable)
                        {
                            SeperateObjects(CollidableObjects[i], StaticCollidableObjects[x]);
                        }

                    }
                    //replace w piixel perfect if there time

                    if (!CollidableObjects[i].activeSelf) { break; }

                }
            }
        }
        foreach (var pair in currCollisions)
        {
            if (!prevCollisions.Contains(pair))
            {
                //oncollisionenter
                pair.x.OnCollisionEnter(pair.y);
                pair.y.OnCollisionEnter(pair.x);
            }
        }
        foreach (var pair in prevCollisions)
        {
            if (!currCollisions.Contains(pair))
            {
                //oncollisionexit
                pair.x.OnCollisionExit(pair.y);
                pair.y.OnCollisionExit(pair.x);
            }
        }
        var old = prevCollisions;
        prevCollisions = currCollisions;
        currCollisions = old;
        foreach (var c in additionSet) {
            CollidableObjects.Add(c);
        }
        foreach (var c in staticAdditionSet)
        {
            StaticCollidableObjects.Add(c);
        }
        foreach (var c in removalSet)
        {
            CollidableObjects.Remove(c);
             StaticCollidableObjects.Remove(c);
        }
        removalSet.Clear();
        additionSet.Clear();
        staticAdditionSet.Clear();
    }
    /// <summary>
    /// adds a collidable through the component on  gameobject
    /// if the collider is static. ex a tilemap collider, does not get checked for collisions with other static colliders
    /// </summary>
    /// <param name="col"></param>
    /// <param name="isStatic"> add if static</param>
    public void AddCollidable(ICollidable col, bool isStatic = false)
    {
        if (isStatic) {
            staticAdditionSet.Add(col);
            return;
        }
        additionSet.Add(col);
        //CollidableObjects.Add(col);
    }
    /// <summary>
    /// removes the collider from the collider list
    /// </summary>
    /// <param name="col"></param>
    public void RemoveCollider(ICollidable col)
    {
        removalSet.Add(col);
        //CollidableObjects.Remove(col);
    }

    /// <summary>
    /// makes it so two collidbale objects are no longer tocuhing each other
    /// i should add a thing where if the collider is static, only the non static collider moves
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    public void SeperateObjects(ICollidable a, ICollidable b)
    {
        Rectangle boundsA = a.BoxCollider;
        Rectangle boundsB = b.BoxCollider;

        Vector2 centerA = new Vector2(boundsA.X + boundsA.Width * 0.5f, boundsA.Y + boundsA.Height * 0.5f);

        Vector2 centerB = new Vector2(boundsB.X + boundsB.Width * 0.5f, boundsB.Y + boundsB.Height * 0.5f);
        Vector2 sepDir = centerB - centerA;

        if (sepDir != Vector2.Zero)
        {
            sepDir.Normalize();
        }
        Move(-sepDir, a);
        Move(sepDir, b);

    }
    //i think i could simplify by just pruning everything each call, but im worried about needlessly iterating? 
    //maybe implement a stack? specifically for bullets? idk TT
    /// <summary>
    /// equivlant of a raycast, can set the radius and the target from where the raycast occurs
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="gameobject"></param>
    /// <returns></returns>
    public List<ICollidable> CircleCast(int radius, ICollidable gameobject, Layer? layer = null, Tag? tag = null)
    {
        List<ICollidable> overlap = new List<ICollidable>();
        Rectangle bounds = gameobject.BoxCollider;
        Vector2 center = new Vector2(
            bounds.X + bounds.Width / 2f,
            bounds.Y + bounds.Height / 2f
        );
        foreach (ICollidable c in CollidableObjects)
        {
            if (c == gameobject || !c.activeSelf || !c.ColliderIsActive) { continue; }
            if (gameobject.owner != null && gameobject.owner == c.owner)
            {
                continue;
            }
            if (layer != null && c.layer == (Layer)layer) { continue; }
            if (tag != null && !c.owner.GetTag((Tag)tag)) { continue; }

            //gameobject center, or else everything else is at the top le

            int closeX = (int)Math.Clamp(center.X, c.BoxCollider.Left, c.BoxCollider.Right);
            int closeY = (int)Math.Clamp(center.Y, c.BoxCollider.Top, c.BoxCollider.Bottom);
            //uses the corner thats closest to the center
            float dist = (float)Math.Sqrt(Math.Pow(center.X - closeX, 2) + Math.Pow(center.Y - closeY, 2));
            if (dist <= radius)
            {
                overlap.Add(c);
            }
        }
        return overlap;
    }
    /// <summary>
    /// circlecast drom a spefici point, will return a list of all icollidable obkects in this section
    /// </summary>
    /// <param name="radius"></param>
    /// <param name="pos"></param>
    /// <returns></returns>
    public List<ICollidable> CircleCast(int radius, Point pos, Layer? layer = null, Tag? tag = null )
    {
        List<ICollidable> overlap = new List<ICollidable>();
        foreach (ICollidable c in CollidableObjects)
        {
            if (!c.activeSelf || !c.ColliderIsActive) { continue; }
            if (layer != null && c.layer == (Layer)layer) { continue; }
            if (tag != null && !c.owner.GetTag((Tag)tag)) { continue; }

            //gameobject center, or else everything else is at the top le

            int closeX = (int)Math.Clamp(pos.X, c.BoxCollider.Left, c.BoxCollider.Right);
            int closeY = (int)Math.Clamp(pos.Y, c.BoxCollider.Top, c.BoxCollider.Bottom);
            //uses the corner thats closest to the center
            float dist = (float)Math.Sqrt(Math.Pow(pos.X - closeX, 2) + Math.Pow(pos.Y - closeY, 2));
            if (dist <= radius)
            {
                overlap.Add(c);
            }
        }
        return overlap;
    }




    //these methodsd are used mainly for scene loading and unloading
    public void AddCollidableList(List<ICollidable> cols, bool isStatic = false)
    {
        foreach (ICollidable c in cols)
        {
            AddCollidable(c, isStatic);
        }
    }
    public void RemoveColliderList(List<ICollidable> cols)
    {
        foreach (ICollidable c in cols)
        {
            RemoveCollider(c);
        }

    }

    //I NEED TO MAKE AN ONENTERCOLLISION, ONEXITCOLLISION, ONSTAY?COLLISION
    //MAYBE ADD TRIGGER? 


    public bool PixelPerfectCollision(IPixelCollidable a, IPixelCollidable b)
    {
        //rework at future date
        // i could also combine w basic check, if in border then also run a pixel perfect check? 
        //should take two gameobject or sprites and check the overlap
        //only take into account the pixels that are overlapping and have a positive alpha value?
        //return true if collision, false if not
        Rectangle rectoverlap = Rectangle.Intersect(a.BoxCollider, b.BoxCollider);
        if (rectoverlap.Width == 0 || rectoverlap.Height == 0)
        {
            return false;
        }
        ascaleX = a.BoxCollider.Width / (float)a.sprite.Region.Width;
        ascaleY = a.BoxCollider.Height / (float)a.sprite.Region.Height;
        bscaleX = b.BoxCollider.Width / (float)b.sprite.Region.Width;
        bscaleY = b.BoxCollider.Height / (float)b.sprite.Region.Height;
        //must use region.width/region.height and not .height or .width becuz region uses unscaled, and .width/height scales up.


        for (int i = rectoverlap.X; i < rectoverlap.Right; i++)
        {
            for (int j = rectoverlap.Y; j < rectoverlap.Bottom; j++)
            {
                int xposA = i - a.BoxCollider.Left;
                int xposB = i - b.BoxCollider.Left;
                int yposA = j - a.BoxCollider.Top;
                int yposB = j - b.BoxCollider.Top;
                //pixel pos on enlarged texture

                int maskAX = (int)MathF.Floor(xposA / ascaleX);
                int maskAY = (int)MathF.Floor(yposA / ascaleY);
                int maskBX = (int)MathF.Floor(xposB / bscaleX);
                int maskBY = (int)MathF.Floor(yposB / bscaleY);
                //convert to normal size position to check for alpha

                bool Aalpha = a.sprite.Region.alphaMask[a.sprite.Region.Width * maskAY + maskAX];
                bool Balpha = b.sprite.Region.alphaMask[b.sprite.Region.Width * maskBY + maskBX];
                if (Aalpha && Balpha)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// attempts to move the object in a specific direction and returns the new position?
    /// </summary>
    /// <param name="newPos"> amount you want to move</param>
    /// <param name="obj">gameobject to move</param>
    /// <param name="onlyCollideWithStatic"> only collide w static objects?</param>
    /// <returns></returns>
    public Vector2 Move(Vector2 newPos, ICollidable obj,bool onlyCollideWithStatic = false)
    {
        //returns the possible movement
        float x = MoveHorizontal(newPos.X, obj, onlyCollideWithStatic);
        obj.transform.position.X += x;
        //move horizontal then vert
        float y = MoveVertical(newPos.Y, obj, onlyCollideWithStatic);
        obj.transform.position.Y += y;
        return new Vector2(x, y);
        //not ideal, might need to rework at a later point, but should like be fine for handling collisions right now
    }
    /// <summary>
    /// Horizontal movement amount
    /// </summary>
    /// <param name="xDist"> distance in the X direction to move</param>
    /// <param name="obj"> object to move</param>
    /// <param name="onlyCollideWithStatic"> only collide w static objects?</param>
    /// <returns></returns>
    float MoveHorizontal(float xDist, ICollidable obj, bool onlyCollideWithStatic = false)
    {
        float finaldist = xDist;

        IEnumerable<ICollidable> colliders = StaticCollidableObjects;

        if (!onlyCollideWithStatic)
        {
            colliders = GetAllColliders();
        }

        foreach (ICollidable col in colliders)
        {
            if (obj.owner != null && obj.owner == col.owner)
            {
                continue;
            }
            if (col == obj || !col.activeSelf || !col.ColliderIsActive || col.IsTrigger || obj.IsTrigger) { continue; }
            bool vertOverlap = (obj.BoxCollider.Top < col.BoxCollider.Bottom && obj.BoxCollider.Bottom > col.BoxCollider.Top);
            if (!vertOverlap) { continue; }
            if (obj is IPixelCollidable && col is IPixelCollidable)
            {
                continue;
            }
            if (xDist > 0)
            {
                if (col.BoxCollider.Left >= obj.BoxCollider.Right)
                {
                    float gap = col.BoxCollider.Left - obj.BoxCollider.Right;
                    if (gap != finaldist)
                    {
                        finaldist = Math.Min(finaldist, gap);
                    }
                }
            }
            else if (xDist < 0)
            {
                if (col.BoxCollider.Right <= obj.BoxCollider.Left)
                {
                    float gap = col.BoxCollider.Right - obj.BoxCollider.Left;
                    if (gap != finaldist)
                    {
                        finaldist = Math.Max(finaldist, gap);
                    }
                }
            }

        }

        if (finaldist < xDist)
        {
            //collision has occured?
        }
        return finaldist;
    }
    float MoveVertical(float yDist, ICollidable obj, bool onlyCollideWithStatic = false)
    {
        float finaldist = yDist;
        IEnumerable<ICollidable> colliders = StaticCollidableObjects;

        if (!onlyCollideWithStatic)
        {
            colliders = GetAllColliders();
        }

        foreach (ICollidable col in colliders)
        {
            if (col == obj || !col.activeSelf || !col.ColliderIsActive || col.IsTrigger || obj.IsTrigger) { continue; }
            bool hOverlap = (obj.BoxCollider.Right > col.BoxCollider.Left && obj.BoxCollider.Left < col.BoxCollider.Right);
            if (!hOverlap) { continue; }
            if (obj is IPixelCollidable && col is IPixelCollidable)
            {
                continue;
            }
            if (yDist > 0)
            {
                if (col.BoxCollider.Top >= obj.BoxCollider.Bottom)
                {
                    float gap = col.BoxCollider.Top - obj.BoxCollider.Bottom;
                    if (gap != finaldist)
                    {

                        finaldist = Math.Min(finaldist, gap);
                    }
                }
            }
            else if (yDist < 0)
            {
                if (col.BoxCollider.Bottom <= obj.BoxCollider.Top)
                {
                    float gap = col.BoxCollider.Bottom - obj.BoxCollider.Top;
                    if (gap != finaldist)
                    {
                        finaldist = Math.Max(finaldist, gap);
                    }
                }

            }

        }
        if (finaldist < yDist)
        {

        }
        return finaldist;
    }

    IEnumerable<ICollidable> GetAllColliders()
    {
        foreach (ICollidable col in CollidableObjects)
            yield return col;

        foreach (ICollidable col in StaticCollidableObjects)
            yield return col;
    }
}