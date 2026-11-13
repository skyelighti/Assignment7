using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Animations;

/// <summary>
/// animation controller, implements Ianimationcontroller
///can set specific animation params here
/// </summary>
public class AnimationController: IAnimationController
{
    public bool IsPaused { get; private set; }
    bool isPlaying;

    public IAnimation Animation { get; private set; }

    public bool IsAnimating => isPlaying && !IsPaused;

    public bool IsLooping => Animation?.IsLooping ?? false;
    public bool IsReversed => Animation?.IsReversed ?? false;
    public bool IsPingPong => Animation?.IsPingPong ?? false;
    public double Speed { get; private set; } = 1;
    private int direction = 1;

    public TimeSpan CurrentFrameTimeRemaining { get; private set; }

    public int CurrentFrame { get; private set; }

    public int FrameCount => Animation?.FrameCount ?? 0;
    //if animation isnt null gets frame count, otherwise 0

    /// <summary>
    /// subscribe and unsubscribe as needed. This broadcasts various stages in the animation, so sorting for the specific animation trigger is often needed
    /// </summary>
    public event Action<IAnimationController, AnimationEventTrigger> OnAnimationEvent;
    
    /// <summary>
    /// this handles all the animation changing stuff. 
    /// however, to change anims call requestanim on animated sprite, so it can clear after update
    /// </summary>
    /// <param name="animation"></param>
    public void ChangeAnimation(IAnimation animation)
    {
        if (animation == null) { return; }
        Animation = animation;
        direction = IsReversed ? -1 : 1;
        SetFrame(direction == 1 ? 0 : FrameCount - 1);
        //need to copy playback settings
    }
    /// <summary>
    /// sets animation speed when playing
    /// defaults at 1, can change to slow or fast forwards anim
    /// does not support reversing animations, use is reversed for that
    /// </summary>
    /// <param name="speed"></param>
    public void SetSpeed(double speed) {
        if (speed >= 0) {
            Speed = speed;
        }
    }
    /// <summary>
    /// sets the current frame of the animation playing
    /// </summary>
    /// <param name="index"></param>
    public void SetFrame(int index)
    {
        if (Animation == null) { return; }
        if (index < FrameCount && index >= 0)
        {
            CurrentFrame = index;
            CurrentFrameTimeRemaining = Animation.Frames[index].Duration;
            OnAnimationEvent?.Invoke(this, AnimationEventTrigger.FrameBegin);
        }
    }

    /// <summary>
    /// pausing the currently playing animation, to resume call unpause
    /// </summary>
    /// <returns>true if paused, false if is already paused or no animation </returns>
    public bool Pause()
    {
        if (!IsAnimating) { 
            return false;
            //cannot pause cuz already paused
        }

        IsPaused = true;
        return true;
    }
    /// <summary>
    /// takes an overload for whether to reset frame duration when pausing
    /// to unpause call unpause, not play
    /// </summary>
    /// <param name="resetFrameDuration"></param>
    /// <returns>true if paused, false if is already paused or cannot pause cuz no anim</returns>
    public bool Pause(bool resetFrameDuration)
    {
    
        if (!Pause())
            return false;

        if (resetFrameDuration)
            CurrentFrameTimeRemaining = Animation.Frames[CurrentFrame].Duration;

        return true;
    }
    /// <summary>
    /// plays an animation from from the first frame
    /// </summary>
    /// <returns>false if no animaation or no frames, true is playing</returns>
    public bool Play()
    {
        int startingFrame = IsReversed ? FrameCount - 1 : 0;
        return Play(startingFrame);
    }

    /// <summary>
    /// plays an animation from a certain frame
    /// </summary>
    /// <param name="startingFrame"> frame to start from</param>
    /// <returns>true if playing is possible & it played, false otherwise</returns>
    public bool Play(int startingFrame)
    {
        if (Animation == null || FrameCount == 0) { return false; }
        isPlaying = true;
        IsPaused = false;
        direction = IsReversed ? -1 : 1;
        SetFrame(startingFrame);
        return true;
    }
     
    /// <summary>
    /// Resets the entire animation, returning to the starting frame.
    /// </summary>
    public void Reset()
    {
        Stop();
        direction = IsReversed ? -1 : 1;
        SetFrame(IsReversed ? FrameCount - 1 : 0);
    }
    /// <summary>
    /// advances to the next frame if possible
    /// </summary>
    /// <returns>returns false if not possible, and true if possible</returns>
    public bool NextFrame() {
        if (Animation == null) { return false; }
        int nextF = CurrentFrame + direction;
        if (nextF >= 0 && nextF < FrameCount)
        {
            SetFrame(nextF);
            return true;
        }
        if (FrameCount == 1)
        {
            if (!IsLooping) 
            {
                Complete();
                return false;
            }

            SetFrame(0);
            OnAnimationEvent?.Invoke(this, AnimationEventTrigger.AnimationLoop);
            return true;
        }
        if (IsPingPong)
        {
            int startingDirection = IsReversed ? -1 : 1;
            bool returning = direction != startingDirection;

            if (returning && !IsLooping)
            {
                Complete();
                return false;
            }

            direction = -direction;
            SetFrame(CurrentFrame + direction);
            return true;
        }

        if (IsLooping)
        {
            SetFrame(direction == 1 ? 0 : FrameCount - 1);
            OnAnimationEvent?.Invoke(this, AnimationEventTrigger.AnimationLoop);
            return true;
        }

        Complete();
        return false;

    }

    /// <summary>
    /// On animation complete, this resets the animation and triggers animationcompleted
    /// </summary>
    /// <returns>true</returns>
    public bool Complete() {
        isPlaying = false;
        IsPaused = false;
        CurrentFrameTimeRemaining = TimeSpan.Zero;

        OnAnimationEvent?.Invoke(this, AnimationEventTrigger.AnimationCompleted);
        return true;
    }

    /// <summary>
    /// stops the current animation. does not allow for unpause or pause to be called after stop
    /// to start anim again, call play
    /// leaves the current frame as is when stopped
    /// </summary>
    /// <returns>true</returns>
    public bool Stop()
    {
        if (!isPlaying && !IsPaused) { return true; }
        isPlaying = false;
        IsPaused = false;
        OnAnimationEvent?.Invoke(this, AnimationEventTrigger.AnimationStopped);
        return true;

    }

    /// <summary>
    /// unpauses the current animation if it paused
    /// </summary>
    /// <returns>returns true if can unpause, false if cannot</returns>
    public bool Unpause()
    {
        if (!isPlaying || !IsPaused)
        {
            return false;
        }

        IsPaused = false;
        return true;
    }

    /// <summary>
    /// unpauses the current animation, has an overload to advance to the next frame
    /// otherwise can just call unpause
    /// </summary>
    /// <param name="advanceToNextFrame">bool, true if need to advanced to the next frame</param>
    /// <returns>true if is possible and can do it, otherwise false</returns>
    public bool Unpause(bool advanceToNextFrame)
    {
        if (!isPlaying || !IsPaused)
            return false;

        if (advanceToNextFrame && !NextFrame())
            return false;

        return Unpause();
    }

    public void Update(GameTime gameTime)
    {
        if (!IsAnimating || Speed == 0)
            return;

        CurrentFrameTimeRemaining -= gameTime.ElapsedGameTime * Speed;

        while (IsAnimating && CurrentFrameTimeRemaining <= TimeSpan.Zero)
        {
            TimeSpan leftover = CurrentFrameTimeRemaining;
            OnAnimationEvent?.Invoke(this, AnimationEventTrigger.FrameEnd);
            if (!IsAnimating) return;
            if (!NextFrame())
            {
                return;
            }

            CurrentFrameTimeRemaining += leftover;
        }
    }
    //holy fuck im not implementing this this week
    //https://deepwiki.com/craftworkgames/MonoGame.Extended/2.2-animation-system

}
