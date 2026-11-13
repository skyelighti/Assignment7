using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  GameBase.Animations;
/// <summary>
/// class for spritesheets, contains the basic information for frames 
/// mainly used within settup
/// </summary>
public class SpriteSheet
{
    // for storing and reference purposes
    public string Name { get; private set; }
    TextureAtlas textureAtlas;
    public int frameHeight { get; }
    public int frameWidth { get; }

    public int regionCount => textureAtlas.RegionCount;
    public int columns { get; }
    public int rows { get; }

    public SpriteSheet(string name, TextureAtlas atlas, int frameH, int frameW, int cols, int rows) {
        textureAtlas = atlas;
        Name = name;
        frameHeight = frameH;
        frameWidth = frameW;
        columns = cols;
        this.rows = rows;
    }
    public Sprite CreateSprite(int regionIndex)
    {
        return new Sprite(textureAtlas.GetTextureRegion(regionIndex));
    }
    public TextureRegion GetTextureRegion(int regionIndex)
    {
        return textureAtlas.GetTextureRegion(regionIndex);
    }
}
