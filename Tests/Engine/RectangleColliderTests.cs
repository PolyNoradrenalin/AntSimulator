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
        public void GetVerticesWithoutRotation()
        {
            RectangleCollider rect1 = new RectangleCollider(new Transform(), new Transform());

            List<Vector2> vertices = new List<Vector2>
            {
                new Vector2(1,1),
                new Vector2(-1,1),
                new Vector2(1,-1),
                new Vector2(-1,-1)
            };

            List<Vector2> retVertices = rect1.GetVertices();
            
            Assert.Equal(vertices, retVertices);
        }

        [Fact]
        public void GetVerticesWithRotation()
        {
            RectangleCollider rect1 = new RectangleCollider(new Transform(), new Transform());

            List<Vector2> vertices = new List<Vector2>
            {
                new Vector2(1,1),
                new Vector2(-1,1),
                new Vector2(1,-1),
                new Vector2(-1,-1)
            };

            List<Vector2> retVertices = rect1.GetVertices();
            
            Assert.Equal(vertices, retVertices);
        }

        
        [Fact]
        public void NonCollidingRectanglesWithoutRotation()
        {
            RectangleCollider col1 = new(new Transform(), new Transform());
            RectangleCollider col2 = new(new Transform(), new Transform());

            col1.ParentTransform.Position = new Vector2(-100, 0);
            col2.ParentTransform.Position = new Vector2(100, 0);

            Assert.False(col1.checkCollision(col2));
        }
    }
}