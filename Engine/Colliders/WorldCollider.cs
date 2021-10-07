using AntEngine.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    /// Collider representing the engineWorld.
    /// </summary>
    public class WorldCollider : Collider
    {
        public WorldCollider(Transform transform, Transform parentTransform) : base(transform, parentTransform) {}
        
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