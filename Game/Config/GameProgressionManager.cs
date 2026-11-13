using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBase.Dialogue;
using GameBase.Items;

namespace GameBase;

public class GameProgressionManager
{
    public GameProgressionManager() {
        EventSystem.DialogueComplete += OnComplete;
        EventSystem.Interact += OnInteract;
        EventSystem.Pickup += OnPickup;
        EventSystem.NPCInteract += OnNPCInteract;
    }

    public void OnComplete(DialogueInfo dInfo) { 
        //i can parse for the specific dialouge ID and character here, and do case specific handling? 
        //this might get really really freaking bloated 
    }
    public void OnInteract(string ID)
    {
    }

    public void OnPickup(ItemInfo info, string ID) { 
        //store and increment score here
    
    }
    public void OnNPCInteract(Character character) { 
    
    }
}
