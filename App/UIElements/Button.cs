using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App.UIElements
{
    /// <summary>
    /// Represents a renderable and clickable button.
    /// </summary>
    public class Button : UIElement
    {
        public static Texture2D Texture { get; set; }
        
        public Color Color { get; set; }

        public Button(Rectangle rect) : base(rect)
        { }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            base.Render(spriteBatch, gdm, canvasOffset);
            
            Rectangle spritePos = new Rectangle(
                Position.X, 
                Position.Y,
                Size.Width,
                Size.Height);
            
            spriteBatch.Draw(Texture, spritePos, Color);
        }
    }
}