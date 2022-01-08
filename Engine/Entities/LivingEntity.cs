using System;
using System.Numerics;
using AntEngine.Entities.States;
using AntEngine.Entities.States.Living;
using AntEngine.Utils.Maths;

namespace AntEngine.Entities
{
    /// <summary>
    ///     An Entity that can live and die.
    /// </summary>
    public abstract class LivingEntity : StateEntity
    {
        private int _health;
        private float _speed;

        public LivingEntity(World world) : this("Living Entity", new Transform(), world)
        {
        }

        public LivingEntity(string name, Transform transform, World world) : this(name, transform, world,
            new LivingState())
        {
        }

        public LivingEntity(string name, Transform transform, World world, IState initialState, int maxHealth = 100) :
            base(name, transform, world, initialState)
        {
            MaxHealth = maxHealth;
            Health = MaxHealth;
        }

        /// <summary>
        ///     Health of the entity.
        ///     The entity dies if this falls down to zero.
        /// </summary>
        public int Health
        {
            get => _health;
            protected set => _health = value > MaxHealth ? MaxHealth : value;
        }

        /// <summary>
        ///     Maximum health of the entity.
        /// </summary>
        public int MaxHealth { get; protected set; }

        /// <summary>
        ///     Current speed of the ant.
        /// </summary>
        public float Speed
        {
            get => _speed;
            protected set => _speed = value > MaxSpeed ? MaxSpeed : value;
        }

        /// <summary>
        ///     Maximum speed of the ant.
        /// </summary>
        public float MaxSpeed { get; protected set; }

        /// <summary>
        ///     Applies movement to ant's coordinates.
        /// </summary>
        /// <param name="dir"></param>
        public void Move(Vector2 dir)
        {
            Transform.Rotation = dir.Angle(Vector2.UnitX);

            Vector2 lastPos = Transform.Position;
            Vector2 movement = dir * Speed;
            movement = movement.Length() > MaxSpeed ? MaxSpeed * (movement / movement.Length()) : movement;
            Collider.ParentTransform.Position += movement;
            if (Collider.CheckCollision(World.Collider)) Collider.ParentTransform.Position = lastPos;

            World.UpdateEntityRegion(this);
        }


        /// <summary>
        ///     Kills the entity.
        /// </summary>
        public void Kill()
        {
            OnEntityDeath();
            World.RemoveEntity(this);
        }

        /// <summary>
        ///     Called when the entity just died.
        ///     Note: this is called before the world removes the entity.
        /// </summary>
        protected virtual void OnEntityDeath()
        {
        }
    }
}