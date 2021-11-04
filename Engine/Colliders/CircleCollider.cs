using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using AntEngine.Utils.Maths;

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
            // Get bounding rectangle pixels to check only in the appropriate sub-matrix of WorldCollider
            Transform rectTransform = new(ColliderTransform.Position + ParentTransform.Position,
                0,
                ColliderTransform.Scale * ParentTransform.Scale);
            
            IList<Vector2> rectVertices = rectTransform.GetRectangleVertices();

            (float min, float max) XPos = (rectVertices[2].X, rectVertices[0].X);
            (float min, float max) YPos = (rectVertices[2].Y, rectVertices[0].Y);

            // If the circle has a part outside the world that's a collision
            if (worldCollider.IsOutOfBounds(XPos, YPos)) return true;
            
            // Convert the bounds into indexes to iterate through the world collider pixels
            (int x, int y) MinIndex = worldCollider.ConvertCoordsToIndex(new Vector2(XPos.min, YPos.min));
            (int x, int y) MaxIndex = worldCollider.ConvertCoordsToIndex(new Vector2(XPos.max, YPos.max));
            
            // Check if any of the pixel inside the circle collide with the world
            for (int x = MinIndex.x; x <= MaxIndex.x; x++)
            {
                for (int y = MinIndex.y; y <= MaxIndex.y; y++)
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