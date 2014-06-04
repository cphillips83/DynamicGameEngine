using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;

namespace GameName1.Space
{
    public class AIControllerOld : Script
    {
        private static Random random = new Random();

        private Transform _transform;
        private Vector2 velocityY;
        private float maxYSpeed = 75f;
        private float accY = 0.5f;

        private Track _tracker = null;
        private LinearVelocity _thruster = null;

        private SimpleGun _gun;

        private float fireTimer = 0.5f;
        private float _fireTimerDelay = 0.5f;
        public float fireTimerDelay
        {
            get { return _fireTimerDelay; }
            set
            {
                _fireTimerDelay = value;
                fireTimer = value;
            }
        }

        private void init()
        {
            _transform = this.gameObject.transform2();
            _tracker = this.gameObject.getScript<Track>();
            _tracker.speed = (float)(random.NextDouble() * 1) + 0.5f;

            _thruster = this.gameObject.getScript<LinearVelocity>();
            _thruster.maxYSpeed = random.Next(100, 500);
            _thruster.accY = (float)random.NextDouble();

            var turret = this.gameObject.createChild();
            {
                turret.transform.Position = new Vector2(0, -7.5f);
                _gun = turret.createScript<SimpleGun>();
                //_gun.targetName = "player";
                _gun.color = Color.Red;
            }

        }

        private void update()
        {
            if (_tracker != null && _thruster != null && _transform != null && _tracker.gameObjectId > -1)
            {
                var go = Root.instance.find(_tracker.gameObjectId);
                if (go != null)
                {
                    var _other = go.transform2();
                    if (_other != null)
                    {
                        //var len = (_transform.DerivedPosition - _other.DerivedPosition).LengthSquared();
                        //if (len < 50 * 50)
                        //    _thruster.enabled = false;
                        //else
                        //    _thruster.enabled = true;
                    }
                }
            }
        }

        private void fixedupdate()
        {
            var dirY = _transform.DerivedForward;

            if (dirY != Vector2.Zero)
            {
                dirY.Normalize();
                velocityY += dirY * accY * Root.instance.time.deltaTime;

                if (velocityY.LengthSquared() > 1)
                {
                    //acquires new heading faster
                    velocityY += dirY * accY * Root.instance.time.deltaTime * 10f;
                    velocityY.Normalize();
                }
            }

            _transform.Position += velocityY * maxYSpeed * Root.instance.time.deltaTime;

            fireTimer -= time.deltaTime;

            if (fireTimer <= 0f)
            {
                fireTimer = fireTimerDelay;
                this.gameObject.sendMessageDown("fire");
            }
        }
    }
}
