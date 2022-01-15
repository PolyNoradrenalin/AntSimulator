using System;
using System.Collections.Generic;
using System.Globalization;
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
        private const string PropertiesFileName = "simulator.properties";
        
        private readonly GraphicsDeviceManager _graphics;
        private readonly List<IRenderer> _renderers;

        private const int _defaultTargetTps = 30;
        private bool _isPaused = true;
        private readonly World _world;
        private DateTime _lastTimeTick;
        private SpriteBatch _spriteBatch;

        public static Properties Properties { get; private set; }
        
        public AntSimulator()
        {
            Properties = new Properties(PropertiesFileName);
            Properties.Save();
            
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            float worldX = float.Parse(Properties.Get("world_size_x", "1000"), CultureInfo.InvariantCulture);
            float worldY = float.Parse(Properties.Get("world_size_y", "1000"), CultureInfo.InvariantCulture);
            int worldCollider = int.Parse(Properties.Get("world_collider_div", "64"));
            int worldRegion = int.Parse(Properties.Get("world_region_div", "256"));
            _world = new World(new Vector2(worldX, worldY), worldCollider, worldRegion);
            
            _renderers = new List<IRenderer>();
            TargetTps = _defaultTargetTps;
        }

        private int TargetTps { get; set; }

        protected override void Initialize()
        {
            base.Initialize();
            
            Window.AllowUserResizing = true;
            IsFixedTimeStep = false;
            
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 500;
            _graphics.SynchronizeWithVerticalRetrace = false;
            _graphics.ApplyChanges();

            SimFrame mainSimFrame = new SimFrame(new Rectangle(0, 0, 800, 500), _world);

            SpeedSlider speedSlider = new SpeedSlider(new Rectangle(_graphics.GraphicsDevice.Viewport.Width - _graphics.GraphicsDevice.Viewport.Width / 10, 20, 3 * 32, 32), 1, 16);

            speedSlider.SpeedChange += OnSpeedSliderChange;
            
            Window.ClientSizeChanged += (sender, args) =>
            {
                mainSimFrame.Size = (_graphics.GraphicsDevice.Viewport.Width, _graphics.GraphicsDevice.Viewport.Height);
                mainSimFrame.UpdatePaintBrushPosition(_graphics.GraphicsDevice.Viewport.Width - _graphics.GraphicsDevice.Viewport.Width / 10);
                speedSlider.Position = (_graphics.GraphicsDevice.Viewport.Width - _graphics.GraphicsDevice.Viewport.Width / 10, speedSlider.Position.Y);
                speedSlider.RefreshPositions();
                _graphics.ApplyChanges();
            };
            
            _renderers.Add(mainSimFrame);
            
            _renderers.Add(speedSlider);
            
            _world.ApplyEntityBuffers();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SimFrame.EntityTexture = Content.Load<Texture2D>("Entities/Entity");
            SimFrame.AntTexture = Content.Load<Texture2D>("Entities/Ant");
            SimFrame.ColonyTexture = Content.Load<Texture2D>("Entities/Colony");
            SimFrame.CircleTexture = Content.Load<Texture2D>("Entities/Circle");
            
            Button.DefaultTexture = Content.Load<Texture2D>("UIElements/Button");
            SpeedSlider.SpeedSliderSpriteSheet = Content.Load<Texture2D>("UIElements/SpeedSliderButtonSpriteSheet");
            PaintBrushSelection.PaintBrushSpriteSheet = Content.Load<Texture2D>("UIElements/PaintBrushButtonSpriteSheet");
            TextLabel.Font = Content.Load<SpriteFont>("UIElements/TextFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            
            if (!_isPaused)
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
            _isPaused = isPaused;
            TargetTps = _defaultTargetTps * newSpeed;
        }
    }
}