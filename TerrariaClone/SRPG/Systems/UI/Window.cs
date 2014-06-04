using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.SRPG.Systems.UI
{
    public class Window : Script
    {
        public int width = 100;
        public int height = 100;
        public Color color = new Color(Color.Black, 0.15f);
        public int renderQueue = 0;

        private void render()
        {
            Root.instance.graphics.Draw(
                renderQueue, 
                AxisAlignedBox.FromRect(gameObject.transform.DerivedPosition, new Vector2(width, height)), 
                color, 
                gameObject.transform.DerivedDepth);
        }
        
 
    }
}
