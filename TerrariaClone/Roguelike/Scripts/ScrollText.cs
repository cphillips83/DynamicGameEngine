using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class ScrollText : Script
    {
        public Color color = Color.White;
        public float duration = 1.5f;
        public float durationTimer = 0f;
        public Color fadeColor = new Color(Color.White, 0.25f);
        public int renderQueue = 0;
        public Vector2 scrollSpeed = new Vector2(0, -0.75f);
        public string text = null;

        private void fixedupdate()
        {
            if (durationTimer < 0f)
            {
                gameObject.destroy();
                return;
            }

            gameObject.transform.DerivedPosition += scrollSpeed;
            durationTimer -= Root.instance.time.deltaTime;
        }

        private void init()
        {
            durationTimer = duration;
        }

        private void render()
        {
            if (!string.IsNullOrEmpty(text))
            {
                var font = Root.instance.resources.findFont("content/fonts/arial.fnt");
                var lerp = 1f - durationTimer / duration;
                var lerpedColor = color.Lerp(fadeColor, lerp);
                Root.instance.graphics.DrawText(renderQueue, font, 1f, gameObject.transform.DerivedPosition, text, lerpedColor);
                //Root.instance.graphics.DrawRect(renderQueue, null, null, AxisAlignedBox.FromDimensions(gameObject.transform.DerivedPosition, Vector2.One * 20), lerpedColor);
            }
        }
    }
}