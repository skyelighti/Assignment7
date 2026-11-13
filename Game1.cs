using GameBase;
using Gum;
using Gum.Forms;
using Gum.Forms.Controls;
using Gum.Wireframe;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using BlendState = Microsoft.Xna.Framework.Graphics.BlendState;

namespace BaseTemplate { 
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        GumService GumUI => GumService.Default;
        private SpriteBatch _spriteBatch;
        private Main main;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            GumUI.Initialize(this);
            MonoGameAndGum.Renderables.ShapeRenderer.Self.Initialize();
            Gum.Wireframe.CustomSetPropertyOnRenderable.InMemoryFontCreator =
                new KernSmith.Gum.KernSmithFontCreator(GraphicsDevice);
            Gum.Expressions.GumExpressionService.Initialize();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Main.ScreenBounds = GraphicsDevice.Viewport.Bounds;
            Main.camera = new MonoGame.Extended.OrthographicCamera(GraphicsDevice);
            main = new Main(Content);

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            GumUI.Update(gameTime);
            // TODO: Add your update logic here
            main.Update(gameTime, GraphicsDevice.Viewport.Bounds);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, blendState: BlendState.AlphaBlend, transformMatrix: Main.camera.GetViewMatrix());
            // TODO: Add your drawing code here
            main.Draw(_spriteBatch);
            _spriteBatch.End();
            GumUI.Draw();
            base.Draw(gameTime);
        }
    }
}
