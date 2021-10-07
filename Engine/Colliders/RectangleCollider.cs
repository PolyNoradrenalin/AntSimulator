using AntEngine.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    /// Collider in the shape of a rectangle.
    /// </summary>
    public class RectangleCollider : Collider
    {
        public RectangleCollider(Transform transform, Transform parentTransform) : base(transform, parentTransform) {}
        
        public override bool checkCollision(CircleCollider circleCollider)
        {
            //TODO : Implement
            throw new System.NotImplementedException();
        }

        public override bool checkCollision(RectangleCollider rectCollider)
        {
            //TODO : Implement
            throw new System.NotImplementedException();
        }

        public override bool checkCollision(WorldCollider worldCollider)
        {
            //TODO : Implement
            throw new System.NotImplementedException();
        }
    }
}