using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace AntEngine.Maths
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
            Vector2 director = Vector2.One;

            director.X = director.X * MathF.Cos(Rotation) - director.Y * MathF.Sin(Rotation);
            director.Y = director.X * MathF.Sin(Rotation) + director.Y * MathF.Cos(Rotation);

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
            float rotation = Rotation;
            Vector2 v = new(Position.X, Position.Y);

            // Calculating each vertex
            for (int index = 0; index < rotationCoefficients.Length; index++)
            {
                Vector2 vertex = new()
                {
                    X = v.X + Scale.X / 2 * rotationCoefficients[index].X * MathF.Cos(rotation) -
                        rotationCoefficients[index].Y * Scale.Y / 2 * MathF.Sin(rotation),
                    Y = v.Y + Scale.X / 2 * rotationCoefficients[index].X * MathF.Sin(rotation) +
                        rotationCoefficients[index].Y * Scale.Y / 2 * MathF.Cos(rotation)
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
    }
}