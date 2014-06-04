using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.PI.Scripts
{
    public class Player : Script
    {
        public int maxElectronics = 100;
        public int maxOrganisms = 100;
        public int maxFuel = 100;
        public int electronics = 0;
        public int organisms = 0;
        public int fuel = 0;
        public int credits = 100;

        private void init()
        {

        }

        private void render()
        {
            //fuck you ads

            var g = Root.instance.graphics;

        }
    }
}
