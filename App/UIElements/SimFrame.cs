﻿using System;
using System.Collections.Generic;
using System.Linq;
using AntEngine;
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

            // TODO: Unsubscribe to allow GC.
        }

        private void OnMouseHeld(MouseState mouseState, UIElement arg2)
        {
            (float mouseX, float mouseY) = mouseState.Position.ToVector2();
            (float relativeX, float relativeY) = (mouseX - Position.X, Size.Height - (mouseY - Position.Y));
            (float simX, float simY) = (SimWorld.Size.X * (relativeX / Size.Width), SimWorld.Size.Y * (relativeY / Size.Height));
            (int worldDivX, int worldDivY) = (
                (int) MathF.Round(relativeX / Size.Width * SimWorld.Collider.Subdivision),
                (int) MathF.Round(relativeY / Size.Height * SimWorld.Collider.Subdivision));

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                SimWorld.Collider.Matrix[worldDivY][worldDivX] = true;
            }
        }

        public World SimWorld { get; }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            base.Render(spriteBatch, gdm, canvasOffset);
            
            foreach (IRenderer r in _renderers)
            {
                r.Render(spriteBatch, gdm,
                    new Rectangle(Position.X + canvasOffset.Left, Position.Y + canvasOffset.Top, Size.Width,
                        Size.Height));
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