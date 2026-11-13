using System;
using System.Collections.Generic;

namespace GameBase.Dialogue;

/// <summary>
/// behavior for a talkable character. When the owner's InteractableComponent fires,
/// this plays the correct dialogue segment (based on persistence progress) and announces
/// the interaction globally via EventSystem.
/// Assembly (which components an NPC is built from) lives in Prefabs.CreateNPC, not here.
/// </summary>
public class NPCBehavior : Component
{
    public Character character { get; private set; }

    readonly List<DialogueInfo> dialogueInfoList = new();

    InteractableComponent interact;
    DialougeComponent dialouge;
    PersistenceManager persistence;

    public NPCBehavior(GameObject o, NPCInfo info) : base(o)
    {
        // constructor = data only; wiring happens in Connect()
        character = info.character;
        var gameDef = Services.Get<GameDefinitions>();
        foreach (string id in info.dialogueIDOrder)
        {
            dialogueInfoList.Add(gameDef.dialogueById[id]);
        }
    }

    public override void Connect()
    {
        // runs via GameObject.Init() once all components are attached
        interact = owner.GetComponent<InteractableComponent>();
        dialouge = owner.GetComponent<DialougeComponent>();
        persistence = Services.Get<PersistenceManager>();
        interact.Interacted += OnInteract;
    }

    void OnInteract(GameObject obj)
    {
        if (!activeSelf || !interact.CanInteract) return;

        EventSystem.OnNPCInteract(character);

        int index = persistence.GetDialogueIndex(character);
        if (index >= 0 && index < dialogueInfoList.Count)
        {
            dialouge.Play(dialogueInfoList[index]);
        }
    }

    public override void OnDestroy()
    {
        if (interact != null)
        {
            interact.Interacted -= OnInteract;
        }
    }
}
