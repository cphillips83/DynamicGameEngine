using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using zSprite;

namespace GameName1.Space
{
    public class PlayerController : Script
    {
        public Transform ship;

        private ParticleEmitter damageParticleEmitter;
        private ParticleEmitter engineEmitter;

        public float damageModifier = 1f;
        public float defenseModifier = 1f;

        public float health = 5000f;
        public float maxHealth = 5000f;

        private Vector2 velocity;
        private TargetableObjects _targetObjects;
        private SteeringClass _steering = new SteeringClass();
        private Settings _settings;

        private void init()
        {
            _settings = rootObject.getScript<Settings>();

            _steering.maxVelocity = 3;
            _targetObjects = rootObject.getScript<TargetableObjects>();
            _targetObjects.add(gameObject, TargetFilters.Player);

            damageParticleEmitter = gameObject.
                createScript<ParticleEmitter>();

            damageParticleEmitter.MinEnergy = 0.25f;
            damageParticleEmitter.MaxEnergy = 0.5f;
            damageParticleEmitter.Size = new Vector2(16, 16);
            damageParticleEmitter.RandomScale = 0.5f;
            damageParticleEmitter.EndSizeScale = 0.5f;
            damageParticleEmitter.MinRotationSpeed = 0.1f;
            damageParticleEmitter.MaxRotationSpeed = 0.2f;
            damageParticleEmitter.StartColor = Color.Green;
            damageParticleEmitter.EndColor = Color.Blue;
            damageParticleEmitter.RandomVelocity = 1f;
            damageParticleEmitter.Velocity = 1;

            var file = "content/textures/streak.png";
            var material = resources.
                createMaterialFromTexture(file);

            material.SetBlendState(BlendState.Additive);

            damageParticleEmitter.material = material;

            setupEngineEmitter();
        }

        private void beforeupdate()
        {
            if (Root.instance.input.IsLeftMouseDown)
                this.gameObject.sendMessageDown("fire");
            else if (Root.instance.input.IsRightMouseDown)
                this.gameObject.sendMessageDown("altfire");
        }

        private void fixedupdate()
        {
            processMovement();

            var x = Utility.Clamp(transform.Position.X, _settings.SectorHalfSize, -_settings.SectorHalfSize);
            var y = Utility.Clamp(transform.Position.Y, _settings.SectorHalfSize, -_settings.SectorHalfSize);
            transform.Position = new Vector2(x, y);

        }

        private float maxTurnSpeed = 0.2f;
        private float desiredRotation;
        private void lookAt(Vector2 from, Vector2 target)
        {

            desiredRotation = from.GetRotation(target);// -MathHelper.PiOver2;
            //transform.Orientation = desiredRotation;
            var rotation = MathHelper.WrapAngle(transform.DerivedOrientation);
            var rotationAmount = MathHelper.WrapAngle(desiredRotation - rotation);

            if (Math.Abs(rotationAmount) < maxTurnSpeed)
                transform.Orientation = desiredRotation;
            else
            {
                //rotationAmount = MathHelper.Clamp(rotationAmount, -0.05f, 0.05f);
                transform.Orientation += MathHelper.Clamp(rotationAmount, -maxTurnSpeed, maxTurnSpeed); ;
            }
        }

        private void processMovement()
        {
            //if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
            //    moveLeft = true;
            //if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
            //    moveRight = true;
            var from = transform.DerivedPosition;
            var to = mainCamera.screenToWorld(input.MousePosition);
            lookAt(from, to);

            if (Root.instance.input.IsKeyDown(Keys.W))
            {
                _steering.seek(transform.DerivedPosition, transform.DerivedPosition + transform.DerivedForward * 100, 1f);
                engineEmitter.emitParticles(30, 50, 0.1f);
            }
            else if (input.IsKeyDown(Keys.A))
                _steering.seek(transform.DerivedPosition, transform.DerivedPosition + transform.DerivedRight * -100, 1f);
            else if (input.IsKeyDown(Keys.D))
                _steering.seek(transform.DerivedPosition, transform.DerivedPosition + transform.DerivedRight * 100, 1f);

            if (input.IsKeyDown(Keys.LeftShift))
            {
                _steering.maxVelocity = 6f;

                engineEmitter.MinEnergy = 1f;
                engineEmitter.MaxEnergy = 1f;
            }
            else
            {
                _steering.maxVelocity = 5f;
                engineEmitter.MinEnergy = 0.5f;
                engineEmitter.MaxEnergy = 0.5f;

            }

            transform.Position = _steering.integrate(transform.DerivedPosition, 20f);


            //if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
            //    moveDown = true;

            //if(moveLeft || moveRight || moveUp || moveDown)
            //engineEmitter.emitParticles(50, 100, 0.1f);
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
            engineEmitter.StartColor = new Color(0.05f, 0.05f, 0.2f, 1f);
            engineEmitter.EndColor = new Color(0.05f, 0.05f, 0.2f, 1f);
            engineEmitter.RandomVelocity = 0.5f;
            engineEmitter.Velocity = 1;

            engineEmitter.material = material;
        }

        private void render()
        {
            //Root.instance.graphics.DrawLine(1, Root.instance.time.Find("basewhite"), DerivedPosition, DerivedPosition + DerivedForward * 20, Color.White);
            //Root.instance.graphics.DrawText(1, Root.instance.resources.findFont("content/fonts/arial.fnt"), 1f, _transform.DerivedPosition, transform.DerivedOrientation.ToString(), Color.White);
        }

        private void beforefixedupdate()
        {
            //var gun = this.gameObject.getScriptWithChildren<SimpleGun>();
            //gun.damage = damageModifier;

            if (Root.instance.input.IsLeftMouseDown)
                this.gameObject.broadcast("fire");
            if (Root.instance.input.IsRightMouseDown)
                this.gameObject.sendMessageDown("altfire");
        }

        private void takedamage(float dmg)
        {
            health -= (dmg / defenseModifier);

            if (health <= 0)
            {
                gameObject.destroy();
                //add explosion effect back in
                var sound = resources.createSoundFromFile("content/sounds/explosions/5.wav");
                sound.Play();

                var go = rootObject.createChild();
                go.transform.Position = transform.DerivedPosition;

                var explosionParticleEmitter = go.
                    createScript<ParticleEmitter>();

                explosionParticleEmitter.MinEnergy = 0.25f;
                explosionParticleEmitter.MaxEnergy = 0.5f;
                explosionParticleEmitter.Size = new Vector2(16, 16);
                explosionParticleEmitter.EndSizeScale = 0.5f;
                explosionParticleEmitter.MinRotationSpeed = 0.1f;
                explosionParticleEmitter.MaxRotationSpeed = 0.2f;
                explosionParticleEmitter.StartColor = Color.Red;
                explosionParticleEmitter.EndColor = Color.Yellow;
                explosionParticleEmitter.RandomVelocity = 2f;
                explosionParticleEmitter.Velocity = 3;
                explosionParticleEmitter.emitParticles(50, 100, 1f);

                var file = "content/textures/streak.png";
                var material = resources.
                    createMaterialFromTexture(file);

                material.SetBlendState(BlendState.Additive);

                explosionParticleEmitter.material = material;

                var destroy = go.createScript<DestroyTimer>();
                destroy.duration = 3f;

            }
            else
            {
               // damageParticleEmitter.emitParticles(20, 50, 0.1f);
               // var sound = resources.createSoundFromFile("content/sounds/misc/laserd.wav");
                //var sound = resources.createSoundFromFile("content/sounds/explosions/1.wav");
               // sound.Play();

            }
        }

        private void ongui()
        {
            var font = resources.findFont("content/fonts/arial.fnt");
            var screenSize = screen.size;

            //draw the health bar
            var g = Root.instance.graphics;
            //var p = transform.DerivedPosition;
            var p = new Vector2(20, screenSize.Y - 40);
            var s = new Vector2(p.X, p.Y);
            var e = new Vector2(p.X + 200, p.Y);
            var l = (float)health / (float)maxHealth;

            var bg = AxisAlignedBox.FromRect(s.X, s.Y - 3, 200, 20);
            var fg = AxisAlignedBox.FromRect(s.X, s.Y - 3, 200 * l, 20);

            g.Draw(null, bg, Color.White);
            g.Draw(null, fg, Color.Red);

            //graphics.DrawText(font, 1f, new Vector2(100, 100), "test", Color.Red);
        }
    }





    //public class PlayerController : Script
    //{
    //    public Transform ship;

    //    private Transform _transform;
    //    private ParticleEmitter damageParticleEmitter;
    //    private ParticleEmitter engineEmitter;

    //    public float damageModifier = 1f;
    //    public float defenseModifier = 1f;
    //    public float GroundTurnSpeed = 0.4f;
    //    public float GroundFriction = 0.2046875f;
    //    public float GroundAcc = 0.186875f;
    //    public float MaxAcceleration = 3.5f;
    //    public float Gravity = 0.31875f;

    //    public float health = 100f;
    //    public float maxHealth = 100f;

    //    private Vector2 velocity;
    //    private TargetableObjects _targetObjects;

    //    private void init()
    //    {
    //        _targetObjects = rootObject.getScript<TargetableObjects>();
    //        _targetObjects.add(gameObject, TargetFilters.Player);

    //        _transform = this.gameObject.transform2();

    //        damageParticleEmitter = gameObject.
    //            createScript<ParticleEmitter>();

    //        damageParticleEmitter.MinEnergy = 0.25f;
    //        damageParticleEmitter.MaxEnergy = 0.5f;
    //        damageParticleEmitter.Size = new Vector2(16, 16);
    //        damageParticleEmitter.RandomScale = 0.5f;
    //        damageParticleEmitter.EndSizeScale = 0.5f;
    //        damageParticleEmitter.MinRotationSpeed = 0.1f;
    //        damageParticleEmitter.MaxRotationSpeed = 0.2f;
    //        damageParticleEmitter.StartColor = Color.Green;
    //        damageParticleEmitter.EndColor = Color.Blue;
    //        damageParticleEmitter.RandomVelocity = 1f;
    //        damageParticleEmitter.Velocity = 1;

    //        var file = "content/textures/streak.png";
    //        var material = resources.
    //            createMaterialFromTexture(file);

    //        material.SetBlendState(BlendState.Additive);

    //        damageParticleEmitter.material = material;

    //        setupEngineEmitter();
    //    }

    //    private void beforeupdate()
    //    {
    //        if (Root.instance.input.IsLeftMouseDown)
    //            this.gameObject.sendMessageDown("fire");
    //        else if (Root.instance.input.IsRightMouseDown)
    //            this.gameObject.sendMessageDown("altfire");
    //    }

    //    private void fixedupdate()
    //    {
    //        processMovement();

    //        _transform.Position += velocity;
    //    }

    //    private void processMovement()
    //    {
    //        var groundTurnSpeed = GroundTurnSpeed;
    //        var groundFriction = GroundFriction;
    //        var groundAcc = GroundAcc;
    //        var groundMaxAcc = MaxAcceleration;

    //        var moveLeft = false;
    //        var moveRight = false;
    //        var moveUp = false;
    //        var moveDown = false;

    //        if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.A))
    //            moveLeft = true;
    //        if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.D))
    //            moveRight = true;
    //        if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.W))
    //            moveUp = true;
    //        if (Root.instance.input.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.S))
    //            moveDown = true;

    //        if (moveLeft)
    //        {
    //            if (velocity.X > 0)
    //                velocity.X -= groundTurnSpeed;
    //            else if (velocity.X > -groundMaxAcc)
    //            {
    //                velocity.X -= groundAcc;

    //                if (velocity.X < -groundMaxAcc)
    //                    velocity.X = -groundMaxAcc;
    //            }
    //        }

    //        if (moveRight)
    //        {
    //            if (velocity.X < 0)
    //                velocity.X += groundTurnSpeed;
    //            else if (velocity.X < groundMaxAcc)
    //            {
    //                velocity.X += groundAcc;

    //                if (velocity.X > groundMaxAcc)
    //                    velocity.X = groundMaxAcc;
    //            }
    //        }

    //        if (moveDown)
    //        {
    //            if (velocity.Y < 0)
    //                velocity.Y += groundTurnSpeed;
    //            else if (velocity.Y < groundMaxAcc)
    //            {
    //                velocity.Y += groundAcc;

    //                if (velocity.Y > groundMaxAcc)
    //                    velocity.Y = groundMaxAcc;
    //            }
    //        }

    //        if (moveUp)
    //        {
    //            if (velocity.Y > 0)
    //                velocity.Y -= groundTurnSpeed;
    //            else if (velocity.Y > -groundMaxAcc)
    //            {
    //                velocity.Y -= groundAcc;

    //                if (velocity.Y < -groundMaxAcc)
    //                    velocity.Y = -groundMaxAcc;
    //            }
    //        }

    //        //if(moveLeft || moveRight || moveUp || moveDown)
    //        engineEmitter.emitParticles(50, 100, 0.1f);

    //        if (!moveLeft && !moveRight)
    //        {
    //            velocity.X -= Math.Min((float)Math.Abs(velocity.X), groundFriction) * Math.Sign(velocity.X);
    //        }

    //        if (!moveUp && !moveDown)
    //        {
    //            velocity.Y -= Math.Min((float)Math.Abs(velocity.Y), groundFriction) * Math.Sign(velocity.Y);
    //        }
    //    }

    //    private void setupEngineEmitter()
    //    {
    //        var file = "content/textures/flare/star.png";
    //        var material = resources.
    //            createMaterialFromTexture(file);

    //        //material.transparency = Color.Black;
    //        material.SetBlendState(BlendState.Additive);

    //        engineEmitter = gameObject.find("playership").
    //           createScript<ParticleEmitter>();

    //        engineEmitter.MinEnergy = 0.25f;
    //        engineEmitter.MaxEnergy = .25f;
    //        engineEmitter.Size = new Vector2(8, 8);
    //        engineEmitter.RandomScale = 0.1f;
    //        engineEmitter.EndSizeScale = 1f;
    //        engineEmitter.FadeTime = 0f;
    //        engineEmitter.RandomFade = 0f;
    //        //engineEmitter.UseParentWorldSpace = true;
    //        engineEmitter.DirectionStart = -0.25f;
    //        engineEmitter.DirectionEnd = 0.25f;
    //        //damageParticleEmitter.MinRotationSpeed = 0.1f;
    //        //damageParticleEmitter.MaxRotationSpeed = 0.2f;
    //        engineEmitter.StartColor = new Color(0.1f, 0.1f, 0.1f, 0);
    //        engineEmitter.EndColor = new Color(0.4f, 0.1f, 0.05f, 0);
    //        engineEmitter.RandomVelocity = 0.5f;
    //        engineEmitter.Velocity = 1;

    //        engineEmitter.material = material;
    //    }

    //    private void render()
    //    {
    //        //Root.instance.graphics.DrawLine(1, Root.instance.time.Find("basewhite"), DerivedPosition, DerivedPosition + DerivedForward * 20, Color.White);
    //        Root.instance.graphics.DrawText(1, Root.instance.resources.findFont("content/fonts/arial.fnt"), 1f, _transform.DerivedPosition, transform.DerivedOrientation.ToString(), Color.White);
    //    }

    //    private void update()
    //    {
    //        var gun = this.gameObject.getScriptWithChildren<SimpleGun>();
    //        gun.damage = damageModifier;

    //        if (Root.instance.input.IsLeftMouseDown)
    //            this.gameObject.sendMessageDown("primaryfire");
    //        if (Root.instance.input.IsRightMouseDown)
    //            this.gameObject.sendMessageDown("altfire");
    //    }

    //    private void takedamage(float dmg)
    //    {
    //        health -= (dmg / defenseModifier);

    //        if (health <= 0)
    //        {
    //            gameObject.destroy();
    //            //add explosion effect back in
    //            var sound = resources.createSoundFromFile("content/sounds/explosions/5.wav");
    //            sound.Play();

    //            var go = rootObject.createChild();
    //            go.transform.Position = transform.DerivedPosition;

    //            var explosionParticleEmitter = go.
    //                createScript<ParticleEmitter>();

    //            explosionParticleEmitter.MinEnergy = 0.25f;
    //            explosionParticleEmitter.MaxEnergy = 0.5f;
    //            explosionParticleEmitter.Size = new Vector2(16, 16);
    //            explosionParticleEmitter.EndSizeScale = 0.5f;
    //            explosionParticleEmitter.MinRotationSpeed = 0.1f;
    //            explosionParticleEmitter.MaxRotationSpeed = 0.2f;
    //            explosionParticleEmitter.StartColor = Color.Red;
    //            explosionParticleEmitter.EndColor = Color.Yellow;
    //            explosionParticleEmitter.RandomVelocity = 2f;
    //            explosionParticleEmitter.Velocity = 3;
    //            explosionParticleEmitter.emitParticles(50, 100, 1f);

    //            var file = "content/textures/streak.png";
    //            var material = resources.
    //                createMaterialFromTexture(file);

    //            material.SetBlendState(BlendState.Additive);

    //            explosionParticleEmitter.material = material;

    //            var destroy = go.createScript<DestroyTimer>();
    //            destroy.duration = 3f;

    //        }
    //        else
    //        {
    //            damageParticleEmitter.emitParticles(20, 50, 0.1f);
    //            var sound = resources.createSoundFromFile("content/sounds/misc/laserd.wav");
    //            //var sound = resources.createSoundFromFile("content/sounds/explosions/1.wav");
    //            sound.Play();

    //        }
    //    }

    //    private void ongui()
    //    {
    //        var font = resources.findFont("content/fonts/arial.fnt");
    //        var screenSize = screen.size;

    //        //draw the health bar
    //        var g = Root.instance.graphics;
    //        //var p = transform.DerivedPosition;
    //        var p = new Vector2(20, screenSize.Y - 40);
    //        var s = new Vector2(p.X, p.Y);
    //        var e = new Vector2(p.X + 200, p.Y);
    //        var l = (float)health / (float)maxHealth;

    //        var bg = AxisAlignedBox.FromRect(s.X, s.Y - 3, 200, 20);
    //        var fg = AxisAlignedBox.FromRect(s.X, s.Y - 3, 200 * l, 20);

    //        g.Draw(null, bg, Color.White);
    //        g.Draw(null, fg, Color.Red);

    //        //graphics.DrawText(font, 1f, new Vector2(100, 100), "test", Color.Red);
    //    }
    //}

}