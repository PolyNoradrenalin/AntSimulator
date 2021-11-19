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
        public EntityRenderer(Entity e, Texture2D entityCharset)
        {
            Entity = e;
            EntityCharset = entityCharset;
        }

        public Entity Entity { get; }

        public virtual void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm)
        {
            if (EntityCharset == null) return;
            Rectangle spritePos = new Rectangle((int) Entity.Transform.Position.X, (int) Entity.Transform.Position.Y,
                (int) Entity.Transform.Scale.X, (int) Entity.Transform.Scale.Y);
            
            spriteBatch.Draw(EntityCharset, spritePos, null, Color.White, Entity.Transform.Rotation, new Vector2(EntityCharset.Width, EntityCharset.Height)/2f, SpriteEffects.None,1);
        }
        
        public Texture2D EntityCharset { get; set; } 
    }
}