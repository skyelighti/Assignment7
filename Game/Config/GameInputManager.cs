using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Input;

public partial class InputManager
{
    //everything here is going to change game to game. everything else in input manager should stay the same
    private readonly Dictionary<GameAction, List<InputBinding>> _actionKeys = new()
    {
        [GameAction.MoveLeft] = new List<InputBinding> { new InputBinding(Keys.A), new InputBinding(Keys.Left) },
        [GameAction.MoveRight] = new List<InputBinding> { new InputBinding(Keys.D), new InputBinding(Keys.Right) },
        [GameAction.Jump] = new List<InputBinding> { new InputBinding(Keys.Space) },
        [GameAction.Restart] = new List<InputBinding> { new InputBinding(Keys.R) }
        //ALLOW FOR MOUSE CLICKING LATER
    };
    //contents can still change. dict itself cannot.

    private readonly Dictionary<GameAction, List<InputBinding>> _dialogueKeys = new()
    {
        [GameAction.ProgressDialogue] = new List<InputBinding> { new InputBinding(Keys.Space), new InputBinding(Keys.Enter) },
        //ALLOW FOR MOUSE CLICKING LATER
    };
    private readonly Dictionary<GameAction, List<InputBinding>> _inventoryKeys = new()
    {
        [GameAction.ToggleInventory] = new List<InputBinding> { new InputBinding(Keys.I), new InputBinding(Keys.Escape) }
    };

    readonly Dictionary<InputActionMap, Dictionary<GameAction, List<InputBinding>>> ActionMap = new();

    public void InitActionMaps() {
        ActionMap[InputActionMap.Gameplay] = _actionKeys;
        ActionMap[InputActionMap.Dialogue] = _dialogueKeys;
        ActionMap[InputActionMap.Inventory] = _inventoryKeys;
    }
}
