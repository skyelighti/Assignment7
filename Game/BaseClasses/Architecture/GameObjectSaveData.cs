using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class GameObjectSaveData
{
    /// <summary>
    /// Basic information needed for each gameobject when saving and loading a scene
    /// </summary>


    /// <summary> 
    /// I should convert to a GUID, but is chill rn, this is for storing eaach specific instance
    /// </summary>
    public string InstanceId { get; set; }
    /// <summary>
    /// this is for like prefab type, so the game knows what to build, ex enemy, player, etc.
    /// </summary>
    public string PrefabId { get; set; }
    public string DefinitionId { get; set; }
    public HashSet<Tag> Tags { get; set; }
    public bool IsActive { get; set; } = true;
    public bool ActiveOnSceneEnter { get; set; } = true;

    public float X { get; set; }
    public float Y { get; set; }
    public float Rotation { get; set; }
    public float ScaleX { get; set; }
    public float ScaleY { get; set; }
    public int? Health { get; set; }
    //npc and item dont have hp
    public float? RemainingLifetime { get; set; }
    //bullet

}

