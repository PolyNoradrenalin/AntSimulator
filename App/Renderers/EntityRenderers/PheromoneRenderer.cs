using AntEngine.Entities;
using AntEngine.Entities.Pheromones;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers.EntityRenderers
{
    /// <summary>
    /// Renderer for a Pheromone Entity.
    /// Degrades the color of the pheromone depending on it's intensity.
    /// </summary>
    public class PheromoneRenderer : EntityRenderer
    {
        private const float PheromoneDepthBuffer = 0F;
        
        public PheromoneRenderer(Pheromone e, Texture2D entityCharset, float depthBuffer = PheromoneDepthBuffer) : base(e, entityCharset, depthBuffer)
        {
        }

        public PheromoneRenderer(Pheromone e, Texture2D entityCharset, Color color, float depthBuffer = PheromoneDepthBuffer) : base(e, entityCharset, color, depthBuffer)
        {
        }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            base.Render(spriteBatch, gdm, canvasOffset);
        }
    }
}