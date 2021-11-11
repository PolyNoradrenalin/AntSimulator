using System.Collections.Generic;
using AntEngine;
using AntEngine.Entities.Ants;
using AntEngine.Utils.Maths;
using App.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = System.Numerics.Vector2;


namespace App
{
    public class AntSimulator : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private World world;
        private List<IRenderer> renderers;
        
        public AntSimulator()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            world = new World(Vector2.One * 500);
            renderers = new List<IRenderer>();
        }

        protected override void Initialize()
        {
            SimFrame mainSimFrame = new SimFrame();
            
            renderers.Add(mainSimFrame);
            
            mainSimFrame.AddRenderer(new EntityRenderer(new Ant("Entities/DefaultEntity", new Transform(new Vector2(50, 50), 0, new Vector2(100, 100)), world)));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            EntityRenderer.entityCharset = Content.Load<Texture2D>("Entities/EntityDefault");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            world.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            
            foreach (IRenderer r in renderers)
            {
                r.Render(_spriteBatch, _graphics);
            }

            base.Draw(gameTime);
            
            _spriteBatch.End();
        }
    }
}