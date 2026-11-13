using GameBase.Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Dialogue;
/// <summary>
/// this should play for both items and NPCs, ex, if the player inspects something within the game world
/// </summary>
public class DialougeComponent: Component
{
    DialogueManager dialogue; // optional — null when dialogue is disabled

    public DialougeComponent(GameObject o) : base(o)
    {
    }

    public override void Connect()
    {
        Services.TryGet(out dialogue);
    }
    public void Play(DialogueInfo info)
    {
        if (!activeSelf || info == null || dialogue == null) return;
        dialogue.DialogueSetup(info);
    }

    public override void OnDestroy()
    {
        
    }

}
