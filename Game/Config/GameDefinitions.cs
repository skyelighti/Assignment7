using GameBase.Dialogue;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class GameDefinitions
{
    List<DialogueInfo> definitions;
    public Dictionary<string, DialogueInfo> dialogueById { get; private set; } = new();
    public Dictionary<Character, NPCInfo> npcByCharacter { get; } = new();
    public Dictionary<string, ItemInfo> itemList { get; private set; } = new();
    public ContentManager content;
    public GameDefinitions(ContentManager c) {
        content = c;
    }
    public void InitItemList()
    {
        List<ItemInfo> infos = ItemInfo.FromFile(content, "LoadInfo/ItemDefinitions.xml");
        foreach (ItemInfo info in infos)
        {
            itemList[info.ID] = info;
        }
        //populate inventorylist items
    }

    public void InitDialogueLines()
    {
        definitions = DialogueInfo.FromFile(content, "LoadInfo/DialogueList.xml");
        foreach (DialogueInfo info in definitions)
        {
            dialogueById.Add(info.ID, info);
        }
    }
    public void InitNPCDefinitions()
    {
        foreach (NPCInfo info in NPCInfo.FromFile(content, "LoadInfo/NPCDefinitions.xml"))
        {
            npcByCharacter.Add(info.character, info);
        }
    }
    public ItemInfo GetItemFromDict(string info)
    {
        return itemList[info];
    }

}
