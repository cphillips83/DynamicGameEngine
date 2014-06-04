using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class Rotate : Script
    {
        private Transform _transform;

        public float speed = 2f;

        private void init()
        {
            _transform = this.gameObject.transform2();
        }

        private void update()
        {
            if (_transform != null)
                _transform.Orientation += Root.instance.time.deltaTime * speed;
        }
    }
}
