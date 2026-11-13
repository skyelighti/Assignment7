using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
public class ObjectPool<T> where T : GameObject
{

    /// <summary>
    /// Generic object pool
    /// </summary>
    public int poolSize { get; }
    //readonly, u dont want to modify after created i think
    List<T> objPool;

    public ObjectPool(int size, Func<T> createObj)
    {
        this.poolSize = size;
        this.objPool = new List<T>();
        for (int i = 0; i < size; i++)
        {
            T obj = createObj();
            objPool.Add(obj);
            // generic function, we input name for the function that creataes the obj
            //calls actual function to create, stores in obj of type T(placeholder/generic?)
            //adds to pool
        }
    }
    //remove/refactor later lol
    public void Update(GameTime gameTime)
    {
        foreach (T obj in objPool)
        {
            if (obj.IsActive)
            {
                obj.Update(gameTime);
            }
        }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (T obj in objPool)
        {
            if (obj.IsActive)
            {
                obj.Draw(spriteBatch);
            }
        }
    }
    public T GetPooledObj()
    {
        foreach (T obj in objPool)
        {
            if (!obj.IsActive)
            {
                return obj;
                //object cleanup, make sure its not currently moving & reset position? 
                //i might just leave that for the specific instance, 
            }
            //check if object is active/figure out how to tag/activate idk
        }
        return null;
    }
    public IEnumerable<T> GetActiveObjects()
    {
        foreach (T obj in objPool)
        {
            if (obj.IsActive)
                yield return obj;
        }
        //so other functions can iterate through 
    }
    public void ResetPool() {
        foreach (T obj in objPool) {
            obj.SetActive(false);
        }
    }

}
