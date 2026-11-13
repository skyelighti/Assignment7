using System;
using Microsoft.Xna.Framework;

namespace GameBase.Animations;
/// <summary>
/// Animated sprite class
/// should be able to access the animated sprite's controller when creating an animated sprite
/// inherits from the sprite class, and has public access to anim controller, and updating anim
/// </summary>
public class AnimatedSprite : Sprite
{
    private IAnimation pendingAnimation;
    //animation offset, in case visual offset is required to line up animation
    private Vector2 offSet = Vector2.Zero;

    public AnimationController Controller { get; }



    public AnimatedSprite(IAnimation anim)
    {
        Controller = new AnimationController();

        Controller.ChangeAnimation(anim);
        Controller.Play();

        UpdateRegion();
    }
    private void UpdateRegion()
    {
        IAnimationFrame frame = Controller.Animation.Frames[Controller.CurrentFrame];

        Region = frame.Region;
    }


    public override void Update(GameTime gameTime)
    {
        

        if (pendingAnimation != null)
        {
            IAnimation nextAnimation = pendingAnimation;
            pendingAnimation = null;
            DrawOffset = offSet;

            Controller.ChangeAnimation(nextAnimation);
            Controller.Play();
        }
        Controller.Update(gameTime);

        UpdateRegion();
    }

    /// <summary>
    /// change the animation playing with an optional offeset
    /// </summary>
    /// <param name="animation"> new animation</param>
    /// <param name="offset"> offset amount for the next animation</param>
    public void RequestAnimation(IAnimation animation, Vector2 offset = default)
    {
        if (animation == null) { return; }
        pendingAnimation = animation;
        offSet = offset;
    }
}
