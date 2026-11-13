using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GameBase.Animations;

/// <summary>
/// this is for loading a spritesheet animation, mainly will be loaded through json. examples are locatied in spritesheetanims json
/// call this in game content loader if needing to load more outside of the ones contained in the current json file or needing to load another
/// json file
/// </summary>
public static class SpriteSheetAnimationBuilder
{
    public static void FromFile(ContentManager content, string filename, SpriteLibrary sprites)
    {
        string filePath = Path.Combine(content.RootDirectory, filename);

        using (Stream stream = TitleContainer.OpenStream(filePath))
        {
            List<SpriteSheetInfo> sheets =JsonSerializer.Deserialize<List<SpriteSheetInfo>>(stream);

            foreach (SpriteSheetInfo info in sheets) {
                Texture2D texture = content.Load<Texture2D>(info.texture);
                TextureAtlas atlas = new TextureAtlas(texture);

                int columns = texture.Width / info.frameWidth;
                int rows = texture.Height / info.frameHeight;

                for (int row = 0; row < rows; row++)
                {
                    for (int column = 0; column < columns; column++)
                    {
                        int index = row * columns + column;

                        atlas.AddRegion(index.ToString(), column * info.frameWidth, row * info.frameHeight, info.frameWidth, info.frameHeight);
                    }
                }
                SpriteSheet sheet = new SpriteSheet(info.name, atlas, info.frameHeight, info.frameWidth, columns, rows);
                foreach (var animation in info.animations) {
                    string animName = animation.Key;
                    AnimationInfo anim = animation.Value;

                    List<AnimationFrame> animframes = new();
                    foreach (int idx in anim.framesindx) {
                        AnimationFrame f = new AnimationFrame(sheet.GetTextureRegion(idx), TimeSpan.FromSeconds(info.frameDuration));
                        animframes.Add(f);
                    }
                    Animation ssanim = new Animation(animName, animframes,anim.isLooping, anim.isReversed, anim.isPingPong);
                    sprites.AddAnimation($"{info.name}.{animName}", ssanim);
                }
                sprites.AddSpriteSheet(info.name, sheet);

            }

        }
    }
}
