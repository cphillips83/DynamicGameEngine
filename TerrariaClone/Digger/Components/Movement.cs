using GameName1.Digger.World;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Scripts;

namespace GameName1.Digger.Components
{
    public class Movement : Script
    {
        public float AirAcc = 0.20375f;
        public float AirDrag = 0.95875f;
        public float gravity = 0.31875f;
        public float GroundAcc = 0.186875f;
        public float GroundFriction = 0.646875f;
        public float GroundTurnSpeed = 0.75f;
        public float JumpSpeed = 7.5f;
        public float MaxAcceleration = 6.5f;
        public float maxGravity = 8;
        public Vector2 speed = Vector2.Zero;
        private bool _actionJump = false;
        private bool _actionMoveLeft = false;
        private bool _actionMoveRight = false;
        private AxisAlignedBox _bounds;
        private bool _hasJumped = false;
        private Terrain _terrain;
        private bool _touchingFeet = false;
        private bool _touchingHead = false;
        private bool _touchingSide = false;

        //private Rigidbody _rigidbody;
        private Transform _transform;

        private void fixedupdate()
        {
            var velocity = speed;
            var groundTurnSpeed = GroundTurnSpeed;
            var groundFriction = GroundFriction;
            var groundAcc = GroundAcc;
            var airAcc = AirAcc;
            var airDrag = AirDrag;
            var groundMaxAcc = MaxAcceleration;

            if (_actionJump)
            {
                if (_touchingFeet)
                {
                    speed.Y = -JumpSpeed;
                    _hasJumped = true;
                    _touchingFeet = false;
                }
            }
            else if (speed.Y < -4 && _hasJumped)
            {
                speed.Y = -4;
            }

            if (_actionMoveLeft)
            {
                if (!_touchingFeet)
                {
                    if (velocity.X > -groundMaxAcc)
                        velocity.X -= airAcc;
                    if (velocity.Y < 0 && velocity.Y > -4 && Math.Abs(velocity.X) > 0.125)
                        velocity.X *= airDrag;
                }
                else if (velocity.X > 0)
                    velocity.X -= groundTurnSpeed;
                else if (velocity.X > -groundMaxAcc)
                {
                    velocity.X -= groundAcc;

                    if (velocity.X < -groundMaxAcc)
                        velocity.X = -groundMaxAcc;
                }
            }

            if (_actionMoveRight)
            {
                if (!_touchingFeet)
                {
                    if (velocity.X < groundMaxAcc)
                        velocity.X += airAcc;
                    if (velocity.Y < 0 && velocity.Y > -4 && Math.Abs(velocity.X) > 0.125)
                        velocity.X *= airDrag;
                }
                else if (velocity.X < 0)
                    velocity.X += groundTurnSpeed;
                else if (velocity.X < groundMaxAcc)
                {
                    velocity.X += groundAcc;

                    if (velocity.X > groundMaxAcc)
                        velocity.X = groundMaxAcc;
                }
            }

            if (!_actionMoveLeft && !_actionMoveRight)
            {
                velocity.X -= Math.Min((float)Math.Abs(velocity.X), groundFriction) * Math.Sign(velocity.X);
            }

            speed.X = velocity.X;

            var np = _terrain.move(_bounds, speed, out _touchingFeet, out _touchingSide, out _touchingHead);
            _transform.Position = np;
            _bounds.Center = np;

            if (_touchingFeet || _touchingHead)
            {
                speed.Y = 0;
                _hasJumped = false;
            }
            else if (speed.Y < maxGravity)
            {
                speed.Y += gravity;
                if (speed.Y > maxGravity)
                    speed.Y = maxGravity;
            }

            if (_touchingSide)
                speed.X = 0;

            _actionMoveLeft = false;
            _actionMoveRight = false;
            _actionJump = false;
        }

        private void init()
        {
            //_rigidbody = gameObject.getComponent<Rigidbody>();
            _terrain = Root.instance.RootObject.getScript<Terrain>();
            _transform = gameObject.transform2();
            _bounds = AxisAlignedBox.FromDimensions(_transform.DerivedPosition, new Vector2(32, 48));
        }

        private void jump()
        {
            _actionJump = true;
        }

        private void left()
        {
            _actionMoveLeft = true;
        }

        private void render()
        {
            Root.instance.graphics.DrawRect(19, _bounds, Color.Purple);
            var font = Root.instance.resources.findFont("fonts/arial.fnt");
            Root.instance.graphics.DrawText(10, font, 1f, new Vector2(0, 0), string.Format("speed: {0}", speed), Color.White);
        }

        private void right()
        {
            _actionMoveRight = true;
        }
    }
}