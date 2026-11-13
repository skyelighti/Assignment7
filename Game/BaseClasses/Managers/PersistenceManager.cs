using GameBase.Dialogue;
using GameBase.Items;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.IO;
using System.Text.Json;

namespace GameBase;

public partial class PersistenceManager
{
    /// <summary>
    /// currently handles most persistence/player informamtion stuff, that needs to be stored.
    /// most of these things would be informamtion that would nice to use on certain conditions or occasions for quest or dialouge progression
    /// </summary>
    //finish this tmmrw, but after each thing we should do a specific check for a hardcoded reaction? that gets handled by the game specific portion of this class.
    //move all these managers into base classes after.
    public Dictionary<Character, int> npcDialogueProgress = new Dictionary<Character, int>();
    //public HashSet<string> collectedItems = new HashSet<string>();
    //for item picksups probs wont be used here
    public HashSet<string> interactedItems = new HashSet<string>();
    public HashSet<string> collectedItems = new HashSet<string>();
    public HashSet<string> completedDialogue = new HashSet<string>();
    public HashSet<Character> talkedToNPCs = new HashSet<Character>();
    public HealthData PlayerHealth { get; } = new HealthData(6);
    private string SavePath => "save.json";

    public PersistenceManager()
    {
        EventSystem.DialogueComplete += OnComplete;
        EventSystem.Interact += OnInteract;
        EventSystem.Pickup += OnPickup;
        EventSystem.NPCInteract += OnNPCInteract;
    }

    private void OnNPCInteract(Character character)
    {
        talkedToNPCs.Add(character);
    }



    public void Reset()
    {
        npcDialogueProgress.Clear();
        interactedItems.Clear();
        completedDialogue.Clear();
        talkedToNPCs.Clear();
        collectedItems.Clear();
    }
    void OnComplete(DialogueInfo dInfo)
    {
        //add to completed dialogue
        //have this pass the dialogue info instead
        completedDialogue.Add(dInfo.ID);
        if (dInfo.advancesProgress && dInfo.character != null)
        {
            AdvanceDialogue((Character)dInfo.character);
        }
    }
    void OnInteract(string ID)
    {
        interactedItems.Add(ID);
    }
    void OnPickup(ItemInfo info, string ID)
    {
        if (string.IsNullOrEmpty(ID)) { return; }
        interactedItems.Add(ID);
        collectedItems.Add(ID);
    }
    public int GetDialogueIndex(Character c)
    {
        if (npcDialogueProgress.ContainsKey(c))
        {
            return npcDialogueProgress[c];
        }
        else
        {
            npcDialogueProgress[c] = 0;
            return 0;
        }
    }
    public void AdvanceDialogue(Character c)
    {

        if (npcDialogueProgress.ContainsKey(c))
        {
            npcDialogueProgress[c]++;
        }
        else
        {
            npcDialogueProgress[c] = 1;
        }
    }



    public void WriteSave(SaveData data)
    {

        string json = JsonSerializer.Serialize(data, new JsonSerializerOptions{WriteIndented = true});
        File.WriteAllText(SavePath, json);
    }
    public bool HasSave()
    {
        return File.Exists(SavePath);
    }
    public SaveData ReadSave()
    {
        string json = File.ReadAllText(SavePath);

        SaveData data = JsonSerializer.Deserialize<SaveData>(json);


        return data;
    }

    public void CaptureState(SaveData save)
    {
        save.interactedItems = new HashSet<string>(interactedItems);
        save.collectedItems = new HashSet<string>(collectedItems);
        save.completedDialogue = new HashSet<string>(completedDialogue);
        save.talkedToNPCs = new HashSet<Character>(talkedToNPCs);

        save.npcDialogueProgress =
            new Dictionary<Character, int>(npcDialogueProgress);

        // inventory is optional (enableItems) — guard instead of assuming it exists
        if (Services.TryGet(out Inventory inventory))
        {
            save.Currency = inventory.currencyCount;
            save.InventoryItemIds = inventory.items.Select(item => item.ID).ToList();
        }
    }

    public void RestoreState(SaveData save)
    {
        Reset();

        interactedItems.UnionWith(save.interactedItems);
        collectedItems.UnionWith(save.collectedItems);
        completedDialogue.UnionWith(save.completedDialogue);
        talkedToNPCs.UnionWith(save.talkedToNPCs);

        foreach (var entry in save.npcDialogueProgress)
        {
            npcDialogueProgress.Add(entry.Key, entry.Value);
        }

        if (Services.TryGet(out Inventory inventory))
        {
            inventory.RestoreState(save.InventoryItemIds, save.Currency);
        }
    }
}
