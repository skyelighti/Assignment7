using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameBase;

namespace GameBase;

public partial class SceneManager
{
    public static SceneManager Instance { get; private set; }
    public StateMachine SceneM = new StateMachine();
    Dictionary<SceneID, Scene> SceneDict = new Dictionary<SceneID, Scene>();

    //inits scenes, in scene manager and sets the functions for enter update and exiting. 

    public void Update(GameTime gameTime)
    {
        SceneM.UpdateState(gameTime);
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        if (SceneM.CurrentState == -1) { return; }
        SceneDict[(SceneID)SceneM.CurrentState].Draw(spriteBatch);
        //update shouldnt know draw spritebatchinfo
    }

    public void RestoreState(SaveData data)
    {
        SceneM.SetState((int)data.currentScene);
        SceneM.ApplyPendingTransition();
        Scene scene = SceneDict[data.currentScene];
        scene.ProcessObj();
        scene.RestoreState(data.Objects);
    }
    public SaveData CaptureState()
    {
        SceneID sceneId = (SceneID)SceneM.CurrentState;
        Scene scene = SceneDict[sceneId];

        scene.ProcessObj();

        return new SaveData
        {
            currentScene = sceneId,
            Objects = scene.CaptureState()
        };
    }
}
