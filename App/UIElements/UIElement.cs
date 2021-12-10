using System;
using App.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App.UIElements
{
    /// <summary>
    /// Represents a UIElement that can be rendered.
    /// </summary>
    public abstract class UIElement : IRenderer
    {
        private MouseState _oldMouseState;

        public (int X, int Y) Position { get; set; }
        public (int Width, int Height) Size { get; set; }

        public bool IsHovered { get; private set; } = false;

        protected UIElement(Rectangle rectangle)
        {
            Position = (rectangle.X, rectangle.Y);

            Size = (rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// Event triggered when this UIElement is released by a mouse.
        /// </summary>
        public event Action<MouseState, UIElement> MouseReleased;

        /// <summary>
        /// Event triggered when this UIElement is being held by a mouse click.
        /// </summary>
        public event Action<MouseState, UIElement> MouseHeld;
        
        /// <summary>
        /// Event triggered when this UIElement is pressed by a mouse.
        /// A click is only considered when the mouse goes from released to pressed to released.
        /// </summary>
        public event Action<MouseState, UIElement> MousePressed;

        /// <summary>
        /// Event triggered when this UIElement is hovered on by a mouse.
        /// </summary>
        public event Action<MouseState, UIElement> MouseEnter;
        
        /// <summary>
        /// Event triggered when this UIElement is no longer hovered on by a mouse.
        /// </summary>
        public event Action<MouseState, UIElement> MouseExit;

        public virtual void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            UpdateMouseStates(canvasOffset);   
        }

        /// <summary>
        /// Updates all mouse states and invokes the corresponding event(s) if needed. 
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
                MouseExit?.Invoke(mouseState, this);
            } 
            else if (!IsHovered && newStateIsHovering)
            {
                IsHovered = true;
                MouseEnter?.Invoke(mouseState, this);
            }

            if (IsHovered)
            {
                switch (_oldMouseState.LeftButton)
                {
                    // Checks release state
                    case ButtonState.Pressed when mouseState.LeftButton == ButtonState.Released:
                        MouseReleased?.Invoke(mouseState, this);
                        break;
                    // Checks pressed state
                    case ButtonState.Released when mouseState.LeftButton == ButtonState.Pressed:
                        MousePressed?.Invoke(mouseState, this);
                        break;
                }

                // Checks held state
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    MouseHeld?.Invoke(mouseState, this);
                }
            }
            
            _oldMouseState = mouseState;
        }

        /// <summary>
        /// Checks if a point is contained inside this object's bounds (parent's frame of reference).
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