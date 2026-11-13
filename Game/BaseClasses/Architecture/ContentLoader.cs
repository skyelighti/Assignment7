using GameBase.Assets;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

/// <summary>
/// Loads all outside content. I have this settup currently as a partial class, so i can keep things that will probably be constant throughout every project that uses this template
/// This will hold a default texture, default spritesheet
/// </summary>
public partial class ContentLoader
{
    //modify as needed
    public ContentLoader(ContentManager content, SpriteLibrary sprites)
    {
        //initalize all textures here. 

        Texture2D texture = content.Load<Texture2D>("Assets/placeholder");

        TextureAtlas atlas = new TextureAtlas(texture);
        atlas.AddRegion("placeholder", 0, 0, texture.Width, texture.Height);

        sprites.AddRegion("placeholder", atlas.GetRegion("placeholder"));
        //make a function that just loads from file lwk

        Init(content, sprites);

    }
    public void Init(ContentManager content, SpriteLibrary sprites) {
        //initalize any constants like maybe a placeholder texture?
        GameSpecificInit(content, sprites);
    }
    partial void GameSpecificInit(ContentManager content, SpriteLibrary sprites);
}
