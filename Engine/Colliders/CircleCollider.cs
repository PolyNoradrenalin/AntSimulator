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
            return CollisionDetection.CircleAndCircle(this, circleCollider);
        }

        public override bool checkCollision(RectangleCollider rectCollider)
        {
            return CollisionDetection.CircleAndRectangle(this, rectCollider);
        }

        public override bool checkCollision(WorldCollider worldCollider)
        {
            return CollisionDetection.CircleAndWorld(this, worldCollider);
        }
    }
}