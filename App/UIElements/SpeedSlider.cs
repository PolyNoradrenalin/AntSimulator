using System;
using System.Dynamic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App.UIElements
{
    public class SpeedSlider : UIElement
    {
        private bool _isPaused = true;
        
        private const int DecreaseButtonOffset = -32;
        private const int IncreaseButtonOffset = 32;

        private readonly Rectangle PauseButtonPosition = new Rectangle(0, 0 * 32, 32, 32);
        private readonly Rectangle IncreaseButtonPosition = new Rectangle(0, 1 * 32, 32, 32);
        private readonly Rectangle DecreaseButtonPosition = new Rectangle(0, 2 * 32, 32, 32);
        private readonly Rectangle PlayButtonPosition = new Rectangle(0, 3 * 32, 32, 32);

        public SpeedSlider(Rectangle posRect, int minimumSpeedValue, int maximumSpeedValue) : base(posRect)
        {
            SpeedValueLimit = (minimumSpeedValue, maximumSpeedValue);
            SpeedValue = 0;
            DecreaseButton = new Button(new Rectangle(posRect.Left + DecreaseButtonOffset, posRect.Top, posRect.Width, posRect.Height));
            PauseButton = new Button(posRect);
            IncreaseButton = new Button(new Rectangle(posRect.Left + IncreaseButtonOffset, posRect.Top, posRect.Width, posRect.Height));

            DecreaseButton.Texture = SpeedSliderSpriteSheet;
            PauseButton.Texture = SpeedSliderSpriteSheet;
            IncreaseButton.Texture = SpeedSliderSpriteSheet;
            
            DecreaseButton.SpriteRectangle = DecreaseButtonPosition;
            PauseButton.SpriteRectangle = PauseButtonPosition;
            IncreaseButton.SpriteRectangle = IncreaseButtonPosition;
            
            DecreaseButton.MouseReleased += OnDecreaseMouseRelease;
            PauseButton.MouseReleased += OnPauseMouseRelease;
            IncreaseButton.MouseReleased += OnIncreaseMouseRelease;
        }

        public event Action<int, bool> SpeedChange;

        public static Texture2D SpeedSliderSpriteSheet { get; set; }

        public int SpeedValue { get; set; }

        public (int Minimum, int Maximum) SpeedValueLimit { get; set; }

        public Button DecreaseButton { get; set; }

        public Button PauseButton { get; set; }

        public Button IncreaseButton { get; set; }

        private void OnDecreaseMouseRelease(MouseState arg1, UIElement arg2, Rectangle rectangle)
        {
            int newSpeedValue = SpeedValue / 2;

            if (newSpeedValue >= SpeedValueLimit.Minimum)
            {
                SpeedValue = newSpeedValue;
                
                SpeedChange?.Invoke(SpeedValue, _isPaused);
            }
        }

        private void OnPauseMouseRelease(MouseState arg1, UIElement arg2, Rectangle rectangle)
        {
            _isPaused = !_isPaused;
            PauseButton.SpriteRectangle = _isPaused ? PlayButtonPosition : PauseButtonPosition;
            SpeedChange?.Invoke(SpeedValue, _isPaused);
        }

        private void OnIncreaseMouseRelease(MouseState arg1, UIElement arg2, Rectangle rectangle)
        {
            int newSpeedValue = SpeedValue * 2;

            if (newSpeedValue <= SpeedValueLimit.Maximum)
            {
                SpeedValue = newSpeedValue;
                SpeedChange?.Invoke(SpeedValue, _isPaused);
            }
        }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            DecreaseButton.Render(spriteBatch, gdm, canvasOffset);
            PauseButton.Render(spriteBatch, gdm, canvasOffset);
            IncreaseButton.Render(spriteBatch, gdm, canvasOffset);
        }
    }
}