using AntEngine.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    /// Abstract collider allows for the calculation of collision between primitive geometrical shapes.
    /// </summary>
    public abstract class Collider
    {
        /// <summary>
        /// Position, rotation and scale of the collider.
        /// </summary>
        private Transform ColliderTransform;
        private Transform ParentTransform;

        /// <summary>
        /// Constructor for a default collider without specifying any coordinates.
        /// Will initialize a collider at 0,0 with a scale of 1,1 with its parent possessing the same coordinates.
        /// </summary>
        public Collider() : this(new Transform(), new Transform()) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="colliderTransform"></param>
        /// <param name="parentTransform"></param>
        public Collider(Transform colliderTransform, Transform parentTransform)
        {
            ColliderTransform = colliderTransform;
            ParentTransform = parentTransform;
        }

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