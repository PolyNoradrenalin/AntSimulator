using System;
using System.Collections.Generic;
using System.Numerics;

namespace AntEngine.Utils.Maths
{
    /// <summary>
    /// Contains the data needed for spatial manipulation of entities.
    /// </summary>
    public class Transform
    {
        private float _rotation;

        /// <summary>
        /// Default constructor for a Transform.
        /// </summary>
        public Transform() : this(new Vector2(0, 0), 0, new Vector2(1, 1))
        {
        }

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
        public float Rotation
        {
            get => _rotation;
            set
            {
                if (value < 0)
                {
                    _rotation = value % (-2 * MathF.PI);
                }
                else
                {
                    _rotation = value % (2 * MathF.PI);
                }
            }
        }

        /// <summary>
        /// Stores a scale value in two dimensions (x and y).
        /// Default value is (1,1).
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Generates the transform's director vector.
        /// </summary>
        /// <returns>
        /// Vector2 representation of the transform's director vector.
        /// </returns>
        public Vector2 GetDirectorVector()
        {
            Vector2 director = new()
            {
                X = MathF.Cos(Rotation),
                Y = MathF.Sin(Rotation)
            };

            return director;
        }
        
        /// <summary>
        /// Generates the colliders vertices in the world from ColliderTransform and ParentTransform.
        /// </summary>
        /// <returns>
        /// A list of 4 Vector2(two dimensional vertices) objects.
        /// 
        /// Top Right - Top Left - Bottom Left - Bottom Right
        /// </returns>
        public List<Vector2> GetRectangleVertices()
        {
            List<Vector2> verts = new();
            
            Vector2[] rotationCoefficients = {new(1, 1), new(-1, 1), new(-1, -1), new(1, -1)};

            // Calculating each vertex
            for (int index = 0; index < rotationCoefficients.Length; index++)
            {
                Vector2 vertex = new()
                {
                    X = Position.X + Scale.X / 2 * rotationCoefficients[index].X * MathF.Cos(Rotation) -
                        rotationCoefficients[index].Y * Scale.Y / 2 * MathF.Sin(Rotation),
                    Y = Position.Y + Scale.X / 2 * rotationCoefficients[index].X * MathF.Sin(Rotation) +
                        rotationCoefficients[index].Y * Scale.Y / 2 * MathF.Cos(Rotation)
                };

                verts.Add(vertex);
            }

            return verts;
        }

        /// <summary>
        /// Gets the distance between two transforms (this and a).
        /// </summary>
        /// <param name="a">Other transform</param>
        /// <returns>Distance between the centers of both transforms.</returns>
        public float GetDistance(Transform a)
        {
            return Vector2.Distance(Position, a.Position);
        }

        /// <summary>
        /// Converts a vector to this transform's reference frame.
        /// </summary>
        /// <param name="local">Vector to be transformed from its local frame to this reference frame.</param>
        /// <returns>New vector converted to this reference frame.</returns>
        public Vector2 ConvertToReferenceFrame(Vector2 local)
        {
            Vector2 scaled = new(local.X * Scale.X, local.Y * Scale.Y);
            
            Vector2 rotated = new(scaled.X * MathF.Cos(Rotation) - scaled.Y * MathF.Sin(Rotation),
                scaled.X * MathF.Sin(Rotation) + scaled.Y * MathF.Cos(Rotation));

            Vector2 translated = new(rotated.X + Position.X, rotated.Y + Position.Y);

            return translated;
        }
    }
}