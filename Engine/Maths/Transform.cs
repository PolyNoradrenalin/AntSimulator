using System.Numerics;

namespace AntEngine.Maths
{
    /// <summary>
    /// Contains the data needed for spatial manipulation of entities.
    /// </summary>
    public class Transform
    {
        /// <summary>
        /// Default constructor for a Transform.
        /// </summary>
        public Transform() : this(new Vector2(0, 0), 0, new Vector2(1, 1)) {}
        
        /// <summary>
        /// Constructor with arguments of a Transform.
        /// </summary>
        /// <param name="pos">Position</param>
        /// <param name="rotation">Rotation</param>
        /// <param name="scale">Scale</param>
        public Transform(Vector2 pos, float rotation, Vector2 scale)
        {
            Position = pos;
            Rotation = rotation;
            Scale = scale;
        }
        
        /// <summary>
        /// Stores a pair of x and y coordinates.
        /// Values can be real numbers.
        /// Default value is 0.
        /// </summary>
        public Vector2 Position { get; set; }
        
        /// <summary>
        /// Stores a rotation value.
        /// Goes from 0 to 360 and default value is 0.
        /// </summary>
        public float Rotation { get; set; }
        
        /// <summary>
        /// Stores a scale value in two dimensions (x and y).
        /// Default value is (1,1).
        /// </summary>
        public Vector2 Scale { get; set; }
    }
}