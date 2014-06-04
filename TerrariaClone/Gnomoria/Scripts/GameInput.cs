using GameName1.Gnomoria.Scripts.World;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Gnomoria.Scripts
{
    public class GameInput :Script
    {
        private WorldManager world;

        private void init()
        {
            world = rootObject.getScript<WorldManager>();
        }

        private void update()
        {
            if (world.isLoaded)
            {
                if (input.WasKeyPressed(Keys.D1))
                    rootObject.broadcast("picktool", "digwall");
            }
        }
    }
}
