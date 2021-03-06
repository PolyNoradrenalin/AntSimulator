using System.Numerics;
using AntEngine.Utils.Maths;

namespace AntEngine.Colliders
{
    /// <summary>
    ///     Collider representing the engineWorld.
    /// </summary>
    public class WorldCollider : Collider
    {
        /// <summary>
        ///     Stores all the "collision states" of the grid.
        ///     True = Collision
        ///     False = No collision
        /// </summary>
        public bool[][] Matrix;

        /// <summary>
        ///     Dimensions of the World Collider.
        ///     This should be the same as the World size.
        /// </summary>
        public Vector2 Size;

        /// <summary>
        ///     The number of "pixels" dividing each side of the world.
        ///     NB: If the World Collider is not a square, the pixels won't be squares as well.
        /// </summary>
        public int Subdivision;

        public WorldCollider(Transform parentTransform, Vector2 size, int div) : base(parentTransform)
        {
            Size = size;
            Subdivision = div;
            Matrix = new bool[div][];
            for (int i = 0; i < div; i++) Matrix[i] = new bool[div];
        }

        /// <summary>
        ///     Checks if the coordinates are in the world boundaries.
        /// </summary>
        /// <param name="posX">Min x and Max x</param>
        /// <param name="poxY">Min y and Max y</param>
        /// <returns>True if all constraints are in the world boundaries, false otherwise</returns>
        public bool IsOutOfBounds((float min, float max) posX, (float min, float max) poxY)
        {
            return posX.min <= 0 || posX.max >= Size.X || poxY.min <= 0 || poxY.max >= Size.Y;
        }

        /// <summary>
        ///     Checks if the coordinates are in the world boundaries.
        /// </summary>
        /// <param name="posX">X coord</param>
        /// <param name="poxY">Y coord</param>
        /// <returns>True if all constraints are in the world boundaries, false otherwise</returns>
        public bool IsOutOfBounds(float x, float y)
        {
            return IsOutOfBounds((x, x), (y, y));
        }

        /// <summary>
        ///     Returns the indexes corresponding to the coordinates.
        /// </summary>
        public (int x, int y) ConvertCoordsToIndex(Vector2 position)
        {
            return (
                (int) (position.X / Size.X * Subdivision),
                (int) (position.Y / Size.Y * Subdivision));
        }

        public override bool CheckCollision(CircleCollider circleCollider)
        {
            return CollisionDetection.CircleAndWorld(circleCollider, this);
        }

        public override bool CheckCollision(RectangleCollider rectCollider)
        {
            return CollisionDetection.RectangleAndWorld(rectCollider, this);
        }

        public override bool CheckCollision(WorldCollider worldCollider)
        {
            return CollisionDetection.WorldAndWorld(this, worldCollider);
        }

        public override bool CheckCollision(Collider collider)
        {
            return collider.CheckCollision(this);
        }
    }
}