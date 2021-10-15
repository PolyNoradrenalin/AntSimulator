using AntEngine.Entities.States;
using AntEngine.Maths;

namespace AntEngine.Entities.Colonies
{
    /// <summary>
    /// Entity that can spawn other entities with resources.
    /// </summary>
    /// <typeparam name="E">The type of the entity to spawn.</typeparam>
    public class Colony<E> : LivingEntity where E : LivingEntity, new()
    {
        public Colony(World world) : base(world)
        {
        }

        public Colony(string name, Transform transform, World world) : base(name, transform, world)
        {
        }

        public Colony(string name, Transform transform, World world, IState initialState) : base(name, transform, world, initialState)
        {
        }
    }
}