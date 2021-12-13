using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers
{
    /// <summary>
    ///     Interface for a renderer
    /// </summary>
    public interface IRenderer
    {
        void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset);
    }
}