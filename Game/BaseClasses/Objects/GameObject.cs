using GameBase.Assets;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Timers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace GameBase;

public class GameObject : IObject
{
    public TransformComponent transformC { get; }
    public Transform transform => transformC.transform;

    public bool IsActive { get; private set; } = true;
    //readonly mainly for pooling
    public string name { get; protected set; }
    // identifier 
    public SpriteComponent spriteC { get; }
    public Sprite sprite => spriteC.sprite;
    public Scene ownerScene { get; protected set; }
    public bool ActiveOnSceneEnter = true;
    public Layer layer { get; protected set; } = Layer.Default;

    public HashSet<Tag> tags { get; private set; } = new();

    public bool activeSelf => IsActive;

    private List<IComponent> components = new();

    private List<IUpdatable> updatecomponentsToAdd = new();
    private List<IUpdatable> updatecomponentsToRemove = new();
    private List<IDrawable> drawcomponentsToAdd = new();
    private List<IDrawable> drawcomponentsToRemove = new();
    private List<IUpdatable> updateComponents = new();
    private List<IDrawable> drawComponents = new();
    bool inited = false;

    //finish in class

    //public bool spriteVisible = true;
    //float invisibilityTimer = 0f;
    public GameObject(string spriteName, Scene owner, string name = null): this(Services.Get<SpriteLibrary>().CreateSprite(spriteName), owner, name)
    { 
    }
    public GameObject(Sprite sprite, Scene owner, string name = null)
    {

        var trans = new TransformComponent(Vector2.Zero, 0, new Vector2(64, 64), this);
        transformC = trans;
        AddComponent(transformC);

        this.name = name;
        var s = new SpriteComponent(sprite, this);
        spriteC = s;
        AddComponent(spriteC);
        ownerScene = owner;
        inited = false;
    }

    public virtual void Init()
    {
        if (inited) return;
        //initialize components here
        ProcessComponents();
        foreach (IComponent component in components)
        {
            component.Connect();
        }
        inited = true;
    }

    public virtual void Update(GameTime gameTime)
    {
        //whereever handles updates should check if each gameobject is active before updating them
        //collision handler

        foreach (IUpdatable component in updateComponents)
        {
            if (component is IObject o && o.activeSelf)
            {
                component.Update(gameTime);
            }
        }
        ProcessComponents();
    }
    public void UpdateName(string newName)
    {
        //updates gameobject name
        name = newName;
    }
    public void UpdateSprite(Sprite newSprite)
    {
        spriteC.UpdateSprite(newSprite);
    }
    public void UpdateLocationX(float x)
    {
        //updates gameobject location x direction
        transform.position = new Vector2(x, transform.position.Y);
        //reduces update calls? 
    }
    public void UpdateLocationY(float y)
    {
        //updates gameobject location y direction
        transform.position = new Vector2(transform.position.X, y);
    }
    public void UpdateLocation(float x, float y)
    {
        //updates gameobject location x and y direction
        transform.position = new Vector2(x, y);
    }
    public void UpdateLocation(Vector2 pos)
    {
        //updates gameobject location x and y direction
        transform.position = pos;
    }
    public void UpdateRotation(float r)
    {
        //updates gameobject rotation
        transform.rotation = r;
    }
    public void UpdateSize(int x, int y)
    {
        transform.scale = new Vector2(x, y);
        //updates gameobject pixel size
        spriteC.UpdateSize(x, y);

    }

    public void UpdateLayer(Layer newLayer)
    {
        layer = newLayer;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {

        // first rect draws location and size, second rectangle is spritesheet location and size, color is tinting

        //should the object that inherits from this handle draw? or should the base handle?
        foreach (IDrawable component in drawComponents)
        {
            if ( component is IObject o && o.activeSelf)
            {
                component.Draw(spriteBatch);
            }
        }
    }

    public void SetActive(bool state)
    {
        IsActive = state;
    }
    public void AddComponent(IComponent c)
    {
        if (c.owner == this && !components.Contains(c))
        {
            components.Add(c);
            if (c is IUpdatable)
            {
                updatecomponentsToAdd.Add(c as IUpdatable);

            }
            if (c is IDrawable)
            {
                drawcomponentsToAdd.Add(c as IDrawable);
            }
            if(inited)
            {
                c.Connect();
            }
        }
    }
    public T GetComponent<T>() where T : class
    {
        foreach (IComponent component in components)
        {
            if (component is T match)
            {
                return match;
            }
        }

        return null;
    }
    public List<T> GetComponents<T>() where T : class
    {
        List<T> c = new();
        foreach (IComponent component in components)
        {
            if (component is T match)
            {
                c.Add(match);
            }
        }

        return c;
    }


    public void AddTag(Tag tag)
    {
        tags.Add(tag);
    }

    public void RemoveTag(Tag tag)
    {
        tags.Remove(tag);
    }

    public bool GetTag(Tag tag)
    {
        return tags.Contains(tag);
    }

    public virtual void OnDestroy() {
        //object subscription clean up here.
        foreach (Component c in components) {
            c.OnDestroy();
        }
    }
    public void RemoveComponent(IComponent c)
    {
        if (components.Contains(c))
        {
            if (c is IUpdatable)
            {
                updatecomponentsToRemove.Add(c as IUpdatable);
            }
            if (c is IDrawable)
            {
                drawcomponentsToRemove.Add(c as IDrawable);
  
            }
            (c as IObject).OnDestroy();
            (c as IObject).SetActive(false);
            components.Remove(c);
        }
    }

    public void ProcessComponents() {
        if (updatecomponentsToAdd.Count > 0)
        {
            updateComponents.AddRange(updatecomponentsToAdd);
            updatecomponentsToAdd.Clear();
        }
        if (updatecomponentsToRemove.Count > 0)
        {
            updateComponents.RemoveAll(updatecomponentsToRemove.Contains);
            updatecomponentsToRemove.Clear();
        }
        if (drawcomponentsToAdd.Count > 0)
        {
            drawComponents.AddRange(drawcomponentsToAdd);
            drawcomponentsToAdd.Clear();
        }
        if (drawcomponentsToRemove.Count > 0)
        {
            drawComponents.RemoveAll(drawcomponentsToRemove.Contains);
            drawcomponentsToRemove.Clear();
        }
    }
}
