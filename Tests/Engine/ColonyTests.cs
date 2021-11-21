using System.Numerics;
using AntEngine;
using AntEngine.Entities;
using AntEngine.Entities.Colonies;
using AntEngine.Resources;
using Xunit;

namespace Tests.Engine
{
    public class ColonyTests
    {
        private class TestEntity : LivingEntity, IColonyMember
        {
            public TestEntity(World world) : base(world)
            {
            }

            public Colony Home { get; set; }
        }

        [Fact]
        public void SpawnPop_EnoughResources_ShouldSucceed()
        {
            Resource resource = new("cheese", "Cheese");
            World world = new(Vector2.One * 100);
            Colony colony = new(world, (_, _, _, _) => new TestEntity(world));
            colony.Stockpile.AddResource(resource, 100);
            colony.SpawnCost.AddResource(resource, 10);

            colony.Spawn(10);
            
            world.ApplyEntityBuffers();
            
            Assert.Equal(11, world.EntityCount);
        }
        
        [Fact]
        public void SpawnPop_NotEnoughResources_ShouldPartiallySucceed()
        {
            Resource resource = new("cheese", "Cheese");
            World world = new(Vector2.One * 100);
            Colony colony = new(world, (_, _, _, _) => new TestEntity(world));
            colony.Stockpile.AddResource(resource, 50);
            colony.SpawnCost.AddResource(resource, 10);
            
            colony.Spawn(10);
            
            world.ApplyEntityBuffers();
            
            Assert.Equal(6, world.EntityCount);
        }
        
        [Fact]
        public void SpawnPop_NoResources_ShouldFail()
        {
            Resource resource = new("cheese", "Cheese");
            World world = new(Vector2.One * 100);
            Colony colony = new(world, (_, _, _, _) => new TestEntity(world));
            colony.Stockpile.AddResource(resource, 0);
            colony.SpawnCost.AddResource(resource, 100);
            
            colony.Spawn(10);
            
            world.ApplyEntityBuffers();
            
            Assert.Equal(1, world.EntityCount);
        } 
        
        [Fact]
        public void SpawnPop_SeveralResources_ShouldSucceed()
        {
            Resource cheese = new("cheese", "Cheese");
            Resource oysters = new("oysters", "Oysters");
            World world = new(Vector2.One * 100);
            Colony colony = new(world, (_, _, _, _) => new TestEntity(world));
            colony.Stockpile.AddResource(cheese, 1000);
            colony.Stockpile.AddResource(oysters, 500);
            colony.SpawnCost.AddResource(cheese, 100);
            colony.SpawnCost.AddResource(oysters, 50);
            
            colony.Spawn(10);
            
            world.ApplyEntityBuffers();
            
            Assert.Equal(11, world.EntityCount);
        }
        
        [Fact]
        public void SpawnPop_NotEnoughSeveralResources_ShouldPartiallySucceed()
        {
            Resource cheese = new("cheese", "Cheese");
            Resource oysters = new("oysters", "Oysters");
            World world = new(Vector2.One * 100);
            Colony colony = new(world, (_, _, _, _) => new TestEntity(world));
            colony.Stockpile.AddResource(cheese, 200);
            colony.Stockpile.AddResource(oysters, 500);
            colony.SpawnCost.AddResource(cheese, 100);
            colony.SpawnCost.AddResource(oysters, 50);
            
            colony.Spawn(10);
            
            world.ApplyEntityBuffers();
            
            Assert.Equal(3, world.EntityCount);
        }
        
    }
}