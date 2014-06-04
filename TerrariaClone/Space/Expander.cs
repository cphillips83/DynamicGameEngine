using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.Space
{
    public class Expander : Script
    {

        private Transform _transform;
        
        public float maxSize = 1.5f;
        public float speed = 1.25f;

        private bool growing = true;
        private float currentSize = 1f;

        private void init()
        {
            _transform = gameObject.transform2();
        }

        private void update()
        {
            if (_transform != null)
            {
                if (growing)
                {
                    currentSize += Root.instance.time.deltaTime * speed;
                    if (currentSize > maxSize)
                    {
                        currentSize = maxSize;
                        growing = false;
                    }
                }
                else
                {
                    currentSize -= Root.instance.time.deltaTime * speed;
                    if (currentSize < 1f)
                    {
                        currentSize = 1f;
                        growing = true;
                    }
                }
                _transform.Scale = Vector2.One * currentSize;
            }
        }
    }
}
