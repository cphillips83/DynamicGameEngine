using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class TrackMouse : Script
    {
        private Transform _transform;

        public float maxDistance = 0f;

        private void init()
        {
            _transform = this.gameObject.transform2();
        }

        private void update()
        {
            if (_transform != null)
            {
                var mp = Camera.mainCamera.screenToWorld(Root.instance.input.MousePosition);
                _transform.DerivedPosition = mp;

                if (maxDistance > 0f && (maxDistance * maxDistance) < _transform.Position.LengthSquared())
                {
                    var dir = _transform.Position.ToNormalized();
                    _transform.Position = dir * maxDistance;
                }
            }
        }

    }
}
