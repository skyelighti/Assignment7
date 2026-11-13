using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameBase.Input;
using GameBase.Assets;
using GameBase.Dialogue;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection.Metadata;

namespace GameBase;

public class Main
{
    public static Rectangle ScreenBounds;
    public static ContentManager contentManager { get; private set; }
    private SceneManager sceneManager;
    public static MonoGame.Extended.OrthographicCamera camera;
    private bool saveRequested;
    private bool loadRequested;

    InputManager input;
    DialogueManager dialogue;        // optional — null when dialogue is disabled
    PersistenceManager persistence;

    public Main(ContentManager content)
    {
        contentManager = content;
        var sprites = new SpriteLibrary(content);
        var gameD = new GameDefinitions(content);
        //UIManager = new UIManager(content);
        Services.Initialize(sprites, gameD, true, false);
        //simplify sprite loading? 

        sceneManager = new SceneManager(content);

        // cache services once; Initialize has already registered them
        input = Services.Get<InputManager>();
        persistence = Services.Get<PersistenceManager>();
        Services.TryGet(out dialogue);

        input.ActionPressed += OnActionPressed;
    }
    //load textures and atlas here for right now? maybe a dedicated asset loader?
    //maybe implement a statemaachine for game state? :P extra work to do if i hate reading!!
    public void Update(GameTime gameTime, Rectangle screenBounds)
    {
        ScreenBounds = screenBounds;

        input.Update(gameTime);

        if (loadRequested)
        {
            loadRequested = false;
            saveRequested = false;

            LoadGame();
            return;
        }

        sceneManager.Update(gameTime);
        dialogue?.Update(gameTime);
        CollisionManager.Instance.Update(gameTime);

        if (saveRequested)
        {
            saveRequested = false;
            SaveGame();
        }
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        sceneManager.Draw(spriteBatch);
        //UIManager.Draw(spriteBatch);
    }

    private void SaveGame()
    {
        SaveData save = sceneManager.CaptureState();

        persistence.CaptureState(save);
        persistence.WriteSave(save);
    }


    private void LoadGame()
    {
        if (!persistence.HasSave())
        {
            return;
        }

        SaveData save = persistence.ReadSave();

        persistence.RestoreState(save);
        sceneManager.RestoreState(save);
    }
    private void OnActionPressed(GameAction action)
    {
        if (action == GameAction.Save)
        {
            saveRequested = true;
        }
        else if (action == GameAction.Load)
        {
            loadRequested = true;
        }
    }
}
