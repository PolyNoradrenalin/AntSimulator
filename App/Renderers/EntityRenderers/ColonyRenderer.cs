using AntEngine.Entities.Colonies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers.EntityRenderers
{
    /// <summary>
    /// Renderer for a Colony Entity.
    /// </summary>
    public class ColonyRenderer : EntityRenderer
    {
        public new static Texture2D entityCharset;
        
        public ColonyRenderer(Colony e) : base(e)
        {
        }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm)
        {
            Rectangle spritePos = new Rectangle((int) entity.Transform.Position.X, (int) entity.Transform.Position.Y,
                (int) entity.Transform.Scale.X, (int) entity.Transform.Scale.Y);
            
            spriteBatch.Draw(entityCharset, spritePos, Color.White);
        }
    }
}