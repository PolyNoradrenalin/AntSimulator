﻿namespace AntEngine.Colliders
{
    /// <summary>
    /// Collider representing the engineWorld.
    /// </summary>
    public class WorldCollider : Collider
    {
        protected override bool checkCollision(CircleCollider circleCollider)
        {
            //TODO : Implement
            throw new System.NotImplementedException();
        }

        protected override bool checkCollision(RectangleCollider rectCollider)
        {
            //TODO : Implement
            throw new System.NotImplementedException();
        }

        protected override bool checkCollision(WorldCollider worldCollider)
        {
            //TODO : Implement
            throw new System.NotImplementedException();
        }
    }
}