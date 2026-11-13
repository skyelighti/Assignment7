using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
    Tilemap ground;
    List<GameObject> spawnedObjects = new();
    GameObject player;
    PlayerMovementComponent playerMovement;
    SpriteLibrary sprites;
    InputManager input;
    bool resetRequested;
    public IReadOnlyList<SpawnInfo> Spawns { get; }

    public GameScene(ContentManager content) : base(content)
    {
        sprites = Services.Get<SpriteLibrary>();
        input = Services.Get<InputManager>();
        LevelData level = LevelLoader.LoadLevelData(content, "LoadInfo/TileMap1.xml");
        ground = level.Map;
        Spawns = level.spawns;
        ground.Scale = new Vector2(2, 2);

        foreach (TileCollider col in ground.CreateColliders())
        {
            AddCollider(col, true);
        }
        sceneUI = new BasicSceneUI(this);
        CreateSpawns();
        ProcessObj();
    }
    public override void EnterScene()
    {
        if (isActive) { return; }
        base.EnterScene();
        input.ActionPressed += OnActionPressed;
    }
    public override void Update(GameTime gameTime)
    {
        if (resetRequested)
        {
            ResetLevel();
        }

        ground.Update(gameTime);
        base.Update(gameTime);
        if (player.transform.position.Y > ground.Rows * ground.TileHeight)
        {
            playerMovement.Die();
        }

        float cameraX = player.transform.position.X + player.transform.scale.X / 2 - Main.ScreenBounds.Width / 2f;
        float maxX = Math.Max(0, ground.Columns * ground.TileWidth - Main.ScreenBounds.Width);
        Main.camera.Position = new Vector2(MathHelper.Clamp(cameraX, 0, maxX), 0);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        ground.Draw(spriteBatch);
        base.Draw(spriteBatch);
    }
    public override void ExitScene()
    {
        if (!isActive) { return; }
        input.ActionPressed -= OnActionPressed;
        base.ExitScene();
    }

    private void CreateSpawns()
    {
        foreach (SpawnInfo spawn in Spawns)
        {
            Vector2 position = new Vector2(spawn.Column * ground.TileWidth, spawn.Row * ground.TileHeight);
            GameObject obj;
            switch (spawn.Type)
            {
                case "Player":
                    Sprite playerSprite = sprites.GetSpriteSheet("Assets/PlayerSpriteSheet.png").CreateSprite(13);
                    obj = Prefabs.CreatePlayer(this, playerSprite, position);
                    player = obj;
                    playerMovement = player.GetComponent<PlayerMovementComponent>();
                    playerMovement.OnDeath += RequestReset;
                    break;
                case "Enemy":
                    Sprite enemySprite = sprites.GetSpriteSheet("Assets/PlayerSpriteSheet.png").CreateSprite(13);
                    enemySprite.Color = Color.OrangeRed;
                    obj = Prefabs.CreateEnemy(this, enemySprite, position);
                    break;
                default:
                    throw new InvalidOperationException("Unknown spawn type: " + spawn.Type);
            }
            spawnedObjects.Add(obj);
            AddObject(obj);
        }
    }

    private void RequestReset()
    {
        resetRequested = true;
    }

    private void OnActionPressed(GameAction action)
    {
        if (action == GameAction.Restart)
        {
            RequestReset();
        }
    }

    public void ResetLevel()
    {
        resetRequested = false;
        playerMovement.OnDeath -= RequestReset;
        foreach (GameObject obj in spawnedObjects)
        {
            RemoveObject(obj);
        }
        ProcessObj();
        spawnedObjects.Clear();
        CreateSpawns();
        ProcessObj();
    }
}
