using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class PersistenceComponent : Component
{
    public string InstanceId { get; }
    public string PrefabId { get; }
    //based off prefab ID we can tell what type of info to create/get
    public string DefinitionId { get; }
    public PersistenceComponent(GameObject o, string prefabID, string instanceID, string definitionId = null) : base(o)
    {
        PrefabId = prefabID;
        InstanceId = instanceID;
        DefinitionId = definitionId;
    }
    
    public GameObjectSaveData CaptureState()
    {
        GameObjectSaveData saveData = new GameObjectSaveData();
        saveData.InstanceId = InstanceId;
        saveData.PrefabId = PrefabId;
        saveData.DefinitionId = DefinitionId;
        saveData.X = owner.transform.position.X;
        saveData.Y = owner.transform.position.Y;
        saveData.Rotation = owner.transform.rotation;
        saveData.ScaleX = owner.transform.scale.X;
        saveData.ScaleY = owner.transform.scale.Y; 
        saveData.IsActive = owner.IsActive;
        saveData.ActiveOnSceneEnter = owner.ActiveOnSceneEnter;
        saveData.Tags = new HashSet<Tag>(owner.tags);

        foreach (IPersistent component in owner.GetComponents<IPersistent>())
        {
            component.CaptureState(saveData);
        }
        return saveData;
    }

    public void RestoreState(GameObjectSaveData data)
    {
        owner.UpdateLocation(data.X, data.Y);
        owner.UpdateSize((int)data.ScaleX, (int)data.ScaleY);
        owner.transform.rotation = data.Rotation;
        owner.ActiveOnSceneEnter = data.ActiveOnSceneEnter;
        owner.SetActive(data.IsActive);
        if (data.Tags != null)
        {
            owner.tags.Clear();
            owner.tags.UnionWith(data.Tags);
        }

        foreach (IPersistent component in owner.GetComponents<IPersistent>())
        {
            component.RestoreState(data);
        }
    }

    public override void OnDestroy()
    {
    }

    public override void Connect()
    {
    }
}
