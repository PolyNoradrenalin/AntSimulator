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

        private Texture2D entityTexture;
        private Texture2D antTexture;
        private Texture2D colonyTexture;
        
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

            Ant a = new Ant("EntityTest", new Transform(new Vector2(50, 50), 0, new Vector2(30, 30)), world);

            mainSimFrame.AddRenderer(new EntityRenderer(a, entityTexture));

            Ant b = new Ant("AntTest", new Transform(new Vector2(150, 150), 0, new Vector2(20, 20)), world);

            mainSimFrame.AddRenderer(new AntRenderer(b, antTexture));

            Colony c = new Colony("ColonyTest", new Transform(new Vector2(300, 170), 0, new Vector2(64, 64)), world,
                (s, t, w, c) => new Ant(world));

            mainSimFrame.AddRenderer(new ColonyRenderer(c, colonyTexture));

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            entityTexture = Content.Load<Texture2D>("Entities/Entity");
            antTexture = Content.Load<Texture2D>("Entities/Ant");
            colonyTexture = Content.Load<Texture2D>("Entities/Colony");

            ((EntityRenderer) ((SimFrame) renderers[0]).GetRenderer(0)).EntityCharset = entityTexture;
            ((EntityRenderer) ((SimFrame) renderers[0]).GetRenderer(1)).EntityCharset = antTexture;
            ((EntityRenderer) ((SimFrame) renderers[0]).GetRenderer(2)).EntityCharset = colonyTexture;
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