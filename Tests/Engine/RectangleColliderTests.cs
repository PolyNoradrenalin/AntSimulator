using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using AntEngine.Colliders;
using AntEngine.Maths;
using Xunit;

namespace Tests.Engine
{
    public class RectangleColliderTests
    {
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
        public void CheckCollision_ParentRotated_ShouldCollide()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-1, 0);
            col1.ParentTransform.Rotation = MathF.PI * 0.5f;
            col1.ParentTransform.Scale = new Vector2(10, 1);

            col2.ParentTransform.Position = new Vector2(1, 0);


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
    }
}