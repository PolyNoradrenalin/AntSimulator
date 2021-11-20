using AntEngine.Entities.Ants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers.EntityRenderers
{
    /// <summary>
    /// Renderer for an Ant Entity.
    /// </summary>
    public class AntRenderer : EntityRenderer
    {
        public AntRenderer(Ant e, Texture2D antCharset) : base(e, antCharset)
        {
        }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
           base.Render(spriteBatch, gdm, canvasOffset);
        }
    }
}