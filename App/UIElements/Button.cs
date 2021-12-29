using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.UIElements
{
    /// <summary>
    ///     Represents a renderable and clickable button.
    /// </summary>
    public class Button : UIElement
    {
        public Button(Rectangle rect) : base(rect)
        {
        }

        public Button(Rectangle rect, Texture2D buttonTexture) : base(rect)
        {
            Texture = buttonTexture;
        }

        public static Texture2D DefaultTexture { get; set; }

        public Texture2D Texture { get; set; } = null;

        public Color Color { get; set; }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            base.Render(spriteBatch, gdm, canvasOffset);

            Rectangle spritePos = new Rectangle(
                Position.X,
                Position.Y,
                Size.Width,
                Size.Height);

            spriteBatch.Draw(Texture ?? DefaultTexture, spritePos, Color);
        }
    }
}