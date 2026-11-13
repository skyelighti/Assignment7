using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBase.Assets;

namespace GameBase;
public abstract class Scene
{
    List<GameObject> sceneObjects = new List<GameObject>();
    List<GameObject> toRemove = new List<GameObject>();
    List<GameObject> toAdd = new();
    List<ICollidable> sceneColliders = new List<ICollidable>();
    List<ICollidable> staticSceneColliders = new();
    public TextureAtlas atlas;
    public SceneUI sceneUI { get; protected set; }
    public bool isActive { get; protected set; } = false;
    

    public Scene(ContentManager content)
    { //, SceneUI sUI) {
        //idk
        //sceneUI = sUI;
    }
    public virtual void EnterScene()
    {
        if (isActive) { return; }
        isActive = true;
        sceneUI?.SetActive(true);
        //load scene UI
        foreach (GameObject g in sceneObjects)
        {
            if (g.ActiveOnSceneEnter)
            {
                g.SetActive(true);
            }
            else
            {
                g.SetActive(false);
            }
            //this might need modification, ex. object pools should not be active at enter.
        }
        CollisionManager.Instance.AddCollidableList(sceneColliders);
        CollisionManager.Instance.AddCollidableList(staticSceneColliders, true);
    }
    public virtual void ExitScene()
    {
        if (!isActive) { return; }
        sceneUI?.SetActive(false);
        //unload Scene UI
        foreach (GameObject g in sceneObjects)
        {
            g.SetActive(false);
        }
        CollisionManager.Instance.RemoveColliderList(sceneColliders);
        CollisionManager.Instance.RemoveColliderList(staticSceneColliders);
        isActive = false;
    }

    public void AddObject(GameObject obj, bool isStatic = false)
    {
        if (!sceneObjects.Contains(obj) && !toAdd.Contains(obj))
        {
            toAdd.Add(obj);
        }
    }

    public void RemoveObject(GameObject obj)
    {
        if (sceneObjects.Contains(obj))
        {
            obj.SetActive(false);
            toRemove.Add(obj);
        }
    }

    public void AddCollider(ICollidable col, bool isStatic = false)
    {
        if (!sceneColliders.Contains(col)&& !staticSceneColliders.Contains(col))
        {
            if (isStatic)
            {
                staticSceneColliders.Add(col);
            }
            else
            {
                sceneColliders.Add(col);
            }
            if (isActive)
            {
                CollisionManager.Instance.AddCollidable(col, isStatic);
            }
        }
    }

    public void RemoveCollider(ICollidable col)
    {
        if (sceneColliders.Contains(col) || staticSceneColliders.Contains(col))
        {
            sceneColliders.Remove(col);
            staticSceneColliders.Remove(col);
            CollisionManager.Instance.RemoveCollider(col);
        }
    }


    public virtual void Update(GameTime gameTime)
    {
        foreach (GameObject g in sceneObjects)
        {
            if (g.IsActive)
            {
                g.Update(gameTime);
            }
        }
        ProcessObj();
    }

    public void ProcessObj()
    {
        List<GameObject> removing = new List<GameObject>(toRemove);
        toRemove.Clear();

        foreach (GameObject obj in removing)
        {
            if (sceneObjects.Contains(obj))
            {
                sceneObjects.Remove(obj);
                obj.OnDestroy();
            }
        }

        List<GameObject> adding = new List<GameObject>(toAdd);
        toAdd.Clear();

        foreach (GameObject obj in adding)
        {
            obj.Init();
            sceneObjects.Add(obj);
        }
    }
    public virtual void Draw(SpriteBatch spriteBatch)
    {
        foreach (GameObject g in sceneObjects)
        {
            if (g.IsActive)
            {
                g.Draw(spriteBatch);
            }
        }
    }
    public virtual void ResetScene() { 
    //reset everything
    }
    private void ClearObjects()
    {
        while (toAdd.Count > 0 || toRemove.Count > 0)
        {
            ProcessObj();
        }

        List<GameObject> oldObjects = new List<GameObject>(sceneObjects);

        foreach (GameObject obj in oldObjects)
        {
            obj.SetActive(false);
        }
        foreach (GameObject obj in oldObjects)
        {
            obj.OnDestroy();
        }

        sceneObjects.Clear();
        toAdd.Clear();
        toRemove.Clear();
    }
    public List<GameObjectSaveData> CaptureState()
    {
        List<GameObjectSaveData> records = new();
        foreach (GameObject sobj in sceneObjects)
        {
            if(sobj.GetComponent<PersistenceComponent>() != null)
            {
                GameObjectSaveData saveData = sobj.GetComponent<PersistenceComponent>().CaptureState();
                records.Add(saveData);
            }
        }
        return records;
    }

    public void RestoreState(IEnumerable<GameObjectSaveData> records)
    {
        List<GameObjectSaveData> savedObjects =new List<GameObjectSaveData>(records);
        ClearObjects();
        CreateUnsavedObjects();
        List<GameObject> restoredObjects = new List<GameObject>();

        foreach (GameObjectSaveData data in savedObjects)
        {
            GameObject obj = Prefabs.CreateFromSave(this, data);

            AddObject(obj);
            restoredObjects.Add(obj);
        }
        while (toAdd.Count > 0 || toRemove.Count > 0)
        {
            ProcessObj();
        }
        for (int i = 0; i < restoredObjects.Count; i++)
        {
            GameObject obj = restoredObjects[i];
            GameObjectSaveData data = savedObjects[i];
            PersistenceComponent persistence = obj.GetComponent<PersistenceComponent>();
            persistence.RestoreState(data);
        }
        OnObjectsRestored();
    }
    protected virtual void CreateUnsavedObjects()
    {
    }
    protected virtual void OnObjectsRestored()
    {
    }
}
