using AntEngine.Utils.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    ///     Abstract collider allows for the calculation of collision between primitive geometrical shapes.
    /// </summary>
    public abstract class Collider
    {
        public Transform ParentTransform;

        /// <summary>
        ///     Constructor for a collider with a specified transform and parentTransform.
        /// </summary>
        /// <param name="parent"></param>
        public Collider(Transform parent)
        {
            ParentTransform = parent;
        }

        /// <summary>
        ///     Checks for a collision between this and a circleCollider.
        /// </summary>
        /// <param name="circleCollider"></param>
        /// <returns></returns>
        public abstract bool CheckCollision(CircleCollider circleCollider);

        /// <summary>
        ///     Checks for a collision between this and a rectangleCollider.
        /// </summary>
        /// <param name="rectCollider"></param>
        /// <returns></returns>
        public abstract bool CheckCollision(RectangleCollider rectCollider);

        /// <summary>
        ///     Checks for a collision between this and a worldCollider.
        /// </summary>
        /// <param name="worldCollider"></param>
        /// <returns></returns>
        public abstract bool CheckCollision(WorldCollider worldCollider);

        /// <summary>
        ///     Checks for a collision between this and a collider.
        /// </summary>
        /// <param name="collider"></param>
        /// <returns></returns>
        public abstract bool CheckCollision(Collider collider);
    }
}