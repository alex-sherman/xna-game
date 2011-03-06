using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Learning
{
    class AIManager
    {
        public delegate void EnvironmentModifiedCallback();
        EnvironmentModifiedCallback BlockAdded, BlockRemoved;

        World world;

        public AIManager(World world)
        {
            this.world = world;
        }

        public Vector3 getHeading(EnemyAgent agent, Vector3 target)
        {
            // TODO implement actual AI.
            Vector3 diff = target - agent.Position;
            diff.Normalize();
            return diff;
        }

        protected void generateNavigationMesh() { }
    }
}
