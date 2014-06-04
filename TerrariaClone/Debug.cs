using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1
{
    public class Debug : Script
    {

        private void ongui()
        {
            Root.instance.gui.label(0, 1f, AxisAlignedBox.FromRect(new Vector2(0, 10), new Vector2(100, 10)), null, 0f, Color.Red, "0");
            Root.instance.gui.label(Vector2.Zero, "0");
        }

    }
}
