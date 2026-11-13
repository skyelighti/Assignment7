using GameBase.Dialogue;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace GameBase.Input;

public partial class InputManager
{
    /// <summary>
    /// Gets the state information of keyboard input.
    /// </summary>
    public KeyboardInfo Keyboard { get; private set; }

    /// <summary>
    /// Gets the state information of mouse input.
    /// </summary>
    public MouseInfo Mouse { get; private set; }
    public event Action<GameAction> ActionPressed;
    public event Action<GameAction> ActionReleased;
    public event Action<GameAction> ActionKeyDown;
    public InputActionMap inputActionMap { get; private set; } = InputActionMap.Gameplay;
    public event Action<InputActionMap> InputMapChanged;

      /// <summary>
    /// Creates a new InputManager.
    /// </summary>
    public InputManager()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();
        InitActionMaps();
        //partial class - is in gamme

    }

    public void UpdateActionMap(InputActionMap map)
    {
        inputActionMap = map;
        InputMapChanged?.Invoke(map);
    }
    /// <summary>
    /// Updates the state information for the keyboard, mouse, and gamepad inputs.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
    public void Update(GameTime gameTime)
    {
        Keyboard.Update();
        Mouse.Update();
        var startmap = inputActionMap;
        foreach (var binding in ActionMap[startmap])
        {
            bool justPressed = false;
            bool justReleased = false;
            bool isDown = false;
            foreach ( InputBinding b in binding.Value) {
                if (b.WasJustPressed(Keyboard, Mouse)) {
                    justPressed = true;
                }
                else if (b.WasJustReleased(Keyboard, Mouse))
                {
                    justReleased = true;
                }
                if (b.IsDown(Keyboard,Mouse))
                {
                    isDown = true;
                }
                
            }
            if (justPressed)
            {
                ActionPressed?.Invoke(binding.Key);
                if (inputActionMap != startmap)
                {
                    return;
                }
            }
            if (justReleased && !isDown)
            {
                ActionReleased?.Invoke(binding.Key);
                if (inputActionMap != startmap)
                {
                    return;
                }
            }
            if (isDown)
            {
                ActionKeyDown?.Invoke(binding.Key);
                if (inputActionMap != startmap)
                {
                    return;
                }
            }


        }
        //prevents double firing for input :P
    }

    public void UpdateKeyBindings(InputActionMap map, GameAction action, List<InputBinding> newBindings)
    {
        if (ActionMap[map].ContainsKey(action))
        {
            ActionMap[map][action] = new List<InputBinding>(newBindings);
        }
    }


}

