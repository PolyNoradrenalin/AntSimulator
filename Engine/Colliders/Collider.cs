using System.Numerics;
using AntEngine.Utils.Maths;

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
        /// Returns the position of the collider in the reference frame of its parent.
        /// </summary>
        public Vector2 Position => ParentTransform.ConvertToReferenceFrame(ColliderTransform.Position);

        /// <summary>
        /// Returns the total rotation applied on the collider (parent and local).
        /// </summary>
        public float Rotation => ParentTransform.Rotation + ColliderTransform.Rotation;

        /// <summary>
        /// Returns the total scale of the collider.
        /// </summary>
        public Vector2 Scale => ParentTransform.Scale * ColliderTransform.Scale;
        
        /// <summary>
        /// Checks for a collision between this and a circleCollider.
        /// </summary>
        /// <param name="circleCollider"></param>
        /// <returns></returns>
        public abstract bool CheckCollision(CircleCollider circleCollider);

        /// <summary>
        /// Checks for a collision between this and a rectangleCollider.
        /// </summary>
        /// <param name="rectCollider"></param>
        /// <returns></returns>
        public abstract bool CheckCollision(RectangleCollider rectCollider);

        /// <summary>
        /// Checks for a collision between this and a worldCollider.
        /// </summary>
        /// <param name="worldCollider"></param>
        /// <returns></returns>
        public abstract bool CheckCollision(WorldCollider worldCollider);
    }
}