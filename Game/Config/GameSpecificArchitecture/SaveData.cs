using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class SaveData
{
    //maybe partial this later
    public SceneID currentScene { get; set; }
    public List<string> InventoryItemIds { get; set; } = new();
    public HashSet<string> interactedItems { get; set; } = new();
    public HashSet<string> collectedItems { get; set; } = new();
    public HashSet<string> completedDialogue { get; set; } = new();
    public HashSet<Character> talkedToNPCs { get; set; } = new();

    public Dictionary<Character, int> npcDialogueProgress { get; set; } = new();
    public List<GameObjectSaveData> Objects { get; set; } = new();
    public int Currency { get; set; }

}
