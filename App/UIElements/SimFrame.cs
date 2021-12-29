using System;
using System.Collections.Generic;
using System.Linq;
using AntEngine;
using AntEngine.Colliders;
using AntEngine.Entities;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Colonies;
using App.Renderers;
using App.Renderers.EntityRenderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App.UIElements
{
    /// <summary>
    ///     Represents the frame in which a simulation will occur.
    /// </summary>
    public class SimFrame : UIElement
    {
        public static Texture2D EntityTexture;
        public static Texture2D AntTexture;
        public static Texture2D ColonyTexture;

        private readonly IList<IRenderer> _renderers;

        public SimFrame(Rectangle rect, World world) : base(rect)
        {
            _renderers = new List<IRenderer>();
            SimWorld = world;
            world.EntityAdded += OnEntityAdded;
            world.EntityRemoved += OnEntityRemoved;
            MouseHeld += OnMouseHeld;

            WorldRenderer worldRenderer = new WorldRenderer(world.Collider, EntityTexture);
            _renderers.Add(worldRenderer);
            // TODO: Unsubscribe to allow GC.
        }

        private void OnMouseHeld(MouseState mouseState, UIElement arg2, Rectangle canvasOffset)
        {
            (int worldPixelWidth, int worldPixelHeight) = WorldRenderer.WorldPixelSize(canvasOffset, SimWorld.Size);
            
            (float mouseX, float mouseY) = mouseState.Position.ToVector2();
            (float relativeX, float relativeY) = (mouseX - Position.X, Size.Height - (mouseY - Position.Y));
            (float simX, float simY) = (SimWorld.Size.X * (relativeX / worldPixelWidth), SimWorld.Size.Y * (relativeY / worldPixelHeight));
            (int worldDivX, int worldDivY) = (
                (int) MathF.Round(relativeX / worldPixelWidth * SimWorld.Collider.Subdivision),
                (int) MathF.Round(relativeY / worldPixelHeight * SimWorld.Collider.Subdivision));

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (0 <= worldDivX && worldDivX < World.WorldColliderDivision &&
                    0 <= worldDivY && worldDivY < World.WorldColliderDivision)
                {
                    SimWorld.Collider.Matrix[worldDivY][worldDivX] = true;
                }
            }
        }

        public World SimWorld { get; }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            base.Render(spriteBatch, gdm, canvasOffset);
            
            foreach (IRenderer r in _renderers)
            {
                r.Render(spriteBatch, gdm,
                    new Rectangle(Position.X + canvasOffset.Left, Position.Y + canvasOffset.Top, Size.Width, Size.Height));
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
            foreach (IRenderer renderer in _renderers.ToList())
            {
                if (!(renderer is EntityRenderer entityRenderer)) continue;

                if (entityRenderer.Entity.Equals(entity)) RemoveRenderer(renderer);
            }
        }
    }
}