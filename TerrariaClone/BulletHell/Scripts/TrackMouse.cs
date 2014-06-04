using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.BulletHell.Scripts
{
    public class TrackMouse : Script
    {
        private void update()
        {
            transform.Position = mainCamera.screenToWorld(input.MousePosition);
        }
    }
}
