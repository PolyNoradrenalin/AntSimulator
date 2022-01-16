using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.UIElements
{
    public class TextLabel : UIElement
    {
        public TextLabel(Rectangle rectangle) : base(rectangle)
        {
        }

        public string Text { get; set; }

        public static SpriteFont Font { get; set; }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            base.Render(spriteBatch, gdm, canvasOffset);

            Vector2 position = new Vector2(Position.X, Position.Y);

            spriteBatch.DrawString(Font, Text, position, Color.Black);
        }
    }
}