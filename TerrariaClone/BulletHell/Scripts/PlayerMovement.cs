﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using zSprite;

namespace GameName1.BulletHell.Scripts
{
    public class PlayerMovement : Script
    {
        public float speed = 8f;
        public Vector2 velocity;
        public float mass = 10f;
        public float maxForce = 5.4f;
        public float drag = 0.94f;

        private void fixedupdate()
        {
            var p = transform.Position;

            var hasInput = input.IsAnyKeyDown(Keys.A, Keys.S, Keys.D, Keys.W);
            if (hasInput)
            {
                //var steering = Steering.seek(speed, p, p + velocity);
                var steering = Steering.zero;
                

                if (input.IsKeyDown(Keys.A)) steering += Steering.seek(speed, p, p + new Vector2(-1, 0));
                if (input.IsKeyDown(Keys.D)) 
                    steering += Steering.seek(speed, p, p + new Vector2(1, 0));
                if (input.IsKeyDown(Keys.W)) steering += Steering.seek(speed, p, p + new Vector2(0, -1));
                if (input.IsKeyDown(Keys.S)) steering += Steering.seek(speed, p, p + new Vector2(0, 1));

                velocity = steering.integrate(p, velocity, maxForce, speed, mass);
            }
            else
            {
                velocity *= drag;
                if (velocity.X >= -0.01 && velocity.X <= 0.01)
                    velocity.X = 0f;

                if (velocity.Y >= -0.01 && velocity.Y <= 0.01)
                    velocity.Y = 0f;
            }

            p += velocity;
            transform.Position = p;
        }
    }
}
