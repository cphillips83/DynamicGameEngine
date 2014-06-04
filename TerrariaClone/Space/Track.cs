using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.Space
{
    public class Track : Script
    {
        private Transform _transform;

        public float speed = 1f;
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
                    {
                        //var p0 = _transform.DerivedPosition;
                        var frameSpeed = speed * Root.instance.time.deltaTime;
                        var p = _other.DerivedPosition;
                        var targetRotation = _transform.GetRotationTo(p, TransformSpace.World);
                        var rotation = _transform.DerivedOrientation;

                        var rotationAmount = (targetRotation - rotation);
                        if (rotationAmount > MathHelper.Pi)
                            rotationAmount -= MathHelper.Pi * 2;

                        var absRotationAmount = Math.Abs(rotationAmount);

                        if (absRotationAmount > frameSpeed)
                        {
                            if (rotationAmount < 0)
                                rotationAmount = -frameSpeed;
                            else
                                rotationAmount = frameSpeed;
                        }

                        _transform.Orientation += rotationAmount;
                    }
                }
            }
        }
    }
}
