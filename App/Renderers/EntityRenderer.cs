using AntEngine.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers
{
    /// <summary>
    /// Represents the renderer for an abstract Entity class.
    /// </summary>
    public class EntityRenderer : IRenderer
    {
        protected Entity entity;
        
        public EntityRenderer(Entity e)
        {
            entity = e;
        }
        public virtual void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm)
        {
            // TODO: Implement
        }
    }
}