using AntEngine.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    /// Collider in the shape of a circle.
    /// </summary>
    public class CircleCollider : Collider
    {
        public CircleCollider(Transform transform, Transform parentTransform) : base(transform, parentTransform) {}
        
        //TODO : Add attributes
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