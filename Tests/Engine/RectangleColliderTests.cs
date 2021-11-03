using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Utils.Maths;
using Xunit;

namespace Tests.Engine
{
    public class RectangleColliderTests
    {
        private WorldCollider worldCollider;
        
        public RectangleColliderTests()
        {
            worldCollider = new WorldCollider(new Transform(), new Transform(), Vector2.One * 1000, 1000);
        }
        
        [Fact]
        public void CheckCollision_WithoutRotation_ShouldNotCollide()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-100, 0);
            col2.ParentTransform.Position = new Vector2(100, 0);

            Assert.False(col1.checkCollision(col2));
        }
        
        [Fact]
        public void CheckCollision_ParentRotated_ShouldNotCollide()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-100, 0);
            col1.ParentTransform.Rotation = MathF.PI * 0.5f;
            
            col2.ParentTransform.Position = new Vector2(100, 0);

            Assert.False(col1.checkCollision(col2));
        }       
        
        [Fact]
        public void CheckCollision_ChildRotated_ShouldNotCollide()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-100, 0);
            col1.ColliderTransform.Rotation = MathF.PI * 0.5f;
            
            col2.ParentTransform.Position = new Vector2(100, 0);

            Assert.False(col1.checkCollision(col2));
        }
        
        [Fact]
        public void CheckCollision_WithoutRotation_ShouldCollide()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-1, 0);
            col1.ParentTransform.Scale = new Vector2(10, 1);
            
            col2.ParentTransform.Position = new Vector2(1, 0);
            
            Assert.True(col1.checkCollision(col2));
        }
        
        [Fact]
        public void CheckCollision_ParentRotated_ShouldCollideOnlyOnRotation()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-1, 0);
            col2.ParentTransform.Position = new Vector2(-1, -2);
            col1.ParentTransform.Scale = new Vector2(10, 1);
            Assert.False(col1.checkCollision(col2));
            
            col1.ParentTransform.Rotation = MathF.PI * 0.5f;
            Assert.True(col1.checkCollision(col2));
            
        }
        
        [Fact]
        public void CheckCollision_ChildRotated_ShouldCollide()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-1, 0);
            col1.ColliderTransform.Rotation = MathF.PI * 0.5f;
            col1.ParentTransform.Scale = new Vector2(10, 1);

            col2.ParentTransform.Position = new Vector2(1, 0);


            Assert.True(col1.checkCollision(col2));
        }

        [Theory]
        [InlineData(100, 100)]
        [InlineData(998, 998)]
        private void CheckCollision_EmptyWorld_ShouldNotCollide(float width, float height)
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            col1.ColliderTransform.Position = new Vector2(500, 500);
            col1.ColliderTransform.Scale = new Vector2(width, height);

            Assert.False(col1.checkCollision(worldCollider));
        }
        
        [Theory]
        [InlineData(100, 100)]
        [InlineData(500, 500)]
        private void CheckCollision_RotatedAndEmptyWorld_ShouldNotCollide(float width, float height)
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            col1.ColliderTransform.Position = new Vector2(500, 500);
            col1.ColliderTransform.Scale = new Vector2(width, height);
            col1.ColliderTransform.Rotation = MathF.PI / 2f;
            
            Assert.False(col1.checkCollision(worldCollider));
        }
        
        [Theory]
        [InlineData(998, 998, MathF.PI / 4F)]
        private void CheckCollision_RotatedAndEmptyWorld_ShouldCollideOnlyRotated(float width, float height, float angle)
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            col1.ColliderTransform.Position = new Vector2(500, 500);
            col1.ColliderTransform.Scale = new Vector2(width, height);
            
            Assert.False(col1.checkCollision(worldCollider));
            
            col1.ColliderTransform.Rotation = angle;
            
            Assert.True(col1.checkCollision(worldCollider));
        }
        
        [Theory]
        [InlineData(998, 998)]
        [InlineData(3, 998)]
        [InlineData(998, 3)]
        private void CheckCollision_NotEmptyWorld_ShouldCollide(float width, float height)
        {
            worldCollider.SetPixel(500, 500, true);
            
            RectangleCollider col1 = new(new Transform(), new Transform());
            col1.ColliderTransform.Position = new Vector2(500, 500);
            col1.ColliderTransform.Scale = new Vector2(width, height);
            
            Assert.True(col1.checkCollision(worldCollider));
        }
        
        [Theory]
        [InlineData(500, 500, MathF.PI / 4F)]
        [InlineData(800, 10, MathF.PI / 2F)]
        private void CheckCollision_NotEmptyWorld_ShouldCollideOnlyRotated(float width, float height, float angle)
        {
            worldCollider.SetPixel(500, 755, true);
            
            RectangleCollider col1 = new(new Transform(), new Transform());
            col1.ColliderTransform.Position = new Vector2(500, 500);
            col1.ColliderTransform.Scale = new Vector2(width, height);
            
            Assert.False(col1.checkCollision(worldCollider));
            
            col1.ColliderTransform.Rotation = angle;
            
            Assert.True(col1.checkCollision(worldCollider));
        }
    }
}