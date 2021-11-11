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
        public override bool CheckCollision(CircleCollider circleCollider)
        {
            return CollisionDetection.CircleAndRectangle(circleCollider, this);
        }


        public override bool CheckCollision(RectangleCollider rectCollider)
        {
            return CollisionDetection.RectangleAndRectangle(this, rectCollider);
        }

        public override bool CheckCollision(WorldCollider worldCollider)
        {
            return CollisionDetection.RectangleAndWorld(this, worldCollider);
        }
    }
}