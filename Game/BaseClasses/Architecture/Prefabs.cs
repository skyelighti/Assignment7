using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBase.Dialogue;

namespace GameBase;

/// <summary>
/// This class is split partially to hold creation for items & thier respective components. Game specific assemble belongs in Game Prefabs
/// </summary>
public static partial class Prefabs
{

    /// <summary>
    /// making a generic gameobject w no components, can be added/modified later
    /// </summary>
    /// <param name="owner">scene that owns this gameobject, important to have becausue scene manages activation </param>
    /// <param name="sprite">visual representation, can be animated sprite or sprite class? init/create before creation</param>
    /// <param name="layer">what layer does this object belong to?</param>
    /// <param name="position">where in the scene is this gameobject</param>
    /// <param name="size">how big is the game object, use pixel size within game world</param>
    /// <param name="rotation"> float, for rotating the sprite</param>
    /// <returns></returns>
    public static GameObject CreateGameObject(Scene owner, Sprite sprite, Layer layer, Vector2 position, Vector2 size, float rotation)
    {
        GameObject obj = new GameObject(sprite, owner);
        obj.UpdateLayer(layer);
        obj.UpdateSprite(sprite);
        obj.UpdateLocation(position);
        obj.UpdateRotation(rotation);
        obj.UpdateSize((int)size.X, (int)size.Y);
        return obj;
    }
    /// <summary>
    /// creates gameobject with a spritename & not a sprite. use if do not need anim sprite immediately
    /// </summary>
    /// <param name="owner">scene owner</param>
    /// <param name="spritename"> name of sprite, make sure its registered first w sprite library</param>
    /// <param name="layer">layer of the object created</param>
    /// <param name="position">pos of the sprite in the game world</param>
    /// <param name="size">use pixels with the sprite, in the gameworld</param>
    /// <returns></returns>
    public static GameObject CreateGameObject(Scene owner, string spritename, Layer layer, Vector2 position, Vector2 size)
    {
        return CreateGameObject(owner, Services.Get<SpriteLibrary>().CreateSprite(spritename), layer, position, size, defaultrotation);
    }
    /// <summary>
    /// creates sprite with an inital size of 16 x 16 for 2d game, use createGameObject w the size overload if 16 x 16 is too small or modify for purposes
    /// takes a string, use sprite overload if anim is immediately needed
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="spritename"></param>
    /// <param name="layer"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static GameObject CreateGameObject(Scene owner, string spritename, Layer layer, Vector2 position)
    {
        return CreateGameObject(owner, Services.Get<SpriteLibrary>().CreateSprite(spritename), layer, position, new Vector2(defaultSize, defaultSize), defaultrotation);
    }
    /// <summary>
    /// creates w init sprite for game, no intial rotation, use overload if rotation is needed
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="sprite"></param>
    /// <param name="layer"></param>
    /// <param name="position"></param>
    /// <param name="size"></param>
    /// <returns></returns>
    public static GameObject CreateGameObject(Scene owner, Sprite sprite, Layer layer, Vector2 position, Vector2 size)
    {
        return CreateGameObject(owner, sprite, layer, position, size, 0f);
    }

    /// <summary>
    /// creates with no intial rotation or size, takes in a sprite, use other overloads depending on need
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="sprite"></param>
    /// <param name="layer"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static GameObject CreateGameObject(Scene owner, Sprite sprite, Layer layer, Vector2 position)
    {
        return CreateGameObject(owner, sprite, layer, position, new Vector2(defaultSize, defaultSize), defaultrotation);
    }

    /// <summary>
    /// creates an interactable object, uses default size, default rotation and interactable layer
    /// adds a hitbox component and an interactable component, can be used for npcs, items, etc.
    /// might need additional modification based on use case
    /// </summary>
    /// <param name="owner"> scene owner</param>
    /// <param name="sprite"> takes an intial sprite/animates sprite </param>
    /// <param name="position">position in game world use pixels</param>
    /// <returns></returns>
    static GameObject CreateInteractable(Scene owner, Sprite sprite, Vector2 position) { 
        GameObject item = CreateGameObject(owner, sprite, Layer.Interactable, position);
        item.AddComponent(new HitboxComponent(item));
        item.AddComponent(new InteractableComponent(item));
        return item;

    }


    public static GameObject CreateSolid(Scene owner, Sprite sprite, Vector2 position, bool IsStatic = false) { 
        GameObject gameObject = CreateGameObject(owner, sprite, Layer.Default, position);
        gameObject.AddComponent(new HitboxComponent(gameObject, IsStatic));
        return gameObject;
    }
    public static GameObject CreateFromSave(Scene scene, GameObjectSaveData data) {
        GameObject obj;
        //TODO: specific object handling, and handling based upon dict. base on Prefab ID, we know where to look for def id
        switch (data.PrefabId)
        {
            case "test":
                obj = CreateSolid(scene,Services.Get<SpriteLibrary>().CreateSprite("placeholder"),new Vector2(data.X, data.Y));
                obj.AddComponent(new DamageComponent(6, obj));
                break;

            default:
                throw new InvalidOperationException("Unknown prefab ID: " + data.PrefabId);
        }
        obj.AddComponent(new PersistenceComponent(obj,data.PrefabId, data.InstanceId));
        
        return obj;
    }

    public static GameObject MakePersistent(GameObject obj, string prefabID, string instanceID, string definitionID = null) {
        obj.AddComponent(new PersistenceComponent(obj, prefabID, instanceID, definitionID));
        return obj;

    }
}