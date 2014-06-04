using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class Follow : Script
    {
        private Transform _transform;

        public int gameObjectId = -1;

        private void init()
        {
            _transform = this.gameObject.transform2();
        }

        private void update()
        {
            if (_transform != null && gameObjectId > -1)
            {
                var go = Root.instance.find(gameObjectId);
                if (go != null)
                {
                    var _other = go.transform2();
                    if (_other != null)
                        _transform.DerivedPosition = _other.DerivedPosition;
                }
            }
        }
    }
}
