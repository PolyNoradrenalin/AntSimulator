using System;
using System.Collections.Generic;
using System.Numerics;

namespace AntEngine.Maths
{
    /// <summary>
    /// A list of Vector2 each associated with a weight.
    /// Used to represent which directions are the most important.
    /// </summary>
    public class PerceptionMap
    {
        /// <summary>
        /// Associates each weight of the list to a vector going all around a circle, equally spaced.
        /// </summary>
        /// <param name="weights"></param>
        public PerceptionMap(IReadOnlyList<int> weights)
        {
            Weights = new Dictionary<Vector2, float>(weights.Count);
            float stepAngle = 2F * MathF.PI / weights.Count;
            
            for (int i = 0; i < weights.Count; i++)
            {
                float angle = i * stepAngle;
                Vector2 dir = new(MathF.Cos(angle), MathF.Sin(angle));
                Weights.Add(dir, weights[i]);
            }
        }
        
        /// <summary>
        /// Each direction associated with its weight.
        /// </summary>
        public Dictionary<Vector2, float> Weights { get; }

        /// <summary>
        /// Returns the mean vector of all direction with weights considered.
        /// </summary>
        public Vector2 Mean
        {
            get
            {
                float totalWeight = 0f;
                Vector2 totalVector = Vector2.Zero;

                foreach (Vector2 dir in Weights.Keys)
                {
                    totalVector += dir;
                    totalWeight += Weights[dir];
                }

                return totalVector / totalWeight;
            }
        }
    }
}