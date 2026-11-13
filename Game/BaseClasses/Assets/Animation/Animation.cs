using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Animations;

/// <summary>
///mainly used within settup, this holds each animation
/// </summary>
public class Animation : IAnimation
{
    private readonly AnimationFrame[] _frames;
    public string name { get; }

    public IReadOnlyList<IAnimationFrame> Frames => _frames;
    public int FrameCount => _frames.Length;

    public bool IsLooping { get; }

    public bool IsReversed { get; }

    public bool IsPingPong { get; }
    public Animation(string n, IReadOnlyList<AnimationFrame> frames, bool isLooping, bool isReversed, bool isPingPong) {
        name = n;
        _frames = frames.ToArray();
        IsLooping = isLooping;
        IsReversed = isReversed;
        IsPingPong = isPingPong;
    }
}
