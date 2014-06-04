//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using Microsoft.Xna.Framework;
//using zSprite;

//namespace GameName1.Space
//{
//    public class Engine : Script
//    {
//        private Transform _transform;
//        private Vector2 velocityY;
//        private Vector2 velocityX;
//        private float accX = 1f;
//        private float accY = 1.25f;
//        private float maxYSpeed = 200f;
//        private float maxXSpeed = 100f;

//        private void init()
//        {
//            _transform = this.gameObject.transform();
//        }

//        private void update()
//        {
//            _transform.Position += (velocityX + velocityY) * Root.instance.time.deltaTime;
//        }

//        private void fixedupdate()
//        {
//            var dirY = Vector2.Zero;
//            var dirX = Vector2.Zero;
//            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
//                dirY += ship.DerivedForward;

//            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
//                dirY += ship.DerivedBackward;

//            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
//                dirX += ship.DerivedRight;

//            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
//                dirX += ship.DerivedLeft;


//            if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
//            {
//                velocityX *= 0.95f;
//                velocityY *= 0.95f;

//                if (velocityX.LengthSquared() < 0.01f)
//                    velocityX = Vector2.Zero;

//                if (velocityY.LengthSquared() < 0.01f)
//                    velocityY = Vector2.Zero;

//            }

//            if (dirY != Vector2.Zero)
//            {
//                dirY.Normalize();
//                velocityY += dirY * accY * Root.instance.time.deltaTime;

//                if (velocityY.LengthSquared() > 1)
//                {
//                    //acquires new heading faster
//                    velocityY += dirY * accY * Root.instance.time.deltaTime * 2f;
//                    velocityY.Normalize();
//                }
//            }

//            if (dirX != Vector2.Zero)
//            {
//                dirX.Normalize();
//                velocityX += dirX * accX * Root.instance.time.deltaTime;

//                if (velocityX.LengthSquared() > 1)
//                {
//                    //acquires new heading faster
//                    velocityX += dirX * accX * Root.instance.time.deltaTime * 2f;
//                    velocityX.Normalize();
//                }
//            }

//            _transform.Position += velocityY * maxYSpeed * Root.instance.time.deltaTime;
//            _transform.Position += velocityX * maxXSpeed * Root.instance.time.deltaTime;
//            velocityX *= 0.95f;
//        }

//        private void render()
//        {
//            //Root.instance.graphics.DrawLine(1, Root.instance.time.Find("basewhite"), DerivedPosition, DerivedPosition + DerivedForward * 20, Color.White);
//            Root.instance.graphics.DrawText(1, Fonts.find("fonts/arial.fnt"), 1f, _transform.DerivedPosition, velocityY.ToString(), Color.White);
//        }




//        //private Transform _transform;
//        ////private 

//        ////public Vector2 thrustConstraint = Vector2.One;
//        //public Vector2 velocity = Vector2.Zero;
//        //public Vector2 force = Vector2.Zero;
//        //public float forwardThrust = 100f;
//        //public float backwardThrust = 100f;
//        //public float sideThrust = 50f;
//        ////public float mps = 100f;

//        //private void init()
//        //{
//        //    _transform = this.gameObject.transform();
//        //}

//        //private void thrustforward()
//        //{
//        //    force += Transform.forward * forwardThrust;
//        //}

//        //private void thrustbackward()
//        //{
//        //    force += Transform.backward * backwardThrust;
//        //}

//        //private void thrustleft()
//        //{
//        //    force += Transform.left * sideThrust;
//        //}

//        //private void thrustright()
//        //{
//        //    force += Transform.right * sideThrust;
//        //}

//        //private void fixedupdate()
//        //{
//        //    //if(force == Vector2.Zero)
//        //    //    force 
//        //}
//    }
//}
