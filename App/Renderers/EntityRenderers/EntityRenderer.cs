﻿using System;
using AntEngine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers.EntityRenderers
{
    /// <summary>
    ///     Represents the renderer for an abstract Entity class.
    /// </summary>
    public class EntityRenderer : IRenderer
    {
        private const float DefaultDepthBuffer = 0.5F;

        private readonly float _depthBuffer;

        public EntityRenderer(Entity e, Texture2D entityCharset, float depthBuffer = DefaultDepthBuffer)
        {
            Entity = e;
            EntityCharset = entityCharset;
            _depthBuffer = depthBuffer;
        }

        public Entity Entity { get; }

        public Texture2D EntityCharset { get; set; }

        public virtual void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            if (EntityCharset == null) return;

            (int worldPixelWidth, int worldPixelHeight) = WorldRenderer.WorldPixelSize(canvasOffset, Entity.World.Size);

            int posX = (int) (Entity.Transform.Position.X / Entity.World.Size.X * worldPixelWidth);
            int posY = (int) (Entity.Transform.Position.Y / Entity.World.Size.Y * worldPixelHeight);

            float scale = worldPixelWidth / Entity.World.Size.X;
            
            int scaleX = (int) MathF.Round(Entity.Transform.Scale.X * scale);
            int scaleY = (int) MathF.Round(Entity.Transform.Scale.Y * scale);

            Rectangle spritePos = new Rectangle(
                canvasOffset.Left + posX,
                canvasOffset.Top + worldPixelHeight - posY,
                scaleX,
                scaleY);

            spriteBatch.Draw(EntityCharset,
                spritePos,
                null,
                Color.White,
                -Entity.Transform.Rotation,
                new Vector2(EntityCharset.Width, EntityCharset.Height) / 2f,
                SpriteEffects.None,
                _depthBuffer);
        }
    }
}