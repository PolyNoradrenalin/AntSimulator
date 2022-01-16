using System;
using AntEngine.Colliders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Vector2 = System.Numerics.Vector2;

namespace App.Renderers
{
    public class WorldRenderer : IRenderer
    {
        private const int BorderWidth = 2;
        
        private readonly WorldCollider _worldCollider;

        public WorldRenderer(WorldCollider worldCollider, Texture2D tileTexture)
        {
            TileTexture = tileTexture;
            _worldCollider = worldCollider;
        }

        public Texture2D TileTexture { get; set; }

        public void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            // To draw the world, we display a sprite at each cell of the world collider when it is a wall.
            // The size (in pixels) of each cell is defined by the size of the canvas divided by the collider subdivision.
            // In the case the cell size have decimals (ex: 12.25px), we correct this by adding 1px to the first cells
            // until we corrected the offset.

            (int worldPixelWidth, int worldPixelHeight) = WorldPixelSize(canvasOffset, _worldCollider.Size);

            int posY = 0;
            for (int y = 0; y < _worldCollider.Subdivision; y++)
            {
                int posX = 0;
                int yTileSize = (int) MathF.Floor((float) worldPixelHeight / _worldCollider.Subdivision);
                int yCorrectionThreshold = (int) (_worldCollider.Subdivision *
                                                  ((float) worldPixelHeight / _worldCollider.Subdivision % 1));
                if (y < yCorrectionThreshold) yTileSize++;

                for (int x = 0; x < _worldCollider.Subdivision; x++)
                {
                    bool isWall = _worldCollider.Matrix[y][x];
                    int xTileSize = (int) MathF.Floor((float) worldPixelWidth / _worldCollider.Subdivision);
                    int xCorrectionThreshold = (int) (_worldCollider.Subdivision *
                                                      ((float) worldPixelWidth / _worldCollider.Subdivision % 1));
                    if (x < xCorrectionThreshold) xTileSize++;

                    if (isWall)
                    {
                        Rectangle destRectangle = new Rectangle(
                            canvasOffset.Left + posX,
                            canvasOffset.Bottom - posY - yTileSize,
                            xTileSize,
                            yTileSize);

                        spriteBatch.Draw(TileTexture, destRectangle, null, Color.Black, 0,
                            Microsoft.Xna.Framework.Vector2.Zero,
                            SpriteEffects.None, 0F);
                    }

                    posX += xTileSize;
                }

                posY += yTileSize;
            }
            // Drawing of the world's border
            Texture2D pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            pointTexture.SetData(new[]{Color.White});
            
            spriteBatch.Draw(pointTexture, new Rectangle(canvasOffset.X, canvasOffset.Y, BorderWidth, worldPixelHeight + BorderWidth), Color.Black);
            spriteBatch.Draw(pointTexture, new Rectangle(canvasOffset.X, canvasOffset.Y, worldPixelWidth + BorderWidth, BorderWidth), Color.Black);
            spriteBatch.Draw(pointTexture, new Rectangle(canvasOffset.X + worldPixelWidth, canvasOffset.Y, BorderWidth, worldPixelHeight + BorderWidth), Color.Black);
            spriteBatch.Draw(pointTexture, new Rectangle(canvasOffset.X, canvasOffset.Y + worldPixelHeight - BorderWidth, worldPixelWidth + BorderWidth, BorderWidth), Color.Black);
        }

        public static (int worldPixelWidth, int worldPixelHeight) WorldPixelSize(Rectangle canvasOffset,
            Vector2 worldSize)
        {
            float worldAspectRatio = worldSize.X / worldSize.Y;
            float simFrameAspectRatio = (float) canvasOffset.Width / canvasOffset.Height;

            int worldPixelWidth = (int) worldSize.X;
            int worldPixelHeight = (int) worldSize.Y;

            if (worldAspectRatio > simFrameAspectRatio)
            {
                // World width is the limiting dimension
                worldPixelWidth = (int) (worldPixelWidth * (float) canvasOffset.Width / (int) worldSize.X);
                worldPixelHeight = (int) (worldPixelHeight * (float) canvasOffset.Width / (int) worldSize.Y);
            }
            else if (worldAspectRatio < simFrameAspectRatio)
            {
                // World height is the limiting dimension
                worldPixelWidth = (int) (worldPixelWidth * (float) canvasOffset.Height / (int) worldSize.X);
                worldPixelHeight = (int) (worldPixelHeight * (float) canvasOffset.Height / (int) worldSize.Y);
            }

            return (worldPixelWidth, worldPixelHeight);
        }
    }
}