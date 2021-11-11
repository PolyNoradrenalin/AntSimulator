using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers
{
    public class SimFrame : IRenderer
    {
        private List<IRenderer> renderers;

        public SimFrame()
        {
            renderers = new List<IRenderer>();
        }
        
        public void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm)
        {
            foreach (IRenderer r in renderers)
            {
                r.Render(spriteBatch, gdm);
            }
        }
    }
}