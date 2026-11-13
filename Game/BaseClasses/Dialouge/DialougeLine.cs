using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Dialogue;

/// <summary>
/// a dialouge line that a certain character says, with the dialouge
/// mainly used in loading in info
/// </summary>
public class DialogueLine
{
    public string Name;
    public string dialogue;
    public Sprite sprite;
    // should this contain a string to the location of the sprite instaead?
    public Sprite background;
    public DialogueLine(string n, string d, Sprite s, Sprite b)
    {
        Name = n;
        dialogue = d;
        sprite = s;
        background = b;
    }
}
