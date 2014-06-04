using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite.Managers;

namespace zSprite.Systems
{
    public interface ISystem
    {
        void init();
        void update();
        void render();
        void destroy();
    }
}
