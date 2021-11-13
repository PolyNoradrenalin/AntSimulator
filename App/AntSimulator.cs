﻿using System.Collections.Generic;
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

            Ant a = new Ant("EntityTest", new Transform(new Vector2(50, 50), 0, new Vector2(30, 30)), world);
            Ant b = new Ant("AntTest", new Transform(new Vector2(150, 150), 0, new Vector2(20, 20)), world);
            Colony c = new Colony("ColonyTest", new Transform(new Vector2(300, 170), 0, new Vector2(64, 64)), world,
                (s, t, w, c) => new Ant(world));
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