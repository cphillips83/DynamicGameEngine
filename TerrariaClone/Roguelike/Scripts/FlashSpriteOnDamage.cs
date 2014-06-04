using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Roguelike.Scripts
{
    public class FlashSpriteOnDamage : Script
    {
        public float flashDuration = 0.25f;
        public float fps = 10f;
        public float transparency = 0.5f;

        private Sprite sprite = null;
        private float flashTimer = 0f;
        private float flashToggle = 0f;
        private bool toggled = false;

        private void init()
        {
            sprite = gameObject.getScript<Sprite>();
        }

        private void fixedupdate()
        {
            //if (Root.instance.input.IsLeftMouseDown)
            //    takedamage(10);

            if (flashTimer > 0f)
            {
                var root = Root.instance;

                flashTimer -= root.time.deltaTime;
                flashToggle -= root.time.deltaTime;

                if (flashToggle <= 0f)
                {
                    toggled = !toggled;
                    if (toggled)
                        flashOn();
                    else
                        flashOff();

                    flashToggle = 0.5f / fps;
                }
            }
            else
                flashOff();
        }

        private void flashOn()
        {
            if (sprite != null)
            {
                var color = sprite.color;
                color.A = (byte)(255 * transparency);
                sprite.color = color;
            }
        }

        private void flashOff()
        {
            if (sprite != null)
            {
                var color = sprite.color;
                color.A = 255;
                sprite.color = color;
            }
        }

        private void applydamage(GameObject go, float amount)
        {
            if (flashTimer <= 0)
            {
                flashTimer = flashDuration;
                flashToggle = 0.5f / fps;
                toggled = true;
                flashOn();
            }

            flashTimer = flashDuration;
        }
    }
}
