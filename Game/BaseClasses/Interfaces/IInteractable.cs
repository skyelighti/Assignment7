using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

/// <summary>
/// anything that implements Iinteractable means that you must be able to inteact with it
/// </summary>
public interface IInteractable
{
    // removed can interact and ID because canInteract is redundant, and ID belongs with more persistence
    public void Interact(GameObject player);
}
