using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App.UIElements
{
    /// <summary>
    /// Allows for the selection of an element to be used as a paint brush.
    /// </summary>
    public class PaintBrushSelection : UIElement
    {
        public enum PaintBrushState 
        {
            Wall,
            Colony,
            Food
        }

        private const float _brightnessMultiplier = (100 - 70) / 100F;
        
        private readonly Rectangle _wallButtonPosition = new Rectangle(0, 0 * 32, 32, 32);
        private readonly Rectangle _colonyButtonPosition = new Rectangle(0, 1 * 32, 32, 32);
        private readonly Rectangle _foodButtonPosition = new Rectangle(0, 2 * 32, 32, 32);
        
        public PaintBrushSelection(Rectangle posRect, PaintBrushState initialState) : base(posRect)
        {
            WallButton = new Button(new Rectangle(posRect.Left, posRect.Top - posRect.Height / 3, posRect.Width, posRect.Height / 3));
            ColonyButton = new Button(new Rectangle(posRect.Left, posRect.Top, posRect.Width, posRect.Height / 3));
            FoodButton = new Button(new Rectangle(posRect.Left, posRect.Top + posRect.Height / 3, posRect.Width, posRect.Height / 3));

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

        private void OnWallMouseRelease(MouseState arg1, UIElement arg2, Rectangle arg3)
        {
            PaintBrushStateChange?.Invoke(PaintBrushState.Wall);
            SetSelectedState(PaintBrushState.Wall);
        }

        private void OnColonyMouseRelease(MouseState arg1, UIElement arg2, Rectangle arg3)
        {
            PaintBrushStateChange?.Invoke(PaintBrushState.Colony);
            SetSelectedState(PaintBrushState.Colony);
        }

        private void OnFoodMouseRelease(MouseState arg1, UIElement arg2, Rectangle arg3)
        {
            PaintBrushStateChange?.Invoke(PaintBrushState.Food);
            SetSelectedState(PaintBrushState.Food);
        }

        public event Action<PaintBrushState> PaintBrushStateChange;
        
        public static Texture2D PaintBrushSpriteSheet { get; set; }

        public Button WallButton { get; set; }

        public Button ColonyButton { get; set; }

        public Button FoodButton { get; set; }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            WallButton.Render(spriteBatch, gdm, canvasOffset);
            ColonyButton.Render(spriteBatch, gdm, canvasOffset);
            FoodButton.Render(spriteBatch, gdm, canvasOffset);
        }

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