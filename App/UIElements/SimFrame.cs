using System;
using System.Collections.Generic;
using AntEngine;
using AntEngine.Entities;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using App.Renderers;
using App.Renderers.EntityRenderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.UIElements
{
    /// <summary>
    /// Represents the frame in which a simulation will occur.
    /// </summary>
    public class SimFrame : UIElement
    {
        public static Texture2D EntityTexture;
        public static Texture2D AntTexture;
        public static Texture2D ColonyTexture;
        
        private List<IRenderer> _renderers;

        public SimFrame(World world)
        {
            _renderers = new List<IRenderer>();
            SimWorld = world;
            world.EntityAdded += OnEntityAdded;
            world.EntityRemoved += OnEntityRemoved;
            
            // TODO: Unsubscribe to allow GC.
        }

        public World SimWorld { get; private set; }
        
        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            foreach (IRenderer r in _renderers)
            {
                r.Render(spriteBatch, gdm, new Rectangle(Position.X + canvasOffset.Left, Position.Y + canvasOffset.Top, Size.Width, Size.Height));
            }
        }

        public void AddRenderer(IRenderer r)
        {
            _renderers.Add(r);
        }
        
        public void RemoveRenderer(IRenderer r)
        {
            _renderers.Remove(r);
        }

        private void OnEntityAdded(Entity entity)
        {
            IRenderer renderer = entity switch
            {
                Ant ant => new AntRenderer(ant, AntTexture),
                Colony colony => new ColonyRenderer(colony, ColonyTexture),
                _ => new EntityRenderer(entity, EntityTexture)
            };

            AddRenderer(renderer);
        }
        
        private void OnEntityRemoved(Entity entity)
        {
            foreach (IRenderer renderer in _renderers)
            {
                if (!(renderer is EntityRenderer entityRenderer)) continue;
                
                if (entityRenderer.Entity.Equals(entity))
                {
                    RemoveRenderer(renderer);
                }
            }
        }
    }
}