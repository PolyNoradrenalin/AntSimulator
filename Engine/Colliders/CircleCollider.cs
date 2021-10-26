using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using AntEngine.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    /// Collider in the shape of a circle.
    /// </summary>
    public class CircleCollider : Collider
    {
        public CircleCollider(Transform transform, Transform parentTransform) : base(transform, parentTransform) {}

        /// <summary>
        /// Radius of the circle. This is the maximum component of the scale vector. (we're not handling ellipses)
        /// </summary>
        public float Radius => MathF.Max(ColliderTransform.Scale.X, ColliderTransform.Scale.Y);

        public override bool checkCollision(CircleCollider circleCollider)
        {
            float distance = Vector2.Distance(ColliderTransform.Position, circleCollider.ColliderTransform.Position);
            return distance < Radius + circleCollider.Radius;
        }

        public override bool checkCollision(RectangleCollider rectCollider)
        {
            //TODO : Implement
            throw new System.NotImplementedException();
        }

        public override bool checkCollision(WorldCollider worldCollider)
        {
            // Get bounding rectangle pixels to check only the circle region on the world collider
            Transform rectTransform = new(ColliderTransform.Position, 0, ColliderTransform.Scale);
            IList<Vector2> rectVertices = rectTransform.GetRectangleVertices();

            float minXPos = rectVertices[2].X;
            float maxXPos = rectVertices[0].X;
            float minYPos = rectVertices[2].Y;
            float maxYPos = rectVertices[0].Y;
            
            // If the circle has a part outside the world that's a collision
            if (minXPos <= 0 || 
                maxXPos >= worldCollider.Size.X || 
                minYPos <= 0 || 
                maxYPos >= worldCollider.Size.Y) return true;
            
            // Convert the bounds into indexes to iterate through the world collider pixels
            int startXIndex = (int)(minXPos / worldCollider.Size.X * worldCollider.Subdivision);
            int endXIndex = (int)(maxXPos / worldCollider.Size.X * worldCollider.Subdivision);
            int startYIndex = (int)(minYPos / worldCollider.Size.Y * worldCollider.Subdivision);
            int endYIndex = (int)(maxYPos / worldCollider.Size.Y * worldCollider.Subdivision);
            
            // Check if any of the pixel inside the circle collide with the world
            for (int x = startXIndex; x <= endXIndex; x++)
            {
                for (int y = startYIndex; y <= endYIndex; y++)
                {
                    Vector2 pixelPos = new(
                        (float)x / worldCollider.Subdivision * worldCollider.Size.X, 
                        (float)y / worldCollider.Subdivision * worldCollider.Size.Y);

                    float distFromOrigin = Vector2.Distance(pixelPos, ColliderTransform.Position);

                    if (distFromOrigin <= Radius)
                    {
                        if (worldCollider.GetPixel(x, y)) return true;
                    }
                }
            }

            return false;
        }
    }
}