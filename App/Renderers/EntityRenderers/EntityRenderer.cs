using System;
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

            float worldAspectRatio = Entity.World.Size.X / Entity.World.Size.Y;
            float simFrameAspectRatio = (float) canvasOffset.Width / canvasOffset.Height;

            int newWorldWidth = canvasOffset.Width;
            int newWorldHeight = canvasOffset.Height;
            float scale = canvasOffset.Width / Entity.World.Size.X;

            if (worldAspectRatio > simFrameAspectRatio)
            {
                newWorldHeight = (int) (Entity.World.Size.X * canvasOffset.Height / canvasOffset.Width);
                scale = canvasOffset.Width / Entity.World.Size.X;
            }
            else if (worldAspectRatio < simFrameAspectRatio)
            {
                newWorldWidth = (int) (Entity.World.Size.Y * canvasOffset.Width / canvasOffset.Height);
                scale = canvasOffset.Height / Entity.World.Size.Y;
            }

            int posX = (int) (Entity.Transform.Position.X / Entity.World.Size.X * newWorldWidth);
            int posY = (int) (Entity.Transform.Position.Y / Entity.World.Size.Y * newWorldHeight);

            int scaleX = (int) MathF.Round(Entity.Transform.Scale.X * scale);
            int scaleY = (int) MathF.Round(Entity.Transform.Scale.Y * scale);

            Rectangle spritePos = new Rectangle(
                canvasOffset.Left + posX,
                canvasOffset.Top + canvasOffset.Height - posY,
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