using System.Numerics;
using AntEngine.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    /// Collider representing the engineWorld.
    /// </summary>
    public class WorldCollider : Collider
    {
        public WorldCollider(Transform transform, Transform parentTransform, Vector2 size, int div) :  base(transform, parentTransform)
        {
            Size = size;
            Subdivision = div;
            Matrix = new bool[][div];
            for (int i = 0; i < div; i++) Matrix[i] = new bool[div];
        }

        /// <summary>
        /// Dimensions of the World Collider.
        /// This should be the same as the World size.
        /// </summary>
        public Vector2 Size { get; private set; }
        /// <summary>
        /// The number of "pixels" dividing each side of the world.
        /// NB: If the World Collider is not a square, the pixels won't be squares as well.
        /// </summary>
        public int Subdivision { get; private set; }
        /// <summary>
        /// Stores all the "collision states" of the grid.
        /// </summary>
        private bool[][] Matrix { get; set; }

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