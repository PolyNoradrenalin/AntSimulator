using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AntEngine.Utils.Maths;

namespace AntEngine.Colliders
{
    public static class CollisionDetection
    {
        /// <summary>
        /// Checks if two CircleColliders collide.
        /// </summary>
        /// <returns>true if collision, false otherwise</returns>
        public static bool CircleAndCircle(CircleCollider circleA, CircleCollider circleB)
        {
            float distance = Vector2.Distance(circleA.ParentTransform.Position, circleB.ParentTransform.Position);
            return distance < circleA.Radius + circleB.Radius;
        }

        /// <summary>
        /// Checks if a CircleCollider and a RectangleCollider collide.
        /// </summary>
        /// <returns>true if collision, false otherwise</returns>
        public static bool CircleAndRectangle(CircleCollider circle, RectangleCollider rect)
        {
            List<Vector2> vertices = rect.ParentTransform.GetRectangleVertices();

            Vector2 circlePos = circle.ParentTransform.Position;

            // First we want to compute the closest point on the circle to a vertex of the rectangle

            float minDist = float.MaxValue;
            Vector2 closestDelta = new();

            foreach (Vector2 vert in vertices)
            {
                Vector2 delta = new(vert.X - circlePos.X, vert.Y - circlePos.Y);

                float distance = MathF.Pow(delta.X, 2) + MathF.Pow(delta.Y, 2);

                if (!(distance < minDist)) continue;
                minDist = distance;
                closestDelta = delta;
            }

            // We normalize the axis found to simplify the following calculations.
            Vector2 circleAxis = Vector2.Normalize(closestDelta);

            Transform t = new()
            {
                Rotation = rect.ParentTransform.Rotation
            };

            Vector2 directorVector = t.GetDirectorVector();

            Vector2 normalVector = new(-directorVector.Y, directorVector.X);

            List<Vector2> axes = new() {directorVector, normalVector, circleAxis};

            // Iterate through the axis, projecting each time all the rectangle's vertices and the circle's center +/- its width
            foreach (Vector2 axis in axes)
            {
                (float rectProjMin, float rectProjMax) = ProjectVertsOnAxis(axis, vertices);

                float temp = Vector2.Dot(axis, circlePos);
                (float circleProjMin, float circleProjMax) =
                    (temp - circle.Radius, temp + circle.Radius);

                // Check if the projected vertices overlap.
                // If they do then according to Separating Axis Theorem, the rectangles cannot be in collision.
                if (rectProjMin - circleProjMax > 0 || circleProjMin - rectProjMax > 0) return false;
            }

            // If all vertices have been checked, then the two colliders are in collision
            return true;
        }

        /// <summary>
        /// Checks if a CircleCollider and a WorldCollider collide.
        /// </summary>
        /// <returns>true if collision, false otherwise</returns>
        public static bool CircleAndWorld(CircleCollider circle, WorldCollider world)
        {
            // Get bounding rectangle pixels to check only in the appropriate sub-matrix of WorldCollider
            Transform rectTransform = new(
                circle.ParentTransform.Position,
                0,
                circle.ParentTransform.Scale);

            IList<Vector2> rectVertices = rectTransform.GetRectangleVertices();

            (float min, float max) XPos = (rectVertices[2].X, rectVertices[0].X);
            (float min, float max) YPos = (rectVertices[2].Y, rectVertices[0].Y);

            // If the circle has a part outside the world that's a collision
            if (world.IsOutOfBounds(XPos, YPos)) return true;

            // Convert the bounds into indexes to iterate through the world collider pixels
            (int x, int y) minIndex = world.ConvertCoordsToIndex(new Vector2(XPos.min, YPos.min));
            (int x, int y) maxIndex = world.ConvertCoordsToIndex(new Vector2(XPos.max, YPos.max));

            // Check if any of the pixel inside the circle collide with the world
            for (int x = minIndex.x; x <= maxIndex.x; x++)
            for (int y = minIndex.y; y <= maxIndex.y; y++)
            {
                Vector2 pixelPos = new(
                    (float) x / world.Subdivision * world.Size.X,
                    (float) y / world.Subdivision * world.Size.Y);

                float distFromOrigin = Vector2.Distance(pixelPos, circle.ParentTransform.Position);

                if (!(distFromOrigin <= circle.Radius)) continue;
                if (world.Matrix[y][x]) return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a RectCollider is in collision with another RectCollider by using the Separating Axis Theorem.
        /// </summary>
        /// <returns>
        /// True if the rectangles are in collision and false otherwise.
        /// </returns>
        /// <exception cref="NullReferenceException">
        /// Throws an exception if the generated vertices list is empty.
        /// </exception>
        public static bool RectangleAndRectangle(RectangleCollider rectA, RectangleCollider rectB)
        {
            // Get the rectangles' initial director vector, normal vector and vertices.
            Transform t = new()
            {
                Rotation = rectA.ParentTransform.Rotation
            };

            Vector2 direct1 = t.GetDirectorVector();

            t.Rotation = rectB.ParentTransform.Rotation;

            Vector2 direct2 = t.GetDirectorVector();

            Vector2 normal1 = new(-direct1.Y, direct1.X);
            Vector2 normal2 = new(-direct2.Y, direct2.X);

            List<Vector2> axes = new() {direct1, direct2, normal1, normal2};

            //TODO : Remove duplicate axes.

            // Gets the rectangles' vertices list.
            List<Vector2> vertices1 = rectA.ParentTransform.GetRectangleVertices();
            List<Vector2> vertices2 = rectB.ParentTransform.GetRectangleVertices();

            // Check that the vertex list has been correctly created
            if (vertices1?.Count == 0 || vertices2?.Count == 0)
                throw new NullReferenceException("Generated vertices list is null during rectangleCollision.");

            foreach (Vector2 axis in axes)
            {
                // Project the vertices of both rectangles on to axis.
                (float projectionMinimum1, float projectionMaximum1) = ProjectVertsOnAxis(axis, vertices1);
                (float projectionMinimum2, float projectionMaximum2) = ProjectVertsOnAxis(axis, vertices2);

                // Check if the projected vertices overlap.
                // If they do then according to Separating Axis Theorem, the rectangles cannot be in collision.
                if (projectionMinimum1 - projectionMaximum2 > 0 || projectionMinimum2 - projectionMaximum1 > 0)
                    return false;
                // Otherwise continue until all axes have been tested.
            }

            // If all possible projections have been completed and every time the projected points overlap
            // then the two rectangles are definitely in collision according to the Separating Axis Theorem.
            return true;
        }

        /// <summary>
        /// Checks if a RectangleCollider and a WorldCollider collide.
        /// </summary>
        /// <returns>true if collision, false otherwise</returns>
        public static bool RectangleAndWorld(RectangleCollider rect, WorldCollider world)
        {
            // Get the pixels of the world collider beneath the rectangle.
            Transform rectTransform = new(rect.ParentTransform.Position, 0, rect.ParentTransform.Scale);
            IList<Vector2> rectVertices = rectTransform.GetRectangleVertices();

            (float min, float max) XPos = (rectVertices[2].X, rectVertices[0].X);
            (float min, float max) YPos = (rectVertices[2].Y, rectVertices[0].Y);

            // If the circle has a part outside the world that's a collision
            if (world.IsOutOfBounds(XPos, YPos)) return true;

            // Convert the bounds into indexes to iterate through the world collider pixels
            (int x, int y) MinIndex = world.ConvertCoordsToIndex(new Vector2(XPos.min, YPos.min));
            (int x, int y) MaxIndex = world.ConvertCoordsToIndex(new Vector2(XPos.max, YPos.max));

            (int x, int y) origin =
                world.ConvertCoordsToIndex(rect.ParentTransform.Position);

            for (int x = MinIndex.x; x < MaxIndex.x; x++)
            for (int y = MinIndex.y; y < MaxIndex.y; y++)
            {
                (int x, int y) rotatedPixels = (
                    (int) ((x - origin.x) * MathF.Cos(rect.ParentTransform.Rotation) -
                           (y - origin.y) * MathF.Sin(rect.ParentTransform.Rotation)),
                    (int) ((x - origin.x) * MathF.Sin(rect.ParentTransform.Rotation) +
                           (y - origin.y) * MathF.Cos(rect.ParentTransform.Rotation)));

                if (world.IsOutOfBounds(rotatedPixels.x + origin.x, rotatedPixels.y + origin.y)) return true;
                if (world.Matrix[rotatedPixels.y + origin.y][rotatedPixels.x + origin.x]) return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if two WorldColliders collide.
        /// </summary>
        /// <returns>true if collision, false otherwise</returns>
        public static bool WorldAndWorld(WorldCollider worldA, WorldCollider worldB)
        {
            int minDiv = Math.Min(worldA.Subdivision, worldB.Subdivision);

            for (int y = 0; y < minDiv; y++)
            for (int x = 0; x < minDiv; x++)
                if (worldA.Matrix[y][x] && worldB.Matrix[x][y])
                    return true;

            return false;
        }

        /// <summary>
        /// Projects a list of vertices onto an axis and returns the minimum and maximum value of the projection.
        /// </summary>
        /// <param name="axis"></param> Axis on which the vertices will be projected
        /// <param name="vertices"></param> Vertices to be projected
        /// <returns>Tuple composed of the minimum projection value and the maximum projection value.</returns>
        private static Tuple<float, float> ProjectVertsOnAxis(Vector2 axis, List<Vector2> vertices)
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
    }
}