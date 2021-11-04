using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using AntEngine.Utils.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    /// Collider in the shape of a rectangle.
    /// </summary>
    public class RectangleCollider : Collider
    {
        public RectangleCollider(Transform transform, Transform parentTransform) : base(transform, parentTransform)
        {
        }

        /// <summary>
        /// Checks if this collider is in collision with a circleCollider using separating axis theorem.
        /// </summary>
        /// <param name="circleCollider">Circle collider to check collision with this collider.</param>
        /// <returns>True if the colliders are in collision and false if not.</returns>
        public override bool checkCollision(CircleCollider circleCollider)
        {
            List<Vector2> vertices = ColliderTransform.GetRectangleVertices()
                .Select(v => ParentTransform.ConvertToReferenceFrame(v)).ToList();

            Vector2 circlePos = circleCollider.ColliderTransform.Position + circleCollider.ParentTransform.Position;
            
            // First we want to compute the closest point on the circle to a vertex of the rectangle
            
            float minDist = float.MaxValue;
            Vector2 closestDelta = new();
            Vector2 circleAxis = new();

            foreach (Vector2 vert in vertices)
            {
                Vector2 delta = new(vert.X - circlePos.X, vert.Y - circlePos.Y);

                float distance = MathF.Pow(delta.X, 2) + MathF.Pow(delta.Y, 2);
                
                if (!(distance < minDist)) continue;
                minDist = distance;
                closestDelta = delta;
            }

            // We normalize the axis found to simplify the following calculations.
            circleAxis = Vector2.Normalize(closestDelta);
            Vector2 directorVector = ColliderTransform.GetDirectorVector();
            Vector2 normalVector = new(-directorVector.Y, directorVector.X);

            List<Vector2> axes = new() {directorVector, normalVector, circleAxis};

            // Iterate through the axis, projecting each time all the rectangle's vertices and the circle's center +/- its width
            foreach (Vector2 axis in axes)
            {
                (float rectProjMin, float rectProjMax) = ProjectVertsOnAxis(axis, vertices);
                
                float temp = Vector2.Dot(axis, circlePos);
                (float circleProjMin, float circleProjMax) =
                    (temp - circleCollider.Radius, temp + circleCollider.Radius);
                
                // Check if the projected vertices overlap.
                // If they do then according to Separating Axis Theorem, the rectangles cannot be in collision.
                if (rectProjMin - circleProjMax > 0 || circleProjMin - rectProjMax > 0)
                {
                    return false;
                }
            }

            // If all vertices have been checked, then the two colliders are in collision
            return true;
        }

        /// <summary>
        /// Checks if this collider is in collision with rectCollider by using the Separating Axis Theorem.
        /// </summary>
        /// <param name="rectCollider">
        /// Rectangle collider to check collision with this object.
        /// </param>
        /// <returns>
        /// True if the rectangles are in collision and false otherwise.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws an exception if the generated vertices list is empty.
        /// </exception>
        public override bool checkCollision(RectangleCollider rectCollider)
        {
            // Get the rectangles' initial director vector, normal vector and vertices.
            Vector2 direct1 = ColliderTransform.GetDirectorVector();
            Vector2 direct2 = rectCollider.ColliderTransform.GetDirectorVector();

            Vector2 normal1 = new(-direct1.Y, direct1.X);
            Vector2 normal2 = new(-direct2.Y, direct2.X);

            List<Vector2> axes = new() {direct1, direct2, normal1, normal2};

            //TODO : Remove duplicate axes.
            
            // Gets the rectangles' vertices list.
            List<Vector2> vertices1 = ColliderTransform.GetRectangleVertices()
                .Select(v => ParentTransform.ConvertToReferenceFrame(v)).ToList();
            List<Vector2> vertices2 = rectCollider.ColliderTransform.GetRectangleVertices()
                .Select(v => rectCollider.ParentTransform.ConvertToReferenceFrame(v)).ToList();

            // Check that the vertex list has been correctly created
            if (vertices1?.Count == 0 || vertices2?.Count == 0)
            {
                throw new NullReferenceException("Generated vertices list is null during rectangleCollision.");
            }

            foreach (Vector2 axis in axes)
            {
                // Project the vertices of both rectangles on to axis.
                (float projectionMinimum1, float projectionMaximum1) = ProjectVertsOnAxis(axis, vertices1);
                (float projectionMinimum2, float projectionMaximum2) = ProjectVertsOnAxis(axis, vertices2);

                // Check if the projected vertices overlap.
                // If they do then according to Separating Axis Theorem, the rectangles cannot be in collision.
                if (projectionMinimum1 - projectionMaximum2 > 0 || projectionMinimum2 - projectionMaximum1 > 0)
                {
                    return false;
                }
                // Otherwise continue until all axes have been tested.
            }

            // If all possible projections have been completed and every time the projected points overlap
            // then the two rectangles are definitely in collision according to the Separating Axis Theorem.
            return true;
        }

        public override bool checkCollision(WorldCollider worldCollider)
        {
            // Get the pixels of the world collider beneath the rectangle.
            Transform rectTransform = new(ColliderTransform.Position, 0, ColliderTransform.Scale);
            IList<Vector2> rectVertices = rectTransform.GetRectangleVertices();

            (float min, float max) XPos = (rectVertices[2].X, rectVertices[0].X);
            (float min, float max) YPos = (rectVertices[2].Y, rectVertices[0].Y);

            // If the circle has a part outside the world that's a collision
            if (worldCollider.IsOutOfBounds(XPos, YPos)) return true;
            
            // Convert the bounds into indexes to iterate through the world collider pixels
            (int x, int y) MinIndex = worldCollider.ConvertCoordsToIndex(new Vector2(XPos.min, YPos.min));
            (int x, int y) MaxIndex = worldCollider.ConvertCoordsToIndex(new Vector2(XPos.max, YPos.max));

            (int x, int y) origin =
                worldCollider.ConvertCoordsToIndex(ColliderTransform.Position + ParentTransform.Position);
            
            for (int x = MinIndex.x; x < MaxIndex.x; x++)
            {
                for (int y = MinIndex.y; y < MaxIndex.y; y++)
                {
                    (int x, int y) rotatedPixels = (
                        (int) ((x - origin.x) * MathF.Cos(ColliderTransform.Rotation) - (y - origin.y) * MathF.Sin(ColliderTransform.Rotation)), 
                        (int) ((x - origin.x) * MathF.Sin(ColliderTransform.Rotation) + (y - origin.y) * MathF.Cos(ColliderTransform.Rotation)));

                    if (worldCollider.IsOutOfBounds(rotatedPixels.x + origin.x, rotatedPixels.y + origin.y)) return true;
                    if (worldCollider.GetPixel(rotatedPixels.x + origin.x, rotatedPixels.y + origin.y)) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Projects a list of vertices onto an axis and returns the minimum and maximum value of the projection.
        /// </summary>
        /// <param name="axis"></param> Axis on which the vertices will be projected
        /// <param name="vertices"></param> Vertices to be projected
        /// <returns>Tuple composed of the minimum projection value and the maximum projection value.</returns>
        private Tuple<float, float> ProjectVertsOnAxis(Vector2 axis, List<Vector2> vertices)
        {
            float minProjection = Vector2.Dot(axis, vertices[0]);
            float maxProjection = minProjection;

            // Get the position of each vertex on the axis
            foreach (Vector2 vertex in vertices)
            {
                float dot = Vector2.Dot(axis, vertex);

                minProjection = MathF.Min(dot, minProjection);
                maxProjection = MathF.Max(dot, maxProjection);
            }

            return new Tuple<float, float>(minProjection, maxProjection);
        }

        private Vector2 GetScale()
        {
            return ParentTransform.Scale + ColliderTransform.Scale;
        }

        private Vector2 GetPosition()
        {
            return ParentTransform.Position + ColliderTransform.Position;
        }

        private float GetRotation()
        {
            return ParentTransform.Rotation + ColliderTransform.Rotation;
        }
    }
}