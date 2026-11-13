using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace GameBase;

public partial class ContentLoader
{
    //wow so reusable
    //me being intimidated by mason last year turns out to be actually useful
    partial void GameSpecificInit(ContentManager content, SpriteLibrary sprites)
    {
        //game specific content goes here!!
        SpriteSheetAnimationBuilder.FromFile(content, "LoadInfo/SpriteSheetAnimations.json", sprites);
    }
}
