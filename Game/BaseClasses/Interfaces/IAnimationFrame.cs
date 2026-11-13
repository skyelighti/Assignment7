using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Animations;

/// <summary>
/// Defines the interface for an animation frame, specifying the frame index and its duration.
///</summary>
public interface IAnimationFrame
{
    /// <summary>
    /// Gets the frame index represented by this animation frame.
    /// </summary>
    TextureRegion Region { get; }

    /// <summary>
    /// Gets the total duration this frame should be displayed during an animation.
    /// </summary>
    TimeSpan Duration { get; }
}
