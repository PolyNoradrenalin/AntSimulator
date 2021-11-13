using AntEngine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace App.Renderers.EntityRenderers
{
    /// <summary>
    /// Represents the renderer for an abstract Entity class.
    /// </summary>
    public class EntityRenderer : IRenderer
    {
        protected readonly Entity entity;
        
        public EntityRenderer(Entity e, Texture2D entityCharset)
        {
            entity = e;
            EntityCharset = entityCharset;
        }
        
        public virtual void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm)
        {
            if (EntityCharset == null) return;
            Rectangle spritePos = new Rectangle((int) entity.Transform.Position.X, (int) entity.Transform.Position.Y,
                (int) entity.Transform.Scale.X, (int) entity.Transform.Scale.Y);
            
            spriteBatch.Draw(EntityCharset, spritePos, Color.White);
        }
        
        public Texture2D EntityCharset { get; set; } 
    }
}