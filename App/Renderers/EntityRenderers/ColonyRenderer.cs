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
        public ColonyRenderer(Colony e, Texture2D colonyCharset) : base(e, colonyCharset)
        {
        }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm)
        {
            base.Render(spriteBatch, gdm);
        }
    }
}