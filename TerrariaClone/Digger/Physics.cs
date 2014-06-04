using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Digger
{
    public enum PhysicsCollisionType
    {

    }

    public struct PhysicsCollision
    {
        public AxisAlignedBox collisions;
    }

    public class Physics : Script
    {
        private Transform _transform;
        
        public AxisAlignedBox collisions;

        private void init()
        {
            _transform = this.gameObject.transform2();
        }



    }
}
