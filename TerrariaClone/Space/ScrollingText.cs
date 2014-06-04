using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Collections;
using zSprite.Resources;

namespace GameName1.Space
{
    public class ScrollingText : Script
    {
        private static Random random = new Random();

        public struct Particle
        {
            public Vector2 Position;
            public Vector2 Direction;
            public float Velocity;
            public float Life;
            public float MaxLife;
            public string text;
            //public float Rotation;
            //public float RotationSpeed;
            public float FadeTime;
            public float LifeDuration { get { return 1f - Life / MaxLife; } }
            public float Scale;
            public Color Color;
        }

        public int renderQueue = 0;
        public float Scale = 2f;
        public float RandomScale = 0f;
        public float MinEnergy = 1f;
        public float MaxEnergy = 1.5f;
        //public int MinEmission = 50;
        //public int MaxEmission = 50;
        //public float MinRotationSpeed = 0f;
        //public float MaxRotationSpeed = 0f;
        public float RandomAngle = 0f;
        public float RandomFade = 0.1f;
        public float FadeTime = 0.5f;
        public float RandomVelocity = 0f;
        public float Velocity = 1f;
        public float DirectionStart = -MathHelper.PiOver2;
        public float DirectionEnd = -MathHelper.PiOver2;
        public Vector2 EmitArea = Vector2.Zero;

        public bool UseParentWorldSpace = false;
        public Color StartColor = Color.White;
        public Color EndColor = new Color(1f, 1f, 1f, 0f);
        public float EndSizeScale = 0.75f;

        private ObjectPool<Particle> _particlePool = new ObjectPool<Particle>();
        private List<int> _activeParticles = new List<int>();

        public BmFont font = null;

        public void emit(Vector2 p, string text, params object[] args)
        {
            emit(p, string.Format(text, args));
        }

        public void emit(Vector2 p, string text)
        {
            emitParticle(p, text);
        }

        private void fixedupdate()
        {
            for (int i = _activeParticles.Count - 1; i >= 0; i--)
            {
                var index = _activeParticles[i];
                var particle = _particlePool[index];

                particle.Life -= time.deltaTime;

                if (particle.Life <= 0)
                {
                    _particlePool.FreeItem(index);
                    _activeParticles.RemoveAt(i);
                }
                else
                {
                    particle.Position += particle.Direction * particle.Velocity;
                    //particle.Rotation += particle.RotationSpeed;
                }

                _particlePool[index] = particle;
            }
        }

        private void render()
        {
            for (int i = _activeParticles.Count - 1; i >= 0; i--)
            {
                var index = _activeParticles[i];
                var p = _particlePool[index];

                var color = StartColor.Lerp(EndColor, p.LifeDuration);

                var a = 1f;
                if (p.LifeDuration > 1f - FadeTime + p.FadeTime)
                    a = (1f - p.LifeDuration) / (1f - (FadeTime + p.FadeTime));


                //float a = p.LifeDuration * (FadeTime + p.FadeTime);
                color.A = (byte)(a * 255);

                var wp = p.Position;
                if (UseParentWorldSpace && transform != null)
                    wp += transform.DerivedPosition;

                //var wr = p.Rotation;
                var ws = Utility.Lerp(p.Scale, EndSizeScale, p.LifeDuration);
                //var ws = p.Scale.Lerp(EndSizeScale * Vector2.One, p.LifeDuration);
                var wz = 0f;
                if (transform != null)
                    wz += transform.DerivedDepth;

                var origin = Vector2.One / 2;

                gui.label(renderQueue, ws, wp, font, wz, color, p.text);
                //graphics.Draw(material, wp, AxisAlignedBox.Null, color, (float)wr, origin, ws, SpriteEffects.None, wz);
                //graphics.Draw(material, wp, Color.Wheat);
            }
        }

        private int emitParticle(Vector2 position, string text)
        {
            var index = _particlePool.GetFreeItem();
            _activeParticles.Add(index);

            var p = _particlePool[index];

            p.Velocity = Velocity;
            p.Scale = Scale + Scale * (((float)random.NextDouble() * RandomScale) - RandomScale * 0.5f);

            p.Life = (float)(random.NextDouble() * (MaxEnergy - MinEnergy) + MinEnergy);
            p.MaxLife = p.Life;
            p.Position.X = (float)(random.NextDouble() * EmitArea.X) - EmitArea.X / 2 + position.X;
            p.Position.Y = (float)(random.NextDouble() * EmitArea.Y) - EmitArea.Y / 2 + position.Y;
            if (!UseParentWorldSpace && transform != null)
                p.Position += transform.Position;

            p.Velocity += (float)(random.NextDouble() * RandomVelocity * 2) - RandomVelocity;
            //p.RotationSpeed = (float)(random.NextDouble() * (MaxRotationSpeed - MinRotationSpeed) + MinRotationSpeed);
            //p.Velocity.Y += (float)(random.NextDouble() * RandomVelocity.Y);
            var rotation = (float)((random.NextDouble() * (DirectionEnd - DirectionStart)) + DirectionStart);
            if (transform != null)
                rotation += transform.DerivedOrientation;

            if (RandomAngle > 0)
                rotation += (float)(random.NextDouble() * RandomAngle * 2) - RandomAngle;

            p.Direction = new Vector2(Utility.Cos(rotation), Utility.Sin(rotation));
            //p.Direction.Normalize();

            //p.Direction = transform.DerivedForward;

            p.FadeTime = (float)(random.NextDouble() * RandomFade * 2) - RandomFade;
            p.text = text;
            _particlePool[index] = p;
            return index;
        }
    }
}
