using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Maths;
using Xunit;

namespace Tests.Engine
{
    public class CircleColliderTests
    {
        public CircleColliderTests()
        {
            WorldCollider = new WorldCollider(new Transform(), new Transform(), Vector2.One * 1000, 1000);
        }

        private WorldCollider WorldCollider { get; set; }
        
        [Theory]
        [InlineData(500, 500, 10)]
        [InlineData(5.5, 50, 10)]
        [InlineData(5.5, 5.5, 10)]
        private void CircleWorld_EmptyWorldAndCircleInBounds_ShouldNotCollide(float posX, float posY, float radius)
        {
            CircleCollider circle = new CircleCollider(new Transform(new Vector2(posX, posY), 0, Vector2.One * radius),
                new Transform());
            
            Assert.False(circle.checkCollision(WorldCollider));
        }
        
        [Theory]
        [InlineData(0, 50, 10)]
        [InlineData(-500, 0, 10)]
        [InlineData(10, 5000, 20)]
        [InlineData(10, 500, 20)]
        [InlineData(500, 10, 20)]
        private void CircleWorld_EmptyWorldAndCircleOutOfBounds_ShouldCollide(float posX, float posY, float radius)
        {
            CircleCollider circle = new(new Transform(new Vector2(posX, posY), 0, Vector2.One * radius),
                new Transform());
            
            Assert.True(circle.checkCollision(WorldCollider));
        }
        
        [Theory]
        [InlineData(399, 399, 100)]
        [InlineData(399, 500, 100)]
        [InlineData(500, 399, 100)]
        [InlineData(500+399, 500, 100)]
        [InlineData(500, 500+399, 100)]
        private void CircleWorld_OnePixelWorldAndCircleDontTouch_ShouldNotCollide(float posX, float posY, float radius)
        {
            CircleCollider circle = new(new Transform(new Vector2(posX, posY), 0, Vector2.One * radius),
                new Transform());
         
            WorldCollider.SetPixel(500, 500, true);
            
            Assert.False(circle.checkCollision(WorldCollider));
        }
        
        [Theory]
        [InlineData(450, 450, 100)]
        [InlineData(450, 500, 100)]
        [InlineData(500, 500, 100)]
        private void CircleWorld_OnePixelWorldAndCircleTouch_ShouldCollide(float posX, float posY, float radius)
        {
            CircleCollider circle = new(new Transform(new Vector2(posX, posY), 0, Vector2.One * radius),
                new Transform());
         
            WorldCollider.SetPixel(500, 500, true);
            
            Assert.True(circle.checkCollision(WorldCollider));
        }
    }
}