using System;
using System.Numerics;
using AntEngine.Maths;
using Xunit;

namespace Tests.Engine
{
    public class TransformTests
    {
        [Theory]
        [InlineData(0, -2 * MathF.PI)]
        [InlineData(0, 2 * MathF.PI)]
        [InlineData(0, -8 * MathF.PI)]
        [InlineData(-MathF.PI, -3 * MathF.PI)]
        public void CreateTransform_RotationOOB_ShouldBeConverted(float f1, float f2)
        {
            Transform t = new Transform(new Vector2(0, 0), f2, new Vector2(1, 1));
            Assert.Equal(f1, t.Rotation, 6);
        }
    }
}