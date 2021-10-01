using AntEngine.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    /// Abstract collider allows for the calculation of collision between primitive geometrical shapes.
    /// </summary>
    public abstract class Collider
    {
        /// <summary>
        /// Boundaries of the collider.
        /// </summary>
        private Transform Transform;

        protected Collider() { }

        /// <summary>
        /// Check for a collision between this and a generic "collider" object.
        /// Calls the checkCollision of the other object.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        protected bool checkCollision(Collider collider)
        {
            return collider.checkCollision(this);
        }

        /// <summary>
        /// Checks for a collision between this and a circleCollider.
        /// </summary>
        /// <param name="circleCollider"></param>
        /// <returns></returns>
        protected abstract bool checkCollision(CircleCollider circleCollider);

        /// <summary>
        /// Checks for a collision between this and a rectangleCollider.
        /// </summary>
        /// <param name="rectCollider"></param>
        /// <returns></returns>
        protected abstract bool checkCollision(RectangleCollider rectCollider);

        /// <summary>
        /// Checks for a collision between this and a worldCollider.
        /// </summary>
        /// <param name="worldCollider"></param>
        /// <returns></returns>
        protected abstract bool checkCollision(WorldCollider worldCollider);
    }
}