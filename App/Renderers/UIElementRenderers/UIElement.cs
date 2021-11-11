using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers
{
    /// <summary>
    /// Represents a UIElement that can be rendered.
    /// </summary>
    public abstract class UIElement : IRenderer
    {
        public abstract void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm);
    }
}