using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space.World
{
    public class Sector
    {
        public GameObject mainObject;

        public void load()
        {
            mainObject = Root.instance.RootObject.createChild("sector");
        }

        public void unload()
        {
            mainObject.destroy();
            mainObject = null;
        }

    }
}
