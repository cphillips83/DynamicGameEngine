using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class Settings : Script
    {
        public int SectorHalfSize = 4096 / 2;
        public int SectorSize = 4096;

        private void render()
        {
            graphics.DrawRect(19, AxisAlignedBox.FromDimensions(Vector2.Zero, SectorSize * Vector2.One), Color.Pink);
        }

    }
}
