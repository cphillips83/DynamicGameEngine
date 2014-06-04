using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;
using zSprite.Resources;

namespace GameName1.Space
{
    public class ShowDirection : Script
    {
        private Transform _transform;
        private Material _material;

        public float length = 10f;
        public Color color = Color.White;

        private void init()
        {
            _transform = this.gameObject.transform2();
        }

        private void render()
        {
            if (_transform != null)
                Root.instance.graphics.DrawLine( _transform.DerivedPosition, _transform.DerivedPosition + _transform.DerivedForward * length, color);
        }
    }
}
