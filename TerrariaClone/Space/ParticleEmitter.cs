using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;
using zSprite.Collections;
using zSprite.Resources;

namespace GameName1.Space
{
    public class ParticleEmitter : Script
    {

        public struct Particle
        {
            public Vector2 Position;
            public Vector2 Direction;
            public float Velocity;
            public float Life;
            public float MaxLife;
            public float Rotation;
            public float RotationSpeed;
            public float FadeTime;
            public float LifeDuration { get { return 1f - Life / MaxLife; } }
            public Vector2 Size;
            public Color Color;
        }

        public Vector2 Size = Vector2.Zero;
        public float RandomScale = 1f;
        public float MinEnergy = 3f;
        public float MaxEnergy = 3f;
        public int MinEmission = 50;
        public int MaxEmission = 50;
        public float MinRotationSpeed = 0f;
        public float MaxRotationSpeed = 0f;
        public float RandomAngle = 0f;
        public float RandomFade = 0.1f;
        public float FadeTime = 0.5f;
        public float RandomVelocity = 1f;
        public float Velocity = 1f;
        public float DirectionStart = 0f;
        public float DirectionEnd = MathHelper.TwoPi;
        public Vector2 EmitArea = Vector2.Zero;

        public bool UseParentWorldSpace = false;
        public Color StartColor = Color.White;
        public Color EndColor = new Color(1f, 1f, 1f, 0f);
        public float EndSizeScale = 1f;
        public float DisableAfterSecods = 0f;
        public bool EmitParticles = false;

        private ObjectPool<Particle> _particlePool = new ObjectPool<Particle>();
        private List<int> _activeParticles = new List<int>();

        private float nextEmit = 0f;
        public Material material = null;
        private float disableTimer = 0f;

        private void update()
        {
            if (EmitParticles)
            {
                var dt = time.deltaTime;

                if (disableTimer > 0f)
                    disableTimer -= dt;

                if (disableTimer < 0f)
                {
                    disableTimer = 0f;
                    EmitParticles = false;
                    return;
                }

                if (nextEmit > 0)
                    nextEmit -= dt;

                while (nextEmit <= 0)
                {
                    nextEmit += 1f / random.Next(MinEmission, MaxEmission);
                    emitParticle();
                }
            }
        }

        public void emitParticles(int min, int max, float duration)
        {
            MinEmission = (int)(min / duration);
            MaxEmission = (int)(max / duration);
            disableTimer = duration;
            EmitParticles = true;
        }

        private void fixedupdate()
        {
            if (input.IsLeftMouseDown)
                this.emitParticles(20000, 20000, 1);

            for (int i = _activeParticles.Count - 1; i >= 0; i--)
            {
                var index = _activeParticles[i];
                var particle = _particlePool[index];

                particle.Life -= time.deltaTime;

                if (particle.Life <= 0)
                {
                    _particlePool.FreeItem(index);
                    _activeParticles[i] = _activeParticles[_activeParticles.Count - 1];
                    _activeParticles.RemoveAt(_activeParticles.Count - 1);
                }
                else
                {
                    particle.Position += particle.Direction * particle.Velocity;
                    particle.Rotation += particle.RotationSpeed;
                }

                _particlePool[index] = particle;
            }
        }

        private void render()
        {
            if (material != null)
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

                    var wr = p.Rotation;
                    var ws = p.Size.Lerp(EndSizeScale * Vector2.One, p.LifeDuration);
                    var wz = 0f;
                    if(transform != null)
                        wz = this.gameObject.transform.DerivedDepth;
                    var origin = Vector2.One / 2;

                    graphics.Draw(material, wp, AxisAlignedBox.Null, color, (float)wr, origin, ws, SpriteEffects.None, wz);
                    //graphics.Draw(material, wp, Color.Wheat);
                }
            }
        }

        private int emitParticle()
        {
            var index = _particlePool.GetFreeItem();
            _activeParticles.Add(index);

            var p = _particlePool[index];
            p.Velocity = Velocity;
            p.Size = Size + Size * (((float)random.NextDouble() * RandomScale) - RandomScale * 0.5f);
            if (p.Size == Vector2.Zero)
                p.Size += material.textureSize;

            p.Life = (float)(random.NextDouble() * (MaxEnergy - MinEnergy) + MinEnergy);
            p.MaxLife = p.Life;
            p.Position.X = (float)(random.NextDouble() * EmitArea.X) - EmitArea.X / 2;
            p.Position.Y = (float)(random.NextDouble() * EmitArea.Y) - EmitArea.Y / 2;
            if (!UseParentWorldSpace && transform != null)
                p.Position += transform.Position;

            p.Velocity += (float)(random.NextDouble() * RandomVelocity * 2) - RandomVelocity;
            p.RotationSpeed = (float)(random.NextDouble() * (MaxRotationSpeed - MinRotationSpeed) + MinRotationSpeed);
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
            _particlePool[index] = p;
            return index;
        }
    }
}
