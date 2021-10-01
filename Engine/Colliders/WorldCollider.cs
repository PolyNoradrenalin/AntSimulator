namespace AntEngine.Colliders
{
    public class WorldCollider : Collider
    {
        protected override bool checkCollision(CircleCollider circleCollider)
        {
            throw new System.NotImplementedException();
        }

        protected override bool checkCollision(RectangleCollider rectCollider)
        {
            throw new System.NotImplementedException();
        }

        protected override bool checkCollision(WorldCollider worldCollider)
        {
            throw new System.NotImplementedException();
        }
    }
}