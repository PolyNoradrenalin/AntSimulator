using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App.UIElements
{
    /// <summary>
    ///     Allows for the selection of an element to be used as a paint brush.
    /// </summary>
    public class PaintBrushSelection : UIElement
    {
        /// <summary>
        ///     Represents the possible selection states. 
        /// </summary>
        public enum PaintBrushState
        {
            Wall,
            Colony,
            Food
        }
        
        // Brightness to be applied when a brush is not selected.
        private const float _brightnessMultiplier = (100 - 70) / 100F;
        
        // Position of textures in spritesheet.
        private readonly Rectangle _colonyButtonPosition = new Rectangle(0, 1 * 32, 32, 32);
        private readonly Rectangle _foodButtonPosition = new Rectangle(0, 2 * 32, 32, 32);
        private readonly Rectangle _wallButtonPosition = new Rectangle(0, 0 * 32, 32, 32);
        private readonly Rectangle _clearButtonPosition = new Rectangle(0, 0 * 32, 32, 32);

        public PaintBrushSelection(Rectangle posRect, PaintBrushState initialState) : base(posRect)
        {
            WallButton = new Button(new Rectangle(posRect.Left, posRect.Top - posRect.Height / 3, posRect.Width,
                posRect.Height / 3));
            ColonyButton = new Button(new Rectangle(posRect.Left, posRect.Top, posRect.Width, posRect.Height / 3));
            FoodButton = new Button(new Rectangle(posRect.Left, posRect.Top + posRect.Height / 3, posRect.Width,
                posRect.Height / 3));
            

            WallButton.Texture = PaintBrushSpriteSheet;
            ColonyButton.Texture = PaintBrushSpriteSheet;
            FoodButton.Texture = PaintBrushSpriteSheet;

            WallButton.SpriteRectangle = _wallButtonPosition;
            ColonyButton.SpriteRectangle = _colonyButtonPosition;
            FoodButton.SpriteRectangle = _foodButtonPosition;

            WallButton.MouseReleased += OnWallMouseRelease;
            ColonyButton.MouseReleased += OnColonyMouseRelease;
            FoodButton.MouseReleased += OnFoodMouseRelease;
            

            SetSelectedState(initialState);
        }

        public static Texture2D PaintBrushSpriteSheet { get; set; }

        public Button WallButton { get; set; }

        public Button ColonyButton { get; set; }

        public Button FoodButton { get; set; }

        /// <summary>
        ///     Activates when the wall brush is clicked.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        private void OnWallMouseRelease(MouseState arg1, UIElement arg2, Rectangle arg3)
        {
            PaintBrushStateChange?.Invoke(PaintBrushState.Wall);
            SetSelectedState(PaintBrushState.Wall);
        }

        /// <summary>
        ///     Activates when the colony brush is clicked.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        private void OnColonyMouseRelease(MouseState arg1, UIElement arg2, Rectangle arg3)
        {
            PaintBrushStateChange?.Invoke(PaintBrushState.Colony);
            SetSelectedState(PaintBrushState.Colony);
        }

        /// <summary>
        ///     Activates when the food brush is clicked.
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        private void OnFoodMouseRelease(MouseState arg1, UIElement arg2, Rectangle arg3)
        {
            PaintBrushStateChange?.Invoke(PaintBrushState.Food);
            SetSelectedState(PaintBrushState.Food);
        }

        /// <summary>
        ///     Represents the StateChange of the PaintBrushSelection.
        /// </summary>
        public event Action<PaintBrushState> PaintBrushStateChange;

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            WallButton.Render(spriteBatch, gdm, canvasOffset);
            ColonyButton.Render(spriteBatch, gdm, canvasOffset);
            FoodButton.Render(spriteBatch, gdm, canvasOffset);
        }

        /// <summary>
        ///     Refreshes the positions of all elements in this object.
        ///     Used when the position of this object changes.
        /// </summary>
        public void RefreshPositions()
        {
            WallButton.Position = (Position.X, Position.Y - Size.Height / 3);
            WallButton.Size = (Size.Width, Size.Height / 3);

            ColonyButton.Position = (Position.X, Position.Y);
            ColonyButton.Size = (Size.Width, Size.Height / 3);

            FoodButton.Position = (Position.X, Position.Y + Size.Height / 3);
            FoodButton.Size = (Size.Width, Size.Height / 3); 
        }

        /// <summary>
        ///     Sets current state of brush.
        ///     Will make non-selected buttons more opaque.
        /// </summary>
        /// <param name="state">New brush state</param>
        private void SetSelectedState(PaintBrushState state)
        {
            switch (state)
            {
                case PaintBrushState.Wall:
                    WallButton.Color = Color.White;
                    ColonyButton.Color = Color.White * _brightnessMultiplier;
                    FoodButton.Color = Color.White * _brightnessMultiplier;
                    break;
                case PaintBrushState.Colony:
                    WallButton.Color = Color.White * _brightnessMultiplier;
                    ColonyButton.Color = Color.White;
                    FoodButton.Color = Color.White * _brightnessMultiplier;
                    break;
                case PaintBrushState.Food:
                    WallButton.Color = Color.White * _brightnessMultiplier;
                    ColonyButton.Color = Color.White * _brightnessMultiplier;
                    FoodButton.Color = Color.White;
                    break;
            }
        }
    }
}