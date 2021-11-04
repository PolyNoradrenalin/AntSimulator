using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using AntEngine.Utils.Maths;
using Xunit;

namespace Tests.Engine
{
    public class PerceptionMapTests
    {
        private class Vector2Comparer :  IEqualityComparer<Vector2>
        {
            private int _precision;
            
            public Vector2Comparer(int precision)
            {
                _precision = precision;
            }
            
            public bool Equals(Vector2 x, Vector2 y)
            {
                return MathF.Abs(x.X - y.X) < MathF.Pow(10, -_precision) && MathF.Abs(x.Y - y.Y) < MathF.Pow(10, -_precision);
            }

            public int GetHashCode(Vector2 obj)
            {
                return HashCode.Combine(obj.X, obj.Y);
            }
        }

        [Theory]
        [InlineData(new[] { 1f })]
        [InlineData(new[] { 1f, 1 })]
        [InlineData(new[] { 1f, 1, 1 })]
        [InlineData(new[] { 1f, 1, 1, 1 })]
        [InlineData(new[] { 1f, 1, 1, 1, 1 })]
        public void MapCreation_SeveralDirections_AnglesEquallySpaced(float[] weights)
        {
            PerceptionMap perceptionMap = new(weights);
            float angleStep = 2F * MathF.PI / weights.Length;
            
            for (int i = 0; i < weights.Length; i++)
            {
                float angle = i * angleStep + MathF.PI / 2F;
                Assert.Contains(new Vector2(MathF.Cos(angle), MathF.Sin(angle)), perceptionMap.Weights.Keys);
            }
        }

        [Theory]
        [InlineData(new[] { 1f, 1 })]
        [InlineData(new[] { 1f, 1, 1 })]
        [InlineData(new[] { 1f, 1, 1, 1 })]
        [InlineData(new[] { 1f, 1, 1, 1, 1 })]
        public void Mean_MoreThanOneEqualWeights_VectorZero(float[] weights)
        {
            PerceptionMap perceptionMap = new(weights);
            Assert.Equal(Vector2.Zero, perceptionMap.Mean, new Vector2Comparer(6));
        }
    }
}