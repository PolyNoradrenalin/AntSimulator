using AntEngine.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    /// Abstract collider allows for the calculation of collision between primitive geometrical shapes.
    /// </summary>
    public abstract class Collider
    {
        /// <summary>
        /// Constructor for a collider with a specified transform and parentTransform.
        /// </summary>
        /// <param name="collider"></param>
        /// <param name="parent"></param>
        public Collider(Transform collider, Transform parent)
        {
            ColliderTransform = collider;
            ParentTransform = parent;
        }
        
        /// <summary>
        /// Position, rotation and scale of the collider.
        /// </summary>
        public Transform ColliderTransform { get; private set; }
        public Transform ParentTransform { get; private set; }

        /// <summary>
        /// Checks for a collision between this and a circleCollider.
        /// </summary>
        /// <param name="circleCollider"></param>
        /// <returns></returns>
        public abstract bool checkCollision(CircleCollider circleCollider);

        /// <summary>
        /// Checks for a collision between this and a rectangleCollider.
        /// </summary>
        /// <param name="rectCollider"></param>
        /// <returns></returns>
        public abstract bool checkCollision(RectangleCollider rectCollider);

        /// <summary>
        /// Checks for a collision between this and a worldCollider.
        /// </summary>
        /// <param name="worldCollider"></param>
        /// <returns></returns>
        public abstract bool checkCollision(WorldCollider worldCollider);
    }
}