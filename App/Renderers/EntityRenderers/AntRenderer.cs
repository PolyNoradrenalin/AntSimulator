using AntEngine.Entities;
using AntEngine.Entities.Ants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers
{
    /// <summary>
    /// Renderer for an Ant Entity.
    /// </summary>
    public class AntRenderer : EntityRenderer
    {
        public new static Texture2D entityCharset; 
        
        public AntRenderer(Ant e) : base(e)
        {
        }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm)
        {
            base.Render(spriteBatch, gdm);
        }
    }
}