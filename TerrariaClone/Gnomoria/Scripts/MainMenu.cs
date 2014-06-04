using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Gnomoria.Scripts
{
    public class MainMenu : Script
    {
        private void ongui()
        {
            //if (gui.button(new Vector2(300, 200), "new game"))
            {
                rootObject.broadcast("loadmap");

                this.enabled = false;
            }
        }
    }
}
