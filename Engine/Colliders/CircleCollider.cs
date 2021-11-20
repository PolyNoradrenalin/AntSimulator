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
        public CircleCollider(Transform parentTransform) : base(parentTransform)
        {
            Radius = MathF.Max(parentTransform.Scale.X, parentTransform.Scale.Y) / 2F;
        }

        /// <summary>
        /// Radius of the circle. This is the maximum component of the scale vector. (we're not handling ellipses)
        /// </summary>
        public float Radius;

        public override bool CheckCollision(CircleCollider circleCollider)
        {
            return CollisionDetection.CircleAndCircle(this, circleCollider);
        }

        public override bool CheckCollision(RectangleCollider rectCollider)
        {
            return CollisionDetection.CircleAndRectangle(this, rectCollider);
        }

        public override bool CheckCollision(WorldCollider worldCollider)
        {
            return CollisionDetection.CircleAndWorld(this, worldCollider);
        }
        
        public override bool CheckCollision(Collider collider)
        {
            return collider.CheckCollision(this);
        }
    }
}