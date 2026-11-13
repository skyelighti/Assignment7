using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GameBase.Input;
/// <summary>
/// this is a wrapper for both mouse and key inputs, allowing for mouse and keyboard input to be parsed for actions 
/// </summary>
public class InputBinding
{
    //this is mainly allowing keyboard and mouse presses and releases to be interchangable
    public MouseButton? mouseButton;
    public Keys? key;
    public InputBinding(MouseButton mouse) {
        mouseButton = mouse;
    }
    public InputBinding(Keys k) {
        key = k;
    }
    public bool IsDown(KeyboardInfo keyboard, MouseInfo mouse) {
        if (mouseButton != null)
        {
            return mouse.IsButtonDown((MouseButton)mouseButton);
        }
        else if (key != null) {
            return keyboard.IsKeyDown((Keys)key);
        }
        return false;
    }
    public bool IsUp(KeyboardInfo keyboard, MouseInfo mouse) {
        if (mouseButton != null)
        {
            return mouse.IsButtonUp((MouseButton)mouseButton);
        }
        else if (key != null)
        {
            return keyboard.IsKeyUp((Keys)key);
        }
        return false;
    }
    public bool WasJustPressed(KeyboardInfo keyboard, MouseInfo mouse) {
        if (mouseButton != null)
        {
            return mouse.WasButtonJustPressed((MouseButton)mouseButton);
        }
        else if (key != null)
        {
            return keyboard.WasKeyJustPressed((Keys)key);
        }
        return false;

    }
    public bool WasJustReleased(KeyboardInfo keyboard, MouseInfo mouse) {
        if (mouseButton != null)
        {
            return mouse.WasButtonJustReleased((MouseButton)mouseButton);
        }
        else if (key != null)
        {
            return keyboard.WasKeyJustReleased((Keys)key);
        }
        return false;

    }
}
