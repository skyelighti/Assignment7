using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Animations;

public enum AnimationEventTrigger
{
    /// <summary>
    /// Triggered at the beginning of a frame.
    /// </summary>
    FrameBegin,

    /// <summary>
    /// Triggered at the end of a frame.
    /// </summary>
    FrameEnd,

    /// <summary>
    /// Triggered when a frame is skipped.
    /// </summary>
    FrameSkipped,

    /// <summary>
    /// Triggered when the animation loops.
    /// </summary>
    AnimationLoop,

    /// <summary>
    /// Triggered when the animation completes.
    /// </summary>
    AnimationCompleted,

    /// <summary>
    /// Triggered when the animation stops.
    /// </summary>
    AnimationStopped,
}