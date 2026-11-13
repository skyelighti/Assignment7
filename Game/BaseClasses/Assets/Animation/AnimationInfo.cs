using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

/// <summary>
/// basic information about an animation, needed for loading from jsons/txt files
/// </summary>
public class AnimationInfo
{
    public List<int> framesindx { get; set; } = new();
    public bool isLooping { get; set; }
    public bool isReversed { get; set; }
    public bool isPingPong { get; set; }

}