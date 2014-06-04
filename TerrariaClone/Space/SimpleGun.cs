using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using zSprite;
using zSprite.Resources;
using Microsoft.Xna.Framework.Graphics;
using zSprite.Collections;

namespace GameName1.Space
{
    public class SimpleGun : Script
    {
        public struct Bullet
        {
            public float damage;
            public float energy;
            public float totalEnergy;
            public float fadeOut;
            public float totalFadeOut;
            public float velocity;
            public float rotation;
            public Vector2 position;
            public Vector2 direction;
        }

        public Color color = Color.White;
        public float fireRate = 0.1f;
        public float fireTimer = 0f;
        public float damage = 1f;
        public float velocity = 2f;
        public float energy = 5f;

        public bool isaltfire = false;

        private ObjectPool<Bullet> _bulletPool = new ObjectPool<Bullet>();
        private List<int> _activeBullets = new List<int>();
        private Material _lineMaterial;
        private Material _lineCapMaterial;

        public TargetFilters targetFilters;
        private TargetableObjects _targets;

        private void init()
        {
            //_transform = this.gameObject.transform();
            //_material = Root.instance.resources.findMaterial("basewhite");
            _targets = rootObject.getScript<TargetableObjects>();
            _lineMaterial = resources.createMaterialFromTexture("content/textures/line.png");
            _lineMaterial.SetBlendState(BlendState.Additive);


            _lineCapMaterial = resources.createMaterialFromTexture("content/textures/linecap.png");
            _lineCapMaterial.SetBlendState(BlendState.Additive);


            //_material.SetBlendState(new BlendState()
            //                            {
            //                                AlphaBlendFunction = BlendFunction.Add,
            //                                ColorBlendFunction = BlendFunction.Add,
            //                                AlphaDestinationBlend = Blend.One,
            //                                AlphaSourceBlend = Blend.One,
            //                                ColorDestinationBlend = Blend.One,
            //                                ColorSourceBlend = Blend.One
            //                            });

        }

        private void fixedupdate()
        {
            if (fireTimer > 0f)
                fireTimer -= time.deltaTime;

            for (int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                var index = _activeBullets[i];
                var bullet = _bulletPool[index];

                if (bullet.energy > 0f)
                    bullet.energy -= time.deltaTime;
                else if (bullet.fadeOut > 0f)
                    bullet.fadeOut -= time.deltaTime;

                if (bullet.energy <= 0 && bullet.fadeOut <= 0)
                {
                    _bulletPool.FreeItem(index);
                    _activeBullets[i] = _activeBullets[_activeBullets.Count - 1];
                    _activeBullets.RemoveAt(_activeBullets.Count - 1);
                }
                else
                {
                    bullet.position += bullet.direction * bullet.velocity;

                    //check for collision
                    var target = _targets.get(bullet.position, 40, targetFilters).FirstOrDefault();
                    if (target != null)
                    {
                        bullet.energy = 0;
                        bullet.fadeOut = 0;
                        _bulletPool.FreeItem(index);
                        _activeBullets[i] = _activeBullets[_activeBullets.Count - 1];
                        _activeBullets.RemoveAt(_activeBullets.Count - 1);
                        target.broadcast("takedamage", damage);
                        var scrolling = rootObject.getScript<ScrollingText>();
                        scrolling.emit(bullet.position, damage.ToString());

                    }
                }

                _bulletPool[index] = bullet;
            }
        }

        private void render()
        {
            for (int i = _activeBullets.Count - 1; i >= 0; i--)
            {
                var index = _activeBullets[i];
                var bullet = _bulletPool[index];
                var A = (bullet.position + bullet.direction * 4).Floor();
                var B = (bullet.position - bullet.direction * 4).Floor();


                var tangent = B - A;
                var rotation = (float)Math.Atan2(tangent.Y, tangent.X);

                const float ImageThickness = 12;
                float thicknessScale = 1f / ImageThickness;
                //float thicknessScale = 128f;

                //Vector2 capOrigin = new Vector2(1f, 0.5f);
                //Vector2 middleOrigin = new Vector2(0, 0.5f);
                //Vector2 middleScale = new Vector2(tangent.Length(), 256);
                Vector2 capOrigin = new Vector2(1f, 0.5f);
                Vector2 middleOrigin = new Vector2(0, 0.5f);
                Vector2 middleScale = new Vector2(tangent.Length(), 1f / thicknessScale);
                Vector2 capScale = new Vector2(64, 1f / thicknessScale);

                var c = new Color(color, bullet.fadeOut / bullet.totalFadeOut);
                graphics.Draw(Game.LAYER_MAIN2, _lineMaterial, A, AxisAlignedBox.Null, c, rotation, middleOrigin, middleScale, SpriteEffects.None, 0f);
                graphics.Draw(Game.LAYER_MAIN2, _lineCapMaterial, A, AxisAlignedBox.Null, c, rotation, capOrigin, 1f / thicknessScale, SpriteEffects.None, 0f);
                graphics.Draw(Game.LAYER_MAIN2, _lineCapMaterial, B, AxisAlignedBox.Null, c, rotation + MathHelper.Pi, capOrigin, 1f / thicknessScale, SpriteEffects.None, 0f);


                //graphics.Draw(Game.LAYER_MAIN2, _lineMaterial, bullet.position, AxisAlignedBox.FromRect(Vector2.Zero, Vector2.One * 128), Color.White, bullet.rotation, Vector2.One / 2f, Vector2.One * 32, SpriteEffects.None, 0f);
                //graphics.Draw(Game.LAYER_MAIN2, _lineCapMaterial, bullet.position, AxisAlignedBox.FromRect(Vector2.Zero, Vector2.One * 128), c, bullet.rotation, Vector2.One / 2f, Vector2.One * 32, SpriteEffects.None, 0f);
                //graphics.Draw(Game.LAYER_MAIN2, _lineCapMaterial, bullet.position, AxisAlignedBox.FromRect(Vector2.Zero, Vector2.One * 128), c, bullet.rotation, Vector2.One / 2f, Vector2.One * 32, SpriteEffects.None, 0f);
            }
        }

        private void tryfire()
        {
            if (fireTimer <= 0f)
            {
                var sp = transform.DerivedPosition;
                var ep = sp + transform.DerivedForward * 800f;
                var lp = new LineSegment(sp, ep);
                var hastarget = _targets.get(lp, 40f, targetFilters).Any();

                if (hastarget)
                    fire();
            }
        }

        private void createBullet(Vector2 p, float rotation)
        {

            var index = _bulletPool.GetFreeItem();
            _activeBullets.Add(index);

            var bullet = _bulletPool[index];
            bullet.position = p;
            bullet.rotation = rotation;
            bullet.direction = new Vector2(Utility.Cos(rotation), Utility.Sin(rotation));
            //bullet.energy = 0.5f;
            //bullet.totalEnergy = 0.5f;
            //bullet.fadeOut = 0.1f;
            //bullet.totalFadeOut = 0.1f;
            //bullet.velocity = 10f;
            bullet.energy = energy;
            bullet.totalEnergy = energy;
            bullet.fadeOut = 0.1f;
            bullet.totalFadeOut = 0.1f;
            bullet.velocity = velocity;
            bullet.damage = damage;

            _bulletPool[index] = bullet;
        }

        private void fire()
        {
            if (fireTimer <= 0f)
            {
                fireTimer = fireRate;
                createBullet(transform.DerivedPosition, transform.DerivedOrientation);
                if (targetFilters != TargetFilters.Aliens)
                {
                    createBullet(transform.DerivedPosition, transform.DerivedOrientation - 0.25f);
                    createBullet(transform.DerivedPosition, transform.DerivedOrientation + 0.25f);
                }
            }
        }

        private void altfire()
        {
            fire();
        }
    }

}
