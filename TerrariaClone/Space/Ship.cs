using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class Ship : Script
    {
        public SteeringClass steering = new SteeringClass();
        public float mass = 20f;
        public TargetFilters targetType;
        public float size = 1f;
        public TargetableObjects targetObjects;

        private void init()
        {
            targetObjects = rootObject.getScript<TargetableObjects>();
        }

        private float desiredRotation = 0f;
        private float rotation = 0f;
        private float rotationAmount = 0f;
        private void fixedupdate()
        {
            gameObject.sendMessage("processlogic");

            ////steering.integrate(transform.DerivedPosition, mass);
            ////transform.Position = steering.integrate(transform.DerivedPosition, mass);
            //var mouse = mainCamera.screenToWorld(input.MousePosition);
            ////var dir = mainCamera.screenToWorld(mouse).ToNormalized();

            //rotation = MathHelper.WrapAngle(transform.DerivedOrientation);
            //desiredRotation = transform.DerivedPosition.GetRotation(mouse) - MathHelper.PiOver2;
            //rotationAmount = MathHelper.WrapAngle(rotation - desiredRotation);

            //if (Math.Abs(rotationAmount) < 0.05f)
            //    transform.Orientation = desiredRotation;
            //else
            //{
            //    //rotationAmount = MathHelper.Clamp(rotationAmount, -0.05f, 0.05f);
            //    transform.Orientation += MathHelper.Clamp(rotationAmount, -0.05f, 0.05f); ;
            //}
            //transform.Forward = steering.direction;
            //MathHelper.WrapAngle(

        }

        //private void render()
        //{
        //    gui.label(transform.DerivedPosition, rotation.ToString());

        //}
    }
}
