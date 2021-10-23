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
        public void NonCollidingRectanglesWithoutRotation()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-100, 0);
            col2.ParentTransform.Position = new Vector2(100, 0);

            Assert.False(col1.checkCollision(col2));
        }
        
        [Fact]
        public void CollidingRectanglesWithoutRotation()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-1, 0);
            col2.ParentTransform.Position = new Vector2(1, 0);

            col1.ParentTransform.Scale = new Vector2(10, 1);

            Assert.True(col1.checkCollision(col2));
        }
    }
}