using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Animations;

/// <summary>
/// this comprises each frame within an animation
/// </summary>
public class AnimationFrame : IAnimationFrame
{
    public TextureRegion Region { get; }

    public TimeSpan Duration { get;}
    public AnimationFrame(TextureRegion region, TimeSpan duration) {
        Region = region;
        Duration = duration;
    
    }
}
