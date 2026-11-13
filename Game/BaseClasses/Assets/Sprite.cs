using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase.Assets;
public class Sprite
{
    /// <summary>
    /// Gets or Sets the source texture region represented by this sprite.
    /// </summary>
    public TextureRegion Region { get; set; }

    /// <summary>
    /// Gets or Sets the color mask to apply when rendering this sprite.
    /// </summary>
    /// <remarks>
    /// Default value is Color.White
    /// </remarks>
    public Color Color { get; set; } = Color.White;
    public Vector2 DrawOffset { get; set; } = Vector2.Zero;

    public Transform transform { get; set; } = new Transform(Vector2.Zero, 0.0f, Vector2.One);
    //position = origin = 0
    //rotation = 0f
    //scale = 1,1

    /// <summary>
    /// Gets or Sets the sprite effects to apply when rendering this sprite.
    /// </summary>
    /// <remarks>
    /// Default value is SpriteEffects.None
    /// </remarks>
    public SpriteEffects Effects { get; set; } = SpriteEffects.None;

    /// <summary>
    /// Gets or Sets the layer depth to apply when rendering this sprite.
    /// </summary>
    /// <remarks>
    /// Default value is 0.0f
    /// </remarks>
    public float LayerDepth { get; set; } = 0.0f;

    /// <summary>
    /// Gets the width, in pixels, of this sprite. 
    /// </summary>
    /// <remarks>
    /// Width is calculated by multiplying the width of the source texture region by the x-axis scale factor.
    /// </remarks>
    public float Width => Region.Width * transform.scale.X;

    /// <summary>
    /// Gets the height, in pixels, of this sprite.
    /// </summary>
    /// <remarks>
    /// Height is calculated by multiplying the height of the source texture region by the y-axis scale factor.
    /// </remarks>
    public float Height => Region.Height * transform.scale.Y;

    public Sprite() { }
    public Sprite(TextureRegion region)
    {
        Region = region;
    }
    public void CenterOrigin()
    {
        transform.position = new Vector2(Region.Width, Region.Height) * 0.5f;
    }
    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
        Region.Draw(spriteBatch, position + DrawOffset, Color, transform.rotation, transform.position, transform.scale, Effects, LayerDepth);
    }
    public virtual void Update(GameTime gameTime)
    {
        //animated sprite is cast as sprite but still needs acess to update method
    }
}
