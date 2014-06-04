using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class SteeringAI : Script
    {
        public static bool debug = true;

        private SteeringClass _steering = new SteeringClass();
        private SteeringClass _steering2 = new SteeringClass();
        

        private Vector2 mouse;
        private void fixedupdate()
        {
            _steering2.maxVelocity = 5;
            var from = transform.DerivedPosition;
            var target = mainCamera.screenToWorld(input.MousePosition);
            //var velocity = _steering.truncate((target - lastMouse), _steering.maxVelocity);
            _steering2.seek(mouse, target);

            var distance = from.LengthSquared();
            var maxDistanceSq = 400 * 400;
            _steering.seek(from, Vector2.Zero, distance / maxDistanceSq);
            _steering.wander(from, 6, 8, 1f, 1f);

            //_steering.flee(from, Vector2.Zero);
            //_steering.arrive(from, target, 100, 1F);

            //if (transform.DerivedPosition.LengthSquared() < 200 * 200)
            //{
            //    _steering.flee(from, Vector2.Zero);
            //    _steering.seek(from, target, 0.5f);
            //}
            //else
            //{
            //    _steering.seek(from, target);
            //}

            if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                //_steering.evade(from, mouse, _steering2.velocity);
                //_steering.seek(from, target);
                //_steering.wander2(from, 6f, 8f, 1f);
                //_steering.flee(from, target);
                _steering.arrive(from, target, 200f, 1f);
            //wander(1000);
            //else
            //_steering.wander(from, 20f, 6f, 8f, 1f);
            //_steering.wander(from, target, 100f, 100, 1);

            transform.Position = _steering.integrate(transform.DerivedPosition, 20f);
            mouse = _steering2.integrate(mouse, 15f);

            this.gameObject.transform.Forward = _steering.velocity;

            //_steering.fixedupdate(transform);

            //            lastMouse = target;

        }


        private void render()
        {
        //    graphics.DrawCircle(null, mouse, 5, 5, Color.Pink);
        //    graphics.DrawCircle(null, Vector2.Zero, 20, 20, Color.Pink);
        //    graphics.DrawCircle(null, transform.DerivedPosition, 10, 10, Color.White);
        //    if (_steering.desiredVelocity != Vector2.Zero)
        //        graphics.DrawLine(transform.DerivedPosition, transform.DerivedPosition + _steering.desiredVelocity * 100, Color.Green);

        //    if (_steering.velocity != Vector2.Zero)
        //        graphics.DrawLine(transform.DerivedPosition, transform.DerivedPosition + _steering.velocity * 100, Color.Red);

        //    if (_steering.lastSteering != Vector2.Zero)
        //        graphics.DrawLine(transform.DerivedPosition, transform.DerivedPosition + _steering.lastSteering.ToNormalized() * 100, Color.Pink);
        }

    }


    //public class SteeringAI : Script
    //{
    //    public static bool debug = true;

    //    private Steering _steering = new Steering();
    //    private Steering _steering2 = new Steering();

    //    private Vector2 mouse;
    //    private void fixedupdate()
    //    {
    //        _steering2.maxVelocity = 5;
    //        var from = transform.DerivedPosition;
    //        var target = mainCamera.screenToWorld(input.MousePosition);
    //        //var velocity = _steering.truncate((target - lastMouse), _steering.maxVelocity);
    //        _steering2.seek(mouse, target);

    //        var distance = from.LengthSquared();
    //        var maxDistanceSq = 400 * 400;
    //        _steering.seek(from, Vector2.Zero, distance / maxDistanceSq);
    //        _steering.wander(from, 6, 8, 1f, 1f);

    //        //if (transform.DerivedPosition.LengthSquared() < 40 * 40)
    //        //{
    //        //    _steering.flee(from, Vector2.Zero);
    //        //    _steering.seek(from, target, 0.5f);
    //        //}
    //        //else
    //        //{
    //        //    _steering.seek(from, target);
    //        //}

    //        if (input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
    //            //_steering.evade(from, mouse, _steering2.velocity);
    //            //_steering.seek(from, target);
    //            //_steering.wander2(from, 6f, 8f, 1f);
    //            //_steering.flee(from, target);
    //            _steering.arrive(from, target, 200f, 1f);
    //        //wander(1000);
    //        //else
    //        //_steering.wander(from, 20f, 6f, 8f, 1f);
    //        //_steering.wander(from, target, 100f, 100, 1);

    //        transform.Position = _steering.integrate(transform.DerivedPosition, 20f);
    //        mouse = _steering2.integrate(mouse, 15f);
    //        //_steering.fixedupdate(transform);

    //        //            lastMouse = target;

    //    }


    //    private void render()
    //    {
    //        graphics.DrawCircle(null, mouse, 5, 5, Color.Pink);
    //        graphics.DrawCircle(null, Vector2.Zero, 20, 20, Color.Pink);
    //        graphics.DrawCircle(null, transform.DerivedPosition, 10, 10, Color.White);
    //        if (_steering.desiredVelocity != Vector2.Zero)
    //            graphics.DrawLine(transform.DerivedPosition, transform.DerivedPosition + _steering.desiredVelocity * 100, Color.Green);

    //        if (_steering.velocity != Vector2.Zero)
    //            graphics.DrawLine(transform.DerivedPosition, transform.DerivedPosition + _steering.velocity * 100, Color.Red);

    //        if (_steering.lastSteering != Vector2.Zero)
    //            graphics.DrawLine(transform.DerivedPosition, transform.DerivedPosition + _steering.lastSteering.ToNormalized() * 100, Color.Pink);
    //    }

    //}
}
