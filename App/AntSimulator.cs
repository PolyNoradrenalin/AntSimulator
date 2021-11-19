using System;
using System.Collections.Generic;
using AntEngine;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using AntEngine.Utils.Maths;
using App.Renderers;
using App.Renderers.EntityRenderers;
using App.UIElements;
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
            base.Initialize();
            
            SimFrame mainSimFrame = new SimFrame(world);
            
            renderers.Add(mainSimFrame);

            for (int i = 0; i < 100; i++)
            {
                Ant a = new Ant("EntityTest", new Transform(new Vector2(new Random().Next(10, 490), new Random().Next(10, 490)), 0, new Vector2(30, 30)), world);
            }
            
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            SimFrame.EntityTexture = Content.Load<Texture2D>("Entities/Entity");
            SimFrame.AntTexture = Content.Load<Texture2D>("Entities/Ant");
            SimFrame.ColonyTexture = Content.Load<Texture2D>("Entities/Colony");
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