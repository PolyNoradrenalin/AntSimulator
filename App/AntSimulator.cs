﻿using System;
using System.Collections.Generic;
using AntEngine;
using AntEngine.Entities;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using AntEngine.Resources;
using App.Renderers;
using App.UIElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = System.Numerics.Vector2;

namespace App
{
    public class AntSimulator : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private readonly List<IRenderer> _renderers;

        private readonly int _defaultTargetTps = 60;
        private readonly World _world;
        private DateTime _lastTimeTick;
        private SpriteBatch _spriteBatch;

        public AntSimulator()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _world = new World(Vector2.One * 500);
            _renderers = new List<IRenderer>();

            TargetTps = _defaultTargetTps;
        }

        private int TargetTps { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            
            Window.AllowUserResizing = true;
            
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;  
            _graphics.ApplyChanges();
            
            SimFrame mainSimFrame = new SimFrame(new Rectangle(0, 0, 800, 500), _world);

            Window.ClientSizeChanged += (sender, args) =>
            {
                mainSimFrame.Size = (_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
                _graphics.ApplyChanges();
            };
            
            _renderers.Add(mainSimFrame);

            Resource food = new Resource("food", "Test Food");

            Colony colony = new Colony(_world, (name, transform, world, _) => new Ant("Ant", transform, world));
            colony.Transform.Position = Vector2.One * 250F;
            colony.Transform.Scale = Vector2.One * 20F;
            colony.SpawnCost.AddResource(food, 10);
            colony.Stockpile.AddResource(food, 10000);
            colony.Spawn(50);

            for (int i = 0; i < 10; i++)
            {
                ResourceEntity foodEntity = new ResourceEntity(_world, 100000, food);
                foodEntity.Transform.Position = new Vector2(new Random().Next(10,
                        490),
                    new Random().Next(10,
                        490));
                foodEntity.Transform.Scale = Vector2.One * 10;
            }
            
            SpeedSlider speedSlider = new SpeedSlider(new Rectangle(600, 20, 100, 40), 1, 8);

            _renderers.Add(speedSlider);
            
            speedSlider.SpeedChange += OnSpeedSliderChange;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SimFrame.EntityTexture = Content.Load<Texture2D>("Entities/Entity");
            SimFrame.AntTexture = Content.Load<Texture2D>("Entities/Ant");
            SimFrame.ColonyTexture = Content.Load<Texture2D>("Entities/Colony");
            Button.DefaultTexture = Content.Load<Texture2D>("UIElements/Button");
            SpeedSlider.SpeedSliderSpriteSheet = Content.Load<Texture2D>("UIElements/SpeedSliderButtonSpriteSheet");
            //UIElement.Font = Content.Load<SpriteFont>("UIElements/TextFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            if (TargetTps != 0)
            {
                if (DateTime.Now.Subtract(_lastTimeTick).TotalSeconds >= 1f / TargetTps)
                {
                    _lastTimeTick = DateTime.Now;
                    _world.Update();
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(SpriteSortMode.FrontToBack);

            foreach (IRenderer r in _renderers)
            {
                r.Render(_spriteBatch, _graphics,
                    new Rectangle(0, 0, _graphics.GraphicsDevice.Viewport.Width,
                        _graphics.GraphicsDevice.Viewport.Height));
            }

            base.Draw(gameTime);

            _spriteBatch.End();
        }

        /// <summary>
        /// Event to be invoked when the speed slider is changed.
        /// </summary>
        /// <param name="newSpeed">New speed multiplier.</param>
        /// <param name="isPaused">Boolean variable determining if the simulation is paused or not.</param>
        private void OnSpeedSliderChange(int newSpeed, bool isPaused)
        {
            TargetTps = isPaused ? 0 : _defaultTargetTps * newSpeed;
        }
    }
}