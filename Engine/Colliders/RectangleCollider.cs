using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using AntEngine.Maths;

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

        public override bool checkCollision(CircleCollider circleCollider)
        {
            //TODO : Implement
            throw new System.NotImplementedException();
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

            // Gets the rectangles' vertices list.
            List<Vector2> vertices1 = ColliderTransform.GetRectangleVertices().Zip(ParentTransform.GetRectangleVertices(), (v1, v2) => v1 + v2).ToList();
            List<Vector2> vertices2 = rectCollider.ColliderTransform.GetRectangleVertices().Zip(rectCollider.ParentTransform.GetRectangleVertices(), (v1, v2) => v1 + v2).ToList();

            // Check that the vertex list has been correctly created
            if (vertices1?.Count == 0 || vertices2?.Count == 0)
            {
                throw new NullReferenceException("Generated vertices list is null during rectangleCollision.");
            }

            foreach (Vector2 axis in axes)
            {
                // Project the vertices of both rectangles on to axis.
                Tuple<float, float> projectionResult1 = ProjectVertsOnAxis(axis, vertices1);
                Tuple<float, float> projectionResult2 = ProjectVertsOnAxis(axis, vertices2);

                // Check if the projected vertices overlap.
                // If they do then according to Separating Axis Theorem, the rectangles cannot be in collision.
                if (projectionResult1.Item1 - projectionResult2.Item2 > 0 || projectionResult2.Item1 - projectionResult1.Item2 > 0)
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
            //TODO : Implement
            throw new System.NotImplementedException();
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