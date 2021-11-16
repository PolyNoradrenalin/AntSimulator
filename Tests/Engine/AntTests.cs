using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AntEngine;
using AntEngine.Entities.Ants;
using AntEngine.Entities.Pheromones;
using AntEngine.Utils.Maths;
using Xunit;

namespace Tests.Engine
{
    public class AntTests
    {

        [Fact]
        public void GetPerceptionMap_OneUpperRightPheromone_ShouldSucceed()
        {
            World w = new(Vector2.One * 100);
            Ant ant = new(w);
            FoodPheromone foodPheromone = new(w);

            ant.Transform.Position = Vector2.One;
            foodPheromone.Transform.Position = Vector2.One * 2;

            List<float> wList = new(new float[24])
            {
                [3] = 1 / MathF.Pow(MathF.PI / 4, 2) + 1 / MathF.Pow(MathF.Sqrt(2), 2)
            };

            PerceptionMap perception = ant.GetPerceptionMap<FoodPheromone>();
            
            for (int i = 0; i < perception.Weights.Count; i++)
            {
                Assert.Equal(perception.Weights.Values.ToList()[i], wList[i]);
            }
        }
        
        [Fact]
        public void GetPerceptionMap_OneBottomLeftPheromone_ShouldSucceed()
        {
            World w = new(Vector2.One * 100);
            Ant ant = new(w);
            FoodPheromone foodPheromone = new(w);

            ant.Transform.Position = -1 * Vector2.One;
            ant.Transform.Rotation = MathF.PI;
            foodPheromone.Transform.Position = Vector2.One * -2;

            List<float> wList = new(new float[24])
            {
                [2] = 1 / MathF.Pow(MathF.PI / 4, 2) + 1 / MathF.Pow(MathF.Sqrt(2), 2)
            };

            PerceptionMap perception = ant.GetPerceptionMap<FoodPheromone>();
            
            for (int i = 0; i < perception.Weights.Count; i++)
            {
                Assert.Equal(perception.Weights.Values.ToList()[i], wList[i], 4);
            }
        }
    }
}