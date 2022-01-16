using System;
using App.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App.UIElements
{
    /// <summary>
    ///     Represents a UIElement that can be rendered.
    /// </summary>
    public abstract class UIElement : IRenderer
    {
        private MouseState _oldMouseState;

        protected UIElement(Rectangle rectangle)
        {
            Position = (rectangle.X, rectangle.Y);

            Size = (rectangle.Width, rectangle.Height);
        }

        /// <summary>
        ///     Position of the UIElement.
        /// </summary>
        public (int X, int Y) Position { get; set; }
        
        /// <summary>
        ///     Size of the UIElement.
        /// </summary>
        public (int Width, int Height) Size { get; set; }

        /// <summary>
        ///     True if this element is hovered by the mouse.
        /// </summary>
        public bool IsHovered { get; private set; }

        /// <summary>
        ///     Local texture of this UIElement.
        ///     UIElements must also contain static texture to serve as a fallback.
        /// </summary>
        public Texture2D Texture { get; set; } = null;

        /// <summary>
        ///     Used when a UIElement's texture is a spritesheet.
        ///     Specifies which part of the spritesheet to use (SourceRectangle).
        /// </summary>
        public Rectangle SpriteRectangle { get; set; } = Rectangle.Empty;

        public virtual void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            UpdateMouseStates(canvasOffset);
        }

        /// <summary>
        ///     Event triggered when this UIElement is released by a mouse.
        /// </summary>
        public event Action<MouseState, UIElement, Rectangle> MouseReleased;

        /// <summary>
        ///     Event triggered when this UIElement is being held by a mouse click.
        /// </summary>
        public event Action<MouseState, UIElement, Rectangle> MouseHeld;

        /// <summary>
        ///     Event triggered when this UIElement is pressed by a mouse.
        ///     A click is only considered when the mouse goes from released to pressed to released.
        /// </summary>
        public event Action<MouseState, UIElement, Rectangle> MousePressed;

        /// <summary>
        ///     Event triggered when this UIElement is hovered on by a mouse.
        /// </summary>
        public event Action<MouseState, UIElement, Rectangle> MouseEnter;

        /// <summary>
        ///     Event triggered when this UIElement is no longer hovered on by a mouse.
        /// </summary>
        public event Action<MouseState, UIElement, Rectangle> MouseExit;

        /// <summary>
        ///     Updates all mouse states and invokes the corresponding event(s) if needed.
        /// </summary>
        /// <param name="canvasOffset">Offset of local frame of reference compared to absolute coordinates</param>
        protected void UpdateMouseStates(Rectangle canvasOffset)
        {
            MouseState mouseState = Mouse.GetState();

            // Checks hover state
            bool newStateIsHovering = IsHovering(mouseState.Position - canvasOffset.Location);

            if (IsHovered && !newStateIsHovering)
            {
                IsHovered = false;
                MouseExit?.Invoke(mouseState, this, canvasOffset);
            }
            else if (!IsHovered && newStateIsHovering)
            {
                IsHovered = true;
                MouseEnter?.Invoke(mouseState, this, canvasOffset);
            }

            if (IsHovered)
            {
                switch (_oldMouseState.LeftButton)
                {
                    // Checks release state
                    case ButtonState.Pressed when mouseState.LeftButton == ButtonState.Released:
                        MouseReleased?.Invoke(mouseState, this, canvasOffset);
                        break;
                    // Checks pressed state
                    case ButtonState.Released when mouseState.LeftButton == ButtonState.Pressed:
                        MousePressed?.Invoke(mouseState, this, canvasOffset);
                        break;
                }

                // Checks held state
                if (mouseState.LeftButton == ButtonState.Pressed) MouseHeld?.Invoke(mouseState, this, canvasOffset);
            }

            _oldMouseState = mouseState;
        }

        /// <summary>
        ///     Checks if a point is contained inside this object's bounds (parent's frame of reference).
        /// </summary>
        /// <param name="point">Point to check hover of.</param>
        /// <returns>True if the point hovers, false otherwise</returns>
        protected bool IsHovering(Point point)
        {
            (int x, int y) = point;

            return x >= Position.X && x <= Position.X + Size.Width && y >= Position.Y && y <= Position.Y + Size.Height;
        }
    }
}