
using GameBase.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
/// <summary>
/// Event system, for events that should be able to be subscribed to from anywhere within the project, like a system-wide annoucement.
/// This is also a partial class, so if there are any game specific events that aren't applicable to the template as a whole, i can keep them seperate
/// The goal of anything and everything within baseclasses is that once im doing with architecture, i dont have to touch them ever again when im making a game.
/// </summary>
public static partial class EventSystem
{
    //edit this as needed.
    /// <summary>
    /// on interact, interactable component calls iirc, passes the instance id?
    /// mainly for persitance, is called every time an interactable is interacted with
    /// </summary>
    public static event Action<string> Interact;
    /// <summary>
    /// for pickup item, runs when the item is picked up, passes the item info and instance id
    /// </summary>
    public static event Action<ItemInfo, string> Pickup;

    /// <summary>
    /// runs when a dialouge segment gets completed, passes the dialogue info
    /// mainly for incrementing and processing when the next bit of dialouge gets triggered, or when the dialouge is fully completed
    /// </summary>
    public static event Action<DialogueInfo> DialogueComplete;
    /// <summary>
    /// runs when the NPC is interacted with. this is mainly for tracking when an NPC is interacted with for the first time
    /// could be somewhat redundant but i think it's nice to have if i need something specific
    /// </summary>
    public static event Action<Character> NPCInteract;
    //global events that the GameInfoManager can like subscribe to, so a reference isnt needed

    /// <summary>
    /// call this when needing to trigger a global event for OnInteract
    /// </summary>
    /// <param name="ID"> pass the instance id or null through, depending if you want to record a specific item pickup or not</param>
    public static void OnInteract(string ID) {
        Interact?.Invoke(ID);
    }

    /// <summary>
    /// call this when wanting to trigger a global event for pickup
    /// </summary>
    /// <param name="info">item info, what was just picked up</param>
    /// <param name="ID">instance ID, for tracking</param>
    public static void OnPickup(ItemInfo info, string ID)
    {
        Pickup?.Invoke(info, ID);
    }

    /// <summary>
    /// call this when triggering a global event for dialouge complete
    /// </summary>
    /// <param name="dialogueInfo"> dialouge info, so like what segement just got finished</param>
    public static void OnDialogueComplete(DialogueInfo dialogueInfo)
    {
        DialogueComplete?.Invoke(dialogueInfo);
    }

    /// <summary>
    /// call this when triggering a global event for NPC nteract
    /// </summary>
    /// <param name="c">which character was just interacted with?</param>
    public static void OnNPCInteract(Character c) {
        NPCInteract?.Invoke(c);
    }
}
