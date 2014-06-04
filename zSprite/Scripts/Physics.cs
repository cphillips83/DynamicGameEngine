using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite.Collections;

namespace zSprite.Scripts
{
    public class Physics : Script//, IQuadObject
    {
        public AxisAlignedBox Bounds { get; private set; }

        private Transform _transform;
        
        private void init()
        {
            _transform = gameObject.transform2();
            //GameEngine.physicsQuadTree.Insert(this);
        }

        private void update()
        {
            
        }

        private void destroy()
        {
            //GameEngine.physicsQuadTree.Remove(this);
        }
    }
}
