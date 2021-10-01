using AntEngine.Maths;

namespace AntEngine.Colliders
{
    public abstract class Collider
    {
        private Transform Transform;

        protected Collider() { }

        protected bool checkCollision(Collider collider)
        {
            return collider.checkCollision(this);
        }

        protected abstract bool checkCollision(CircleCollider circleCollider);

        protected abstract bool checkCollision(RectangleCollider rectCollider);

        protected abstract bool checkCollision(WorldCollider worldCollider);
    }
}