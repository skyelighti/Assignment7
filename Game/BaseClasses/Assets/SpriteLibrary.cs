using Microsoft.Xna.Framework.Content;
using RenderingLibrary.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBase;
using GameBase.Animations;
using Microsoft.Xna.Framework.Graphics;

namespace GameBase.Assets;
/// <summary>
/// this basically holds all the information for visual stuff
/// it hold all the texture regions, spritesheets, and animations
/// </summary>
public class SpriteLibrary
{
    private Dictionary<string, TextureRegion> _regions;
    private Dictionary<string, SpriteSheet> _spriteSheets = new();
    private ContentLoader _contentLoader;
    Dictionary<string, IAnimation> animations = new();
    public SpriteLibrary(ContentManager content)
    {
        _spriteSheets = new Dictionary<string, SpriteSheet>();
        _regions = new Dictionary<string, TextureRegion>();
        _contentLoader = new ContentLoader(content, this);
    }
    /// <summary>
    /// adds a region with a name to the regions dict held by this class
    /// </summary>
    /// <param name="name">key for the region when storing</param>
    /// <param name="region">texture region to store</param>
    public void AddRegion(string name, TextureRegion region)
    {
        _regions.Add(name, region);
    }

    /// <summary>
    /// gets the region from the regions dict
    /// </summary>
    /// <param name="name">the key the region is stored under in the dict</param>
    /// <returns></returns>
    /// <remarks>
    /// if region was added through atlas, its atlasname.regionname
    /// iff manually added, its whatever i passed in gamecontentloader
    /// </remarks>
    public TextureRegion GetRegion(string name)
    {
        return _regions[name];
    }
    public bool RemoveRegion(string name)
    {
        return _regions.Remove(name);
    }
    /// <summary>
    /// adds a spritesheet to the dict
    /// </summary>
    /// <param name="sheetName">key</param>
    /// <param name="sheet">value</param>
    /// <remarks>
    /// if loaded from json file, sheetName is the value stored in name
    /// </remarks>
    public void AddSpriteSheet(string sheetName, SpriteSheet sheet)
    {
        _spriteSheets.Add(sheetName, sheet);
    }

    /// <summary>
    /// gets a spritesheet based upon name value
    /// </summary>
    /// <param name="sheetName"> if added through json, most likely the value stored in there</param>
    /// <returns></returns>
    public SpriteSheet GetSpriteSheet(string sheetName)
    {
        return _spriteSheets[sheetName];
    }
    public bool RemoveSpriteSheet(string sheetName)
    {
        return _spriteSheets.Remove(sheetName);
    }
    /// <summary>
    /// Creates a new animated sprite using the animation from this texture atlas with the specified name.
    /// </summary>
    /// <param name="animationName">The name of the animation to use.</param>
    /// <returns>A new AnimatedSprite using the animation with the specified name.</returns>
    public AnimatedSprite CreateAnimatedSprite(string sheetName, string animationName)
    {
        IAnimation animation = GetAnimation($"{sheetName}.{animationName}");
        return new AnimatedSprite(animation);
    }
    public Sprite CreateSprite(string regionName)
    {
        TextureRegion region = GetRegion(regionName);
        return new Sprite(region);
    }
    public void AddAtlas(string atlasName, TextureAtlas atlas)
    {
        foreach (var entry in atlas.Regions)
        {
            string spriteName = $"{atlasName}.{entry.Key}";
            AddRegion(spriteName, entry.Value);
        }
    }

    public TextureAtlas LoadSprite(ContentManager content, string assetLocation, string name) {
        Texture2D texture = content.Load<Texture2D>(assetLocation);
        TextureAtlas atlas = new TextureAtlas(texture);
        atlas.AddRegion(name, 0, 0 , texture.Width, texture.Height);
        AddRegion(name, atlas.GetRegion(name));
        return atlas;
    }

    /// <summary>
    /// adds an animation to the animation dict
    /// </summary>
    /// <param name="name"> name passed through the json anim loader</param>
    /// <param name="animation"></param>
    public void AddAnimation(string name, IAnimation animation)
    {
        animations.Add(name, animation);
    }
    /// <summary>
    /// gets and returns an animation
    /// </summary>
    /// <param name="name">most likely the name passed through the json animloader</param>
    /// <returns></returns>
    public IAnimation GetAnimation(string name)
    {
        return animations[name];
    }
}
