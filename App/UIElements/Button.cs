using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.UIElements
{
    /// <summary>
    ///     Represents a render-able and clickable button.
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

        // TODO : Add color to UIElement so that all rendered elements will have Color property.
        public Color Color { get; set; } = Color.White;
        
        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            base.Render(spriteBatch, gdm, canvasOffset);

            Rectangle spritePos = new Rectangle(
                Position.X,
                Position.Y,
                Size.Width,
                Size.Height);
            if (SpriteRectangle == Rectangle.Empty)
                spriteBatch.Draw(Texture ?? DefaultTexture, spritePos, Color);
            else
                spriteBatch.Draw(Texture ?? DefaultTexture, spritePos, SpriteRectangle, Color);
        }
    }
}