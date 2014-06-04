using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    //public class Weapon : Script
    //{
    //    public float minRange { get { return 0; } }


    //    private void fire()
    //    {

    //    }
    //}


    public class AIWanderAggresive : Script
    {
        public enum AIState
        {
            LookingForTarget,
            ChasingTarget,
            Targeting,
            Returning
        }

        public float distance = 6f;
        public float radius = 8f;
        public float change = 1f;
        public Vector2 around;
        public float aroundRadius = 200f;
        private float health = 250;
        private float maxHealth = 250;

        public TargetFilters friendlyFilters;
        public TargetFilters attackFilters;

        public float aggressionRange = 1000f;
        public float followDistance = 1000f;

        private AIState _aistate = AIState.LookingForTarget;
        private GameObject _target;
        private Vector2 _lookAt;
        private float _targetAcquistionTimer = 0.5f;
        private float _targetAcquistion = 0.5f;
        private Vector2 _returnLocation;
        private ParticleEmitter engineEmitter;
        private int moveState = 0;
        private float moveTimer = 1f;

        private TargetableObjects _targetObjects;
        private Ship _ship;


        private void init()
        {

            if (aroundRadius != 0 && around == Vector2.Zero)
                around = transform.DerivedPosition;

            _targetObjects = rootObject.getScript<TargetableObjects>();
            _targetObjects.add(gameObject, friendlyFilters);

            _ship = gameObject.getScript<Ship>();

            setupEngineEmitter();
        }

        private void render()
        {
            //draw the health bar
            if (health < maxHealth)
            {
                var g = Root.instance.graphics;
                var p = transform.DerivedPosition;
                var s = new Vector2(p.X - 50, p.Y);
                var e = new Vector2(p.X + 50, p.Y);
                var l = (float)health / (float)maxHealth;

                var bg = AxisAlignedBox.
                    FromRect(s.X, s.Y - 3, 50, 4);
                var fg = AxisAlignedBox.
                    FromRect(s.X, s.Y - 3, 50 * l, 4);

                g.Draw(null, bg, Color.White);
                g.Draw(null, fg, Color.Red);
            }

            gui.label(transform.DerivedPosition, moveState.ToString());
        }
        private bool shit = false;
        private void processlogic()
        {
            if (_ship != null)
            {
                //return;
                var from = transform.DerivedPosition;
                switch (_aistate)
                {
                    case AIState.LookingForTarget:
                        {
                            _targetAcquistionTimer -= time.deltaTime;
                            if (_targetAcquistionTimer <= 0f)
                            {
                                _targetAcquistionTimer = _targetAcquistion;

                                var targets = _targetObjects.get(from, aggressionRange, attackFilters);
                                var target = targets.OrderByDescending(x => x.transform.DerivedPosition.LengthSquared()).FirstOrDefault();

                                if (target != null)
                                {
                                    _aistate = AIState.ChasingTarget;
                                    _target = target;
                                    _returnLocation = from;
                                    break;
                                }
                            }

                            if (aroundRadius > 0f)
                            {
                                var arSQ = aroundRadius * aroundRadius;
                                var dstSQ = (around - from).LengthSquared();

                                _ship.steering.seek(from, around, dstSQ / arSQ);
                                _ship.steering.wander(from, distance, radius, change);
                            }
                            else
                            {
                                _ship.steering.wander(from, distance, radius, change);
                            }

                            _lookAt = transform.DerivedPosition + _ship.steering.velocity * 300f;
                            engineEmitter.emitParticles(30, 50, 0.1f);
                            break;
                        }
                    case AIState.ChasingTarget:
                        {
                            if (_target.destroyed)
                            {
                                _target = null;
                                _aistate = AIState.Returning;
                                break;
                            }

                            var targetPosition = _target.transform.DerivedPosition;
                            var followDistanceSq = followDistance * followDistance;
                            var direction = (from - targetPosition);
                            var distanceSq = direction.LengthSquared();

                            if (followDistanceSq < distanceSq)
                            {
                                _aistate = AIState.Returning;
                                _target = null;
                                break;
                            }

                            if (distanceSq < 50 * 50)
                            {
                                //moveState = 3;
                                //_ship.steering.flee(from, targetPosition);
                            }
                            
                            if (distanceSq > 550 * 550)
                            {
                                shit = false;
                                moveState = 2;
                                //direction.Normalize();
                                //targetPosition += direction * 300f;
                                _ship.steering.seek(from, targetPosition);
                                engineEmitter.emitParticles(30, 50, 0.1f);
                            }
                            else
                            {
                                moveTimer -= time.deltaTime;
                                if (moveTimer <= 0f)
                                {
                                    moveState = random.Next(5);
                                    moveTimer = random.Next(1f, 2f);
                                    _ship.steering.seek(from, transform.DerivedRight * 300f);
                                }
                                else if (random.Next(6) == 0)
                                {
                                    _ship.steering.seek(from, transform.DerivedRight * -300f);
                                }

                                switch (moveState)
                                {
                                    case 0: _ship.steering.seek(from, transform.DerivedRight * 300f); break;
                                    case 1: _ship.steering.seek(from, -transform.DerivedRight * 300f); break;
                                    case 2: _ship.steering.seek(from, transform.DerivedForward * 300f); break;
                                    case 3: _ship.steering.seek(from, -transform.DerivedForward * 300f); break;
                                    case 4: _ship.steering.wander(from, 6f, 10f, change); break;
                                    default:
                                        break;
                                }

                                //else if (random.Next(10) == 0)
                                //{
                                //    _ship.steering.seek(from, transform.DerivedForward * 300f);
                                //}
                                //else if (random.Next(5) == 0)
                                //{
                                //    _ship.steering.seek(from, transform.DerivedForward * -300f);
                                //}
                                //else if (random.Next(5) == 0)
                                //else if (random.Next(15) == 0)
                                //{
                                //    var arSQ = aroundRadius * aroundRadius;
                                //    var dstSQ = (around - from).LengthSquared();

                                //    //_ship.steering.seek(from, targetPosition, dstSQ / arSQ);
                                //    _ship.steering.wander(from, 6f, 10f, change);
                                //}


                                this.gameObject.broadcast("tryfire");

                            }
                            _lookAt = targetPosition;



                            break;
                        }
                    case AIState.Returning:
                        {
                            var distanceSq = (from - _returnLocation).LengthSquared();
                            var returnThresholdSq = 150 * 150;

                            if (distanceSq < returnThresholdSq)
                            {
                                _aistate = AIState.LookingForTarget;
                                break;
                            }

                            _ship.steering.seek(from, _returnLocation);
                            _lookAt = _returnLocation;
                            engineEmitter.emitParticles(30, 50, 0.1f);
                            break;
                        }
                }


                _ship.steering.steering += -seperation(20f) * 0.1f;
                transform.DerivedPosition = _ship.steering.integrate(transform.DerivedPosition, _ship.mass);
                lookAt(from, _lookAt);


                //var sep = seperation(10f);
                //_ship.steering.seek(transform.DerivedPosition, transform.DerivedPosition + _ship.steering.truncate(sep, _ship.steering.maxVelocity));
                //transform.DerivedPosition = _ship.steering.integrate(transform.DerivedPosition, _ship.mass);

            }
        }

        private float desiredRotation = 0.025f;
        public float maxTurnSpeed = 0.265f;
        //public float maxTurnSpeed = 0.065f;
        private void lookAt(Vector2 from, Vector2 target)
        {

            desiredRotation = from.GetRotation(target);// -MathHelper.PiOver2;
            //transform.Orientation = desiredRotation;
            var rotation = MathHelper.WrapAngle(transform.DerivedOrientation);
            var rotationAmount = MathHelper.WrapAngle(desiredRotation - rotation);

            //if (Math.Abs(rotationAmount) < maxTurnSpeed)
            //    transform.Orientation = desiredRotation;
            //else 
            if (Math.Abs(rotationAmount) < 1.5f)
            {
                var mtsq = ((Math.Abs(rotationAmount) / 1.5f) ) ;
                //var mtsq = ((Math.Abs(rotationAmount) / 1.5f) + 1) / 2f;
                //var mstq = random.Next(0.5f, 0.75f);
                transform.Orientation += (MathHelper.Clamp(rotationAmount, -maxTurnSpeed, maxTurnSpeed) * mtsq) ;
            }
            else
            {
                //rotationAmount = MathHelper.Clamp(rotationAmount, -0.05f, 0.05f);
                transform.Orientation += MathHelper.Clamp(rotationAmount, -maxTurnSpeed, maxTurnSpeed) ;
            }
        }

        private Vector2 seperation(float radius)
        {
            var radiusSq = radius * radius;
            var from = transform.DerivedPosition;
            var targets = _targetObjects.get(from, radius, TargetFilters.Aliens);
            var count = 0;
            var modifier = Vector2.Zero;
            foreach (var t in targets)
            {
                //var ship = t.getScript<Ship>();
                //if (ship != null)
                //{
                //    modifier += ship.steering.velocity;
                //    count++;
                //}
                modifier += (t.transform.DerivedPosition - from);
            }

            if (count == 0)
                return modifier;

            return (modifier / count).ToNormalized();
        }


        private void setupEngineEmitter()
        {
            var file = "content/textures/flare/star.png";
            var material = resources.
                createMaterialFromTexture(file);

            //material.transparency = Color.Black;
            material.SetBlendState(BlendState.Additive);

            var engineEmitterGO = gameObject.createChild("engine");
            engineEmitterGO.transform.Position = new Vector2(-15, 0);
            engineEmitter = engineEmitterGO.createScript<ParticleEmitter>();

            engineEmitter.MinEnergy = 0.5f;
            engineEmitter.MaxEnergy = 0.5f;
            engineEmitter.Size = new Vector2(16, 16);
            engineEmitter.RandomScale = 0.1f;
            engineEmitter.EndSizeScale = 0.01f;
            engineEmitter.FadeTime = 0f;
            engineEmitter.RandomFade = 0f;
            //engineEmitter.UseParentWorldSpace = true;
            engineEmitter.DirectionStart = MathHelper.Pi - 0.25f;
            engineEmitter.DirectionEnd = MathHelper.Pi + 0.25f;
            engineEmitter.UseParentWorldSpace = true;
            //damageParticleEmitter.MinRotationSpeed = 0.1f;
            //damageParticleEmitter.MaxRotationSpeed = 0.2f;
            engineEmitter.StartColor = new Color(0.2f, 0.05f, 0.05f, 1f);
            engineEmitter.EndColor = new Color(0.2f, 0.05f, 0.05f, 1f);
            engineEmitter.RandomVelocity = 0.5f;
            engineEmitter.Velocity = 1;

            engineEmitter.material = material;
        }

        private void destroyed()
        {
        }

        private void takedamage(float damage)
        {
            health -= damage;

            if (health <= 0)
            {
                _targetObjects.remove(gameObject, TargetFilters.Aliens);
                gameObject.destroy();
                //add explosion effect back in
                //var sound = resources.createSoundFromFile("content/sounds/explosions/5.wav");
                //sound.Play();

                var go = rootObject.createChild();
                go.transform.Position = transform.DerivedPosition;

                var particles = go.
                    createScript<ParticleEmitter>();

                particles.MinEnergy = 0.25f;
                particles.MaxEnergy = 0.5f;
                particles.Size = new Vector2(32, 32);
                particles.EndSizeScale = 0.5f;
                particles.MinRotationSpeed = 0.1f;
                particles.MaxRotationSpeed = 0.2f;
                particles.StartColor = Color.Red;
                particles.EndColor = Color.Yellow;
                particles.RandomVelocity = 2f;
                particles.Velocity = 3;
                particles.emitParticles(50, 100, 1f);

                var file = "content/textures/streak.png";
                var material = resources.
                    createMaterialFromTexture(file);

                material.SetBlendState(BlendState.Additive);

                particles.material = material;

                var destroy = go.createScript<DestroyTimer>();
                destroy.duration = 3f;


                //var random = new Random();
                //var powerup = (PowerUpType)random.Next(0, 3);
                //var po = rootObject.createChild();
                //var ps = po.createScript<PowerUp>();
                //ps.type = powerup;

            }
            else
            {
                //particles.emitParticles(20, 50, 0.1f);
                //var sound = resources.createSoundFromFile("content/sounds/misc/laserd.wav");
                //var sound = resources.createSoundFromFile("content/sounds/explosions/1.wav");
                //sound.Play();

            }
        }
    }
}
