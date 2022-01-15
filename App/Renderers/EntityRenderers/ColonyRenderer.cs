using AntEngine.Entities.Colonies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers.EntityRenderers
{
    /// <summary>
    ///     Renderer for a Colony Entity.
    /// </summary>
    public class ColonyRenderer : EntityRenderer
    {
        private const float ColonyDepthBuffer = 0.7F;
        
        public ColonyRenderer(Colony e, Texture2D colonyCharset) : base(e, colonyCharset, ColonyDepthBuffer)
        {
        }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            base.Render(spriteBatch, gdm, canvasOffset);
        }
    }
}