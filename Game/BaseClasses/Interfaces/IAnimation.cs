using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Animations;

/// <summary>
/// interface for IAnimation, so any class that implements Ianimation will always have these
/// this might currently be a little redundant as only Animation implements this
/// </summary>
public interface IAnimation
{
    string name { get; }
    IReadOnlyList<IAnimationFrame> Frames { get; }
    int FrameCount { get; }
    bool IsLooping { get; }
    bool IsReversed { get; }
    bool IsPingPong { get; }
}
