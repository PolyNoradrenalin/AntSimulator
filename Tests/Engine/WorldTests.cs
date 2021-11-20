using System.Numerics;
using AntEngine;
using AntEngine.Entities;
using Xunit;

namespace Tests.Engine
{
    public class WorldTests
    {
        private class TestEntity : Entity
        {
            public TestEntity(World world) : base(world)
            {
            }

            public override void Update()
            {
            }
        }

        [Fact]
        public void SpawnEntity_EntityAlreadySpawn_NotAddTwice()
        {
            Vector2 size = new(10, 10);
            World world = new(size);
            TestEntity testEntity = new(world);
            world.ApplyEntityBuffers();
            Assert.Contains(testEntity, world.Entities);
            
            world.AddEntity(testEntity);
            world.ApplyEntityBuffers();
            
            Assert.NotEqual(2, world.EntityCount);
        }
    }
}