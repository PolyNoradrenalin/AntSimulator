using System;
using AntEngine.Colliders;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace App.Renderers
{
    public class WorldRenderer : IRenderer
    {
        private WorldCollider _worldCollider;

        public WorldRenderer(WorldCollider worldCollider, Texture2D tileTexture)
        {
            TileTexture = tileTexture;
            _worldCollider = worldCollider;
        }

        public Texture2D TileTexture { get; set; }

        public void Render(SpriteBatch spriteBatch, GraphicsDeviceManager gdm, Rectangle canvasOffset)
        {
            int posY = 0;
            
            for (int y = 0; y < _worldCollider.Subdivision; y++)
            {
                int posX = 0;
                int yTileSize = (int) MathF.Floor((float) canvasOffset.Height / _worldCollider.Subdivision);
                int yCorrectionThreshold = (int) (_worldCollider.Subdivision * ((float) canvasOffset.Height / _worldCollider.Subdivision % 1));
                if (y < yCorrectionThreshold) yTileSize++;
                
                for (int x = 0; x < _worldCollider.Subdivision; x++)
                {
                    bool isWall = _worldCollider.Matrix[y][x];
                    int xTileSize = (int) MathF.Floor((float) canvasOffset.Width / _worldCollider.Subdivision);
                    int xCorrectionThreshold = (int) (_worldCollider.Subdivision * ((float) canvasOffset.Width / _worldCollider.Subdivision % 1));
                    if (x < xCorrectionThreshold) xTileSize++;
                    
                    if (isWall)
                    {
                        Rectangle destRectangle = new Rectangle(
                            canvasOffset.Left + posX - (int) MathF.Floor((float) canvasOffset.Width / _worldCollider.Subdivision),
                            canvasOffset.Bottom - posY,
                            xTileSize,
                            yTileSize);
                        
                        spriteBatch.Draw(TileTexture, destRectangle, null, Color.Black, 0, Vector2.Zero,
                            SpriteEffects.None, 0F);
                    }

                    posX += xTileSize;
                }

                posY += yTileSize;
            }
        }
    }
}