using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public class InteractableComponent : Component, IInteractable
{

    public bool CanInteract { get; private set; } = true;

    public string ID => owner.name;
    public event Action<GameObject> Interacted;

    public InteractableComponent(GameObject o) : base(o)
    {
    }

    /// <summary>
    /// lets player handle the interaction or allows other components to subscribe and handle how the player handles the interaction
    /// also invokes global interaciton id
    /// </summary>
    /// <param name="player"></param>
    public void Interact(GameObject player)
    {
        if (!activeSelf || !CanInteract)
        {
            return;
        }

        Interacted?.Invoke(player);
        EventSystem.OnInteract(ID);
    }
    public void SetCanInteract(bool state)
    {
        CanInteract = state;
    }

    public override void OnDestroy()
    {
    }

    public override void Connect()
    {
    }
}
