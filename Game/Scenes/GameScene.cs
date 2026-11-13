using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using GameBase.Tiles;

namespace GameBase;

public class GameScene : Scene
{
    private readonly Tilemap ground;
    List<ICollidable> tileColliders = new();
    public IReadOnlyList<SpawnInfo> Spawns { get; }

    //

    //private readonly Player player;
    // references to a tilemap & player if we using either
    public GameScene(ContentManager content) : base(content)
    {
        //load level here. 
        LevelData level = LevelLoader.LoadLevelData(content, "TileMap1.xml");
        ground = level.Map;
        Spawns = level.spawns;
        ground.Scale = new Vector2(3, 3);

        //foreach spawn in spawns: get location, and spawn type here
        //resettiilecolliders
    }
    public override void EnterScene()
    {
        base.EnterScene();
    }
    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }
    public override void ExitScene()
    {
        base.ExitScene();
    }

    public void ResetLevel() { 
    //if level requires a hard reset
    }
}
