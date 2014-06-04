using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;
using zSprite.Resources;
using Microsoft.Xna.Framework.Graphics;

namespace GameName1.Space
{
    public class SimpleLaser : Script
    {
        private bool isfiring = false;

        public float length = 200f;
        public Color color = Color.White;
        public bool isaltfire = false;
        public TargetFilters targetFilters;
        public TargetableObjects targetObjects;
        public float currentLength = 0f;
        private GameObject particleEmitterLocation;
        private ParticleEmitter damageParticleEmitter;
        private float damageTimer = 0.1f;
        private float damageTimerRemaining = 0.25f;
        private float fireTimer = 2f;
        private float fireTimerRemaining = 2f;

        private void init()
        {
            targetObjects = rootObject.getScript<TargetableObjects>();

            particleEmitterLocation = rootObject.createChild("laseremitter");
            damageParticleEmitter = particleEmitterLocation.
             createScript<ParticleEmitter>();

            damageParticleEmitter.MinEnergy = 0.20f;
            damageParticleEmitter.MaxEnergy = 0.40f;
            damageParticleEmitter.Size = new Vector2(14, 14);
            damageParticleEmitter.RandomScale = 0.2f;
            damageParticleEmitter.EndSizeScale = 0.7f;
            damageParticleEmitter.MinRotationSpeed = 0.1f;
            damageParticleEmitter.MaxRotationSpeed = 0.2f;
            damageParticleEmitter.StartColor = color;
            damageParticleEmitter.EndColor = Color.Black;
            damageParticleEmitter.RandomVelocity = 1f;
            damageParticleEmitter.Velocity = 1;

            var file = "content/textures/flares2/x4.png";
            var material = resources.
                createMaterialFromTexture(file);

            material.SetBlendState(BlendState.Additive);

            damageParticleEmitter.material = material;
        }

        private void fire()
        {
            //if (!isaltfire)
            dofire(true);
        }

        private void altfire()
        {
            //if (isaltfire)
            dofire(true);
        }

        private void tryfire()
        {
            //if (!isaltfire)
            dofire(false);
        }

        private void dofire(bool force)
        {
            if (!isfiring)
            {
                var sp = transform.DerivedPosition;
                var ep = sp + transform.DerivedForward * length;
                var lp = new LineSegment(sp, ep);
                var hastarget = targetObjects.get(lp, 20f, targetFilters).Any();

                if (!hastarget && !force)
                    return;

                isfiring = true;
                damageTimerRemaining = 0f;
                fireTimerRemaining = fireTimer;
            }
        }

        private void afterfixedupdate()
        {
            if (isfiring)
            {
                fireTimerRemaining -= time.deltaTime;

                if (fireTimerRemaining < 0f)
                {
                    isfiring = false;
                    return;
                }

                if (damageTimerRemaining > 0f)
                    damageTimerRemaining -= time.deltaTime;

                var sp = transform.DerivedPosition;
                var ep = sp + transform.DerivedForward * length;
                var lp = new LineSegment(sp, ep);
                var target = targetObjects.get(lp, 20f, targetFilters).OrderBy(x => (x.transform.DerivedPosition - sp).LengthSquared()).FirstOrDefault();

                if (fireTimerRemaining > 0.5f)
                {
                    currentLength = length;
                    if (target != null)
                    {
                        currentLength = (sp - target.transform.DerivedPosition).Length();
                        var loc = sp + transform.DerivedForward * currentLength;
                        if (damageTimerRemaining <= 0f)
                        {
                            damageTimerRemaining = damageTimer;
                            var scrolling = rootObject.getScript<ScrollingText>();
                            scrolling.emit(loc, "1");
                            target.broadcast("takedamage", 1f);

                            damageParticleEmitter.emitParticles(30, 50, 0.1f);
                        }
                        particleEmitterLocation.transform.Position = loc;
                    }
                }
            }
            //var enemySprite = target.getScriptWithChildren<Sprite>();
            //var aabb = AxisAlignedBox.FromDimensions(target.transform.DerivedPosition, enemySprite.size);
            //if (aabb.Intersects(transform.DerivedPosition))
            //{
            //    target.sendMessage("takedamage", damage);
            //    gameObject.destroy();
            //}
        }

        private float width = 2f;
        private bool dir = true;
        private void render()
        {
            if (isfiring)
            {
                var materal = resources.createMaterialFromTexture("content/textures/flares2/pearlring.png");
                //var materal = resources.createMaterialFromTexture("content/textures/laser.png");
                materal.SetBlendState(BlendState.Additive);
                materal.SetSamplerState(SamplerState.AnisotropicWrap);

                var materal2 = resources.createMaterialFromTexture("content/textures/flares2/i0.png");
                //var materal = resources.createMaterialFromTexture("content/textures/laser.png");
                materal2.SetBlendState(BlendState.Additive);
                materal2.SetSamplerState(SamplerState.AnisotropicWrap);

                var t = 1f;
                if (fireTimerRemaining < 0.5f)
                    t = fireTimerRemaining;

                var _color = new Color(color.R / 255f, color.G / 255f, color.B, color.A / 255f * t);

                if (transform != null && isfiring)
                {
                    if (dir)
                    {
                        width += random.Next(1.5f, 1.75f);
                        if (width > 5f)
                            dir = !dir;
                    }
                    else
                    {
                        width -= random.Next(1.5f, 1.75f);
                        if (width < 4f)
                            dir = !dir;
                    }
                    //var width = random.Next(2f, 5f);
                    var width2 = random.Next(8f, 12f);
                    graphics.DrawLine(Game.LAYER_MAIN2, materal, transform.DerivedPosition, transform.DerivedPosition + transform.DerivedForward * currentLength, _color, width / 1.2f, 0);
                    graphics.DrawLine(Game.LAYER_MAIN2, materal, transform.DerivedPosition, transform.DerivedPosition + transform.DerivedForward * currentLength, _color, width / 1.2f, 0);
                    graphics.DrawLine(Game.LAYER_MAIN2, materal, transform.DerivedPosition, transform.DerivedPosition + transform.DerivedForward * currentLength, _color, 12f, 0);
                }
            }
        }
    }

}
