using System;
using AntEngine.Colliders;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities
{
    /// <summary>
    ///     Base class for all things that can exist on a World.
    /// </summary>
    public abstract class Entity
    {
        protected const string DefaultName = "Entity";

        public Entity(World world) : this(DefaultName, new Transform(), world)
        {
        }

        public Entity(string name, Transform transform, World world)
        {
            Name = name;
            Transform = transform;
            World = world;

            world.AddEntity(this);
        }

        /// <summary>
        ///     Identifier of the entity.
        ///     Names are not necessarily unique but it's better to distinguish entities.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Transform of the entity in the world.
        ///     Represents the position, rotation and scale of the entity in the world.
        /// </summary>
        public Transform Transform { get; }

        /// <summary>
        ///     World of the entity.
        /// </summary>
        public World World { get; }

        /// <summary>
        ///     Region of the entity in the world.
        /// </summary>
        public (int X, int Y) Region { get; set; }

        /// <summary>
        ///     Collider of the current entity.
        /// </summary>
        public Collider Collider { get; protected set; }

        /// <summary>
        ///     Called by the world when this entity needs to update the status of each of its components.
        /// </summary>
        public abstract void Update();

        /// <summary>
        ///     Displays a message in the console.
        /// </summary>
        /// <param name="msg">The message to display</param>
        public virtual void Log(string msg)
        {
            Console.WriteLine($"[{GetType().Name}] {Name} : {msg}");
        }
    }
}