using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace App.UIElements
{
    /// <summary>
    ///     Speed slider allowing for the change of a game's speed via an event.
    ///     Speed limits are positive and should be between 1 and MAX_INT.
    ///     If the speed is too high, the system will be unable to go faster and the simulation will start lagging.
    /// </summary>
    public class SpeedSlider : UIElement
    {
        private readonly Rectangle DecreaseButtonPosition = new Rectangle(0, 2 * 32, 32, 32);
        private readonly Rectangle IncreaseButtonPosition = new Rectangle(0, 1 * 32, 32, 32);

        private readonly Rectangle PauseButtonPosition = new Rectangle(0, 0 * 32, 32, 32);
        private readonly Rectangle PlayButtonPosition = new Rectangle(0, 3 * 32, 32, 32);
        private bool _isPaused = true;

        public SpeedSlider(Rectangle posRect, int minimumSpeedValue, int maximumSpeedValue) : base(posRect)
        {
            SpeedValueLimit = (minimumSpeedValue, maximumSpeedValue);
            SpeedValue = 1;

            DecreaseButton = new Button(new Rectangle(posRect.Left - posRect.Width / 3, posRect.Top, posRect.Width / 3,
                posRect.Height));
            PauseButton = new Button(new Rectangle(posRect.Left, posRect.Top, posRect.Width / 3, posRect.Height));
            IncreaseButton = new Button(new Rectangle(posRect.Left + posRect.Width / 3, posRect.Top, posRect.Width / 3,
                posRect.Height));

            DecreaseButton.Texture = SpeedSliderSpriteSheet;
            PauseButton.Texture = SpeedSliderSpriteSheet;
            IncreaseButton.Texture = SpeedSliderSpriteSheet;

            DecreaseButton.SpriteRectangle = DecreaseButtonPosition;
            PauseButton.SpriteRectangle = PlayButtonPosition;
            IncreaseButton.SpriteRectangle = IncreaseButtonPosition;

            DecreaseButton.MouseReleased += OnDecreaseMouseRelease;
            PauseButton.MouseReleased += OnPauseMouseRelease;
            IncreaseButton.MouseReleased += OnIncreaseMouseRelease;

            SpeedLabel = new TextLabel(new Rectangle(posRect.Left, posRect.Top + posRect.Height, posRect.Width / 3,
                posRect.Height))
            {
                Text = SpeedValue.ToString()
            };
        }

        public static Texture2D SpeedSliderSpriteSheet { get; set; }

        public int SpeedValue { get; set; }

        public (int Minimum, int Maximum) SpeedValueLimit { get; set; }

        public Button DecreaseButton { get; set; }

        public Button PauseButton { get; set; }

        public Button IncreaseButton { get; set; }

        public TextLabel SpeedLabel { get; set; }

        public event Action<int, bool> SpeedChange;

        private void OnDecreaseMouseRelease(MouseState arg1, UIElement arg2, Rectangle rectangle)
        {
            int newSpeedValue = SpeedValue / 2;

            if (newSpeedValue >= SpeedValueLimit.Minimum)
            {
                SpeedValue = newSpeedValue;
                SpeedChange?.Invoke(SpeedValue, _isPaused);
                UpdateSpeedLabelText();
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
                UpdateSpeedLabelText();
            }
        }

        public override void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            DecreaseButton.Render(spriteBatch, gdm, canvasOffset);
            PauseButton.Render(spriteBatch, gdm, canvasOffset);
            IncreaseButton.Render(spriteBatch, gdm, canvasOffset);
            SpeedLabel.Render(spriteBatch, gdm, canvasOffset);
        }

        /// <summary>
        ///     Refreshes the positions of all elements in this object.
        ///     Used when the position of this object changes.
        /// </summary>
        public void RefreshPositions()
        {
            DecreaseButton.Position = (Position.X - Size.Width / 3, Position.Y);
            DecreaseButton.Size = (Size.Width / 3, Size.Height);

            PauseButton.Position = (Position.X, Position.Y);
            PauseButton.Size = (Size.Width / 3, Size.Height);

            IncreaseButton.Position = (Position.X + Size.Width / 3, Position.Y);
            IncreaseButton.Size = (Size.Width / 3, Size.Height);

            SpeedLabel.Position = (Position.X, Position.Y + Size.Height);
            SpeedLabel.Size = (Size.Width / 3, Size.Height);
        }

        private void UpdateSpeedLabelText()
        {
            SpeedLabel.Text = SpeedValue.ToString();
        }
    }
}