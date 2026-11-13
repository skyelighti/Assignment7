using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;
/// <summary>
/// info to store when loading in a spritesheet from a json file
/// </summary>
public class SpriteSheetInfo
{
    public string name { get; set; }
    public string texture { get; set; }
    public int frameWidth { get; set; }
    public int frameHeight { get; set; }
    public double frameDuration { get; set; }

    public Dictionary<string, AnimationInfo> animations { get; set; } = new();
}
