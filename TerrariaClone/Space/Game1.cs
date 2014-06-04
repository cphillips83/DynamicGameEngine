#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using zSprite;
using GameName1.Space;
using Microsoft.Xna.Framework.Media;
using GameName1.Space.World;
#endregion

namespace GameName1.Space
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : GameEngine
    {
        public const int LAYER_BACKGROUND = 0;
        public const int LAYER_PARALLAX0 = 1;
        public const int LAYER_PARALLAX1 = 2;
        public const int LAYER_PARALLAX2 = 3;
        public const int LAYER_PARALLAX3 = 4;
        public const int LAYER_FOG0 = 5;
        public const int LAYER_MAIN0 = 6;
        public const int LAYER_MAIN1 = 7;
        public const int LAYER_MAIN2 = 8;
        public const int LAYER_FOG1 = 9;
        public const int LAYER_SCROLLINGTEXT = 10;
        public const int LAYER_HUD = 11;
        public const int LAYER_MENU = 12;

        public static Game instance { get; private set; }


        //public Player currentPlayer;
        public Universe universe;

        public Game()
            : base()
        {
            instance = this;
        }



        //protected void 

        protected override void LoadContent()
        {
            base.LoadContent();

            //{
            //    var len = 10;
            //    var p = new Vector2(20, 3);

            //    var i = len / p.Length();
            //    i = i < 1.0f ? i : 1.0f;

            //    p *= i;
            //}

            Root.instance.resources.addSearchPath("..\\..\\..\\");

            //var song = Root.instance.resources.createMusicFromFile("content/music/nebula.wav");
            //MediaPlayer.Play(song);
            var rootObject = Root.instance.RootObject;

            var camera = Root.instance.RootObject.createChild();
            var c = camera.createScript<Camera>();
            c.clear = Color.Black;

            var emitterGO = rootObject.createChild();
            emitterGO.createScript<TrackMouse>();
            var engineEmitter = emitterGO.createScript<ParticleEmitter>();
            var file = "content/textures/flare/star.png";
            var material = Root.instance.resources.
                createMaterialFromTexture(file);

            //material.transparency = Color.Black;
            material.SetBlendState(BlendState.Additive);

            engineEmitter.MinEnergy = 1f;
            engineEmitter.MaxEnergy = 1f;
            engineEmitter.Size = new Vector2(24, 24);
            engineEmitter.RandomScale = 0.1f;
            engineEmitter.EndSizeScale = 0.01f;
            engineEmitter.FadeTime = 0f;
            engineEmitter.RandomFade = 0f;
            //engineEmitter.UseParentWorldSpace = true;
            //engineEmitter.DirectionStart = MathHelper.Pi - 0.25f;
            //engineEmitter.DirectionEnd = MathHelper.Pi + 0.25f;
            //engineEmitter.UseParentWorldSpace = true;
            //damageParticleEmitter.MinRotationSpeed = 0.1f;
            //damageParticleEmitter.MaxRotationSpeed = 0.2f;
            engineEmitter.StartColor = new Color(0.05f, 0.05f, 0.2f, 1f);
            engineEmitter.EndColor = new Color(0.2f, 0.05f, 0.05f, 1f);
            engineEmitter.RandomVelocity = 2f;
            engineEmitter.Velocity = 3;

            engineEmitter.material = material;
            //universe = new Universe();

            //var random = new Random();

            //rootObject.createScript<TargetableObjects>();
            //var _settings = rootObject.createScript<Settings>();

            ////var tex2d = new Texture2D(this.GraphicsDevice, 1, 1);
            ////tex2d.SetData(new Color[] { Color.White });
            //var go = Root.instance.RootObject.createChild();
            ////go.createComponent<Rotate>();
            ////var test = go.createChild();
            ////test.transform().Orientation = MathHelper.PiOver2;
            ////{
            ////    var sprite = test.createComponent<TiledSprite>();
            ////    sprite.origin = new Vector2(0.5f, 0.5f);
            ////    sprite.color = Color.Red;
            ////    sprite.size = new Vector2(109 * 2, 142 * 2);

            ////    sprite.material = Root.instance.materials.Create();
            ////    sprite.material.texture = "textures/ship.png";
            ////    sprite.material.sampler = SamplerState.LinearWrap;
            ////    //sprite.material.blend = BlendState.AlphaBlend;
            ////    sprite.color = new Color(1f, 1f, 1f, 1f);

            ////    test.createComponent<LookAtMouse>();
            ////}

            ////this code shows the background
            ////var starfield = go.createChild();
            ////{
            ////    var sprite = starfield.createScript<ParallaxBackground>();
            ////    sprite.material = Root.instance.resources.createMaterial();
            ////    sprite.material.SetSamplerState(SamplerState.LinearWrap);
            ////    sprite.material.textureName = "content/textures/space-bg1.png";
            ////    sprite.speed = 0.01f;
            ////    sprite.transform.Scale = new Vector2(1.25f, 1.25f);
            ////}



            ////var parallaxLayer1 = go.createChild("planets");
            ////var parallaxScript1 = parallaxLayer1.createScript<ParallaxScroller>();
            ////parallaxScript1.speed *= 0.9f;
            ////spawnPlanet(123, Vector2.Zero, 1f, 3);

            ////var fog2 = go.createChild();
            //////fog2.transform.Depth = 0.1f;
            ////fog2.transform.Scale = Vector2.One / 4;
            ////{
            ////    var sprite = fog2.createScript<ParallaxBackground>();
            ////    sprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/fog3.png");
            ////    sprite.material.SetSamplerState(SamplerState.LinearWrap);
            ////    sprite.material.SetBlendState(BlendState.AlphaBlend);
            ////    //sprite.material.textureName = "content/textures/fog3.png";
            ////    sprite.color = new Color(new Vector4(0.05f, 0.1f, 0.15f, 0f) / 2);
            ////    //sprite.color = new Color(0.5f, 0.5f, 0.5f, 0f);
            ////    //                sprite.color = new Color(0.00f, 0.0f, 0.75f, 0.2f);
            ////    sprite.speed = 0.25f;
            ////}
            ////var planet1 = parallaxLayer1.createChild();
            ////{
            ////    planet1.transform.Position = new Vector2(500, 500);

            ////    {
            ////        var sprite = planet1.createScript<Sprite>();
            ////        sprite.material = Root.instance.resources.createMaterial();
            ////        sprite.material.SetSamplerState(SamplerState.LinearClamp);
            ////        sprite.material.textureName = "content/textures/planetred.png";
            ////        sprite.material.transparency = new Color(1, 1, 1);
            ////        sprite.size = new Vector2(300, 300);
            ////    }

            ////    {
            ////        var sprite = planet1.createScript<Sprite>();
            ////        sprite.material = Root.instance.resources.createMaterial();
            ////        sprite.material.SetBlendState(BlendState.Additive);
            ////        sprite.material.SetSamplerState(SamplerState.LinearClamp);
            ////        sprite.material.textureName = "content/textures/flare/tunel.png";
            ////        sprite.color = new Color(0.4f, 0.2f, 0.1f);
            ////        //sprite.color = new Color(
            ////        sprite.size = new Vector2(300, 300);
            ////    }
            ////    //sprite.speed = 0.03f;               
            ////    //sprite.transform.Scale = new Vector2(0.5f, 0.5f);

            ////}

            //rootObject.createScript<ParallaxLayer0>();
            //rootObject.createScript<ParallaxLayer1>();
            //rootObject.createScript<ParallaxLayer2>();
            //rootObject.createScript<ParallaxLayer3>();

            //var hud = rootObject.createChild("hud");
            //hud.transform.Position = new Vector2(1024 + 128 - 16, 128 + 16);
            //hud.createScript<Map>();
            ////var parallaxLayer2 = go.createChild();
            ////var parallaxScript2 = parallaxLayer1.createScript<ParallaxScroller>();
            ////parallaxScript2.speed *= 0.95f;
            ////var planet2 = parallaxLayer1.createChild();
            ////{
            ////    //planet2.transform.Position = new Vector2(-500, -500);

            ////    //{
            ////    //    var sprite = planet2.createScript<Sprite>();
            ////    //    sprite.material = Root.instance.resources.createMaterial();
            ////    //    sprite.material.SetSamplerState(SamplerState.LinearClamp);
            ////    //    sprite.material.textureName = "content/textures/stars/Star K eg9.bmp";
            ////    //    sprite.material.SetBlendState(BlendState.Additive);
            ////    //    //sprite.material.textureName = "content/textures/sun.png";
            ////    //    sprite.material.transparency = new Color(1, 1, 1);
            ////    //    sprite.size = new Vector2(500, 500);
            ////    //}

            ////    //{
            ////    //    var sprite = planet2.createScript<Sprite>();
            ////    //    sprite.material = Root.instance.resources.createMaterial();
            ////    //    sprite.material.SetBlendState(BlendState.Additive);
            ////    //    sprite.material.SetSamplerState(SamplerState.LinearClamp);
            ////    //    sprite.material.textureName = "content/textures/flares2/sun.png";
            ////    //    sprite.color = new Color(0.1f, 0.1f, 0.1f);
            ////    //    //sprite.color = new Color(
            ////    //    sprite.size = new Vector2(2000, 2000);

            ////    //}

            ////    {
            ////        var sprite = planet2.createScript<Sprite>();
            ////        sprite.material = Root.instance.resources.createMaterial();
            ////        sprite.material.SetBlendState(BlendState.Additive);
            ////        sprite.material.SetSamplerState(SamplerState.LinearClamp);
            ////        sprite.material.textureName = "content/textures/flares2/corona.png";
            ////        sprite.color = new Color(0.3f, 0.5f, 0.5f);
            ////        //sprite.color = new Color(
            ////        sprite.size = new Vector2(2000, 2000);

            ////    }
            ////    //sprite.speed = 0.03f;               
            ////    //sprite.transform.Scale = new Vector2(0.5f, 0.5f);

            ////}

            //{

            //    var rand = new Random();
            //    for (var i = 0; i < 60; i++)
            //    {
            //        var material = Root.instance.resources.createMaterialFromTexture(string.Format("content/textures/rock ({0}).png", rand.Next(1, 10)));
            //        var x = rand.Next(-_settings.SectorHalfSize, _settings.SectorHalfSize);
            //        var y = rand.Next(-_settings.SectorHalfSize, _settings.SectorHalfSize);

            //        var ao = go.createChild();
            //        ao.transform.Position = new Vector2(x, y);
            //        ao.transform.Scale = Vector2.One * 0.5f;

            //        var sprite = ao.createScript<Sprite>();
            //        sprite.material = material;
            //        sprite.renderQueue = Game.LAYER_MAIN0;
            //    }
            //}
            ////Asteroids_256x256_008

            ////Star G eg9



            ////-------------------------------------------------------------------
            //var fog1 = go.createChild();
            ////fog1.transform.Depth = 0.1f;
            //fog1.transform.Scale = Vector2.One / 2f;
            //{
            //    var sprite = fog1.createScript<ParallaxBackground>();
            //    sprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/fog6.png");
            //    sprite.material.SetSamplerState(SamplerState.LinearWrap);
            //    sprite.material.SetBlendState(BlendState.Additive);
            //    sprite.color = new Color(0.35f, 0.2075f, 0.4f, 0.3f);
            //    sprite.renderQueue = Game.LAYER_FOG0;
            //    //sprite.color = new Color(0.90f, 0.50f, 0.17f, 0.15f);
            //    sprite.speed = 0.25f;
            //}
            ////-------------------------------------------------------------------


            ////}



            //var fog3 = go.createChild();
            ////fog3.transform.Depth = 0.1f;
            //fog3.transform.Scale = Vector2.One / 8;
            //{
            //    var sprite = fog3.createScript<ParallaxBackground>();
            //    sprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/fog4.png");
            //    sprite.material.SetSamplerState(SamplerState.LinearWrap);
            //    sprite.material.SetBlendState(BlendState.Additive);
            //    sprite.color = new Color(0.1f, 0.3f, 0.6f, 0.53f);
            //    //sprite.color = new Color(0.15f, 0.015f, 0.0025f, 1f);
            //    sprite.renderQueue = Game.LAYER_MENU;
            //    //sprite.color = new Color(1.0f, 0.0f, 0.7f, 0.2f);
            //    sprite.speed = 0.75f;
            //}

            //var fog4 = go.createChild();
            //fog4.transform.Depth = 0.1f;
            //fog4.transform.Scale = Vector2.One / 10;
            //{
            //    var sprite = fog4.createScript<ParallaxBackground>();
            //    sprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/fog6.png");
            //    sprite.material.SetSamplerState(SamplerState.LinearWrap);
            //    sprite.material.SetBlendState(BlendState.AlphaBlend);
            //    sprite.renderQueue = Game.LAYER_FOG1;
            //    sprite.color = new Color(0.075f, 0.05f, 0.125f, 0f);
            //    //sprite.color = new Color(0.10f, 0.5f, 0.7f, 0.2f);
            //    sprite.speed = 3f;
            //}


            //var player = Root.instance.RootObject.createChild("player");
            //{
            //    //var material = Root.instance.resources.createMaterialFromTexture("content/textures/streak.png");
            //    //var particles = player.createScript<ParticleEmitter>();
            //    //material.SetBlendState(BlendState.Additive);
            //    //particles.EmitParticles = true;
            //    //particles.MinRotationSpeed = 0.1f;
            //    //particles.MaxRotationSpeed = 0.2f;
            //    //particles.StartColor = Color.Red;
            //    //particles.EndColor = Color.Yellow;
            //    //particles.RandomVelocity = 5;
            //    //particles.Velocity = 10;
            //    ////particles.DirectionStart = 0f;
            //    ////particles.DirectionEnd = MathHelper.Pi;
            //    //particles.MaxEmission = 1250;
            //    //particles.MinEmission = 750;
            //    //particles.material = material;
            //    ////var move = player.createComponent<Move>();
            //}

            ////var targetReticle = player.createChild("reticle");
            ////{
            ////    var sprite = targetReticle.createScript<Sprite>();
            ////    sprite.material = Root.instance.resources.createMaterial();
            ////    sprite.material.SetBlendState(BlendState.Additive);
            ////    sprite.material.textureName = "content/textures/selection.png";
            ////    sprite.size = new Vector2(20, 20);

            ////    var rotate = targetReticle.createScript<Rotate>();
            ////    rotate.speed = 2f;

            ////    var expander = targetReticle.createScript<Expander>();
            ////    expander.maxSize = 1.5f;
            ////    expander.speed = 1.25f;

            ////    var trackmouse = targetReticle.createScript<TrackMouse>();
            ////    trackmouse.maxDistance = 200f;
            ////}
            //{
            //    //var ship = player.createChild("playership");
            //    {
            //        var sprite = player.createScript<Sprite>();
            //        sprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/ship.png");
            //        sprite.size = new Vector2(50, 50);
            //        sprite.renderQueue = Game.LAYER_MAIN1;
            //        //var reticleTracker = player.createScript<TrackMouse>();
            //        ////reticleTracker.speed = 2000f;
            //        ////reticleTracker.gameObjectId = player.find("reticle").id;

            //        var controller = player.createScript<PlayerController>();
            //        //controller.ship = ship.transform2();

            //    }

            //    {
            //        var turret = player.createChild();
            //        {
            //            turret.transform.Position = new Vector2(0, -17.5f) / 2f;
            //            //turret.transform.Depth = -0.1f;
            //            var laser = turret.createScript<SimpleGun>();
            //            //laser.color = new Color(Color.Green, 0.5f);
            //            laser.velocity = 10f;
            //            laser.damage = 10;
            //            laser.color = new Color(1f, 0.5f, 1f);
            //            laser.targetFilters = TargetFilters.Aliens;
            //            //laser.length = 400f;
            //            //laser.targetFilters = TargetFilters.Aliens;
            //            //laser.color = Color.Red;

            //        }
            //    }

            //    {
            //        var hardpoint = player.createChild();
            //        {
            //            hardpoint.transform.Position = new Vector2(0, 17.5f) / 2f;
            //            //hardpoint.transform.Depth = -0.1f;

            //            var laser = hardpoint.createScript<SimpleGun>();
            //            // laser.length = 400f;
            //            laser.velocity = 10f;
            //            laser.damage = 10;
            //            laser.color = new Color(1f, 0.5f, 1f);
            //            laser.targetFilters = TargetFilters.Aliens;
            //            //laser.color = new Color(Color.Green, 0.5f);
            //            //laser.targetFilters = TargetFilters.Aliens;
            //            //Scroll
            //        }
            //    }

            //    {
            //        var turret = player.createChild();
            //        {
            //            turret.transform.Position = new Vector2(-30, -37.5f) / 2f;
            //            //turret.transform.Depth = -0.1f;
            //            var laser = turret.createScript<SimpleGun>();
            //            //laser.color = new Color(Color.Green, 0.5f);
            //            laser.velocity = 10f;
            //            laser.color = new Color(0.5f, 1f, 0.5f);
            //            laser.damage = 5;
            //            laser.targetFilters = TargetFilters.Aliens;
            //            //laser.lvength = 400f;
            //            //laser.targetFilters = TargetFilters.Aliens;
            //            //laser.color = Color.Red;

            //        }
            //    }

            //    {
            //        var hardpoint = player.createChild();
            //        {
            //            hardpoint.transform.Position = new Vector2(-30, 37.5f) / 2f;
            //            //hardpoint.transform.Depth = -0.1f;

            //            var laser = hardpoint.createScript<SimpleGun>();
            //            // laser.length = 400f;
            //            laser.velocity = 10f;
            //            laser.damage = 5;
            //            laser.color = new Color(0.5f, 1f, 0.5f);
            //            laser.targetFilters = TargetFilters.Aliens;
            //            //laser.color = new Color(Color.Green, 0.5f);
            //            //laser.targetFilters = TargetFilters.Aliens;
            //            //Scroll
            //        }
            //    }
            //}


            //var r = new Random();
            //for (var i = 0; i < 12; i += 2)
            //{

            //    var enemy = Root.instance.RootObject.createChild("enemy");
            //    {
            //        enemy.transform.Position = new Vector2(r.Next(-500, 500), r.Next(-500, 500));
            //        //enemy.createScript<Enemy>();
            //        //var ship = enemy.createChild();
            //        {
            //            var sprite = enemy.createScript<Sprite>();
            //            sprite.material = Root.instance.resources.createMaterialFromTexture("content/textures/ship.png");
            //            sprite.size = new Vector2(40, 40);
            //            sprite.rotation = MathHelper.PiOver2;
            //            sprite.renderQueue = Game.LAYER_MAIN1;

            //            var ai = enemy.createScript<AIWanderAggresive>();
            //            var ship = enemy.createScript<Ship>();
            //            ship.steering.maxForce = 4;
            //            ship.steering.maxVelocity = 2;
            //            ai.attackFilters = (i % 2) == 0 ? TargetFilters.Player : TargetFilters.Aliens;
            //            ai.friendlyFilters = (i % 2) == 1 ? TargetFilters.Player : TargetFilters.Aliens;
            //            ai.aroundRadius = 400;

            //            var scale = random.Next(1f, 3f);
            //            enemy.transform.DerivedScale = Vector2.One * scale;
            //            ship.steering.maxVelocity /= scale;
            //            ai.maxTurnSpeed -= ((scale - 1f) / 2f) * 0.025f;
            //            scale *= 2;
            //            ship.mass = 20f * scale;
            //            //var controller = enemy.createScript<LinearVelocity>();
            //            //var ai = enemy.createScript<AIControllerOld>();
            //            //ai.fireTimerDelay = (float)r.NextDouble() * 2 + 0.5f;

            //            //var track = enemy.createScript<Track>();
            //            //track.gameObjectId = player.id;

            //            //enemy.createScript<Enemy>();
            //        }


            //        var turret = enemy.createChild();
            //        {
            //            turret.transform.Position = new Vector2(7.5f, 0);
            //            //var reticleTracker = turret1.createScript<Track>();
            //            //reticleTracker.speed = 0.5f;
            //            //reticleTracker.gameObjectId = player.find("reticle").id;

            //            var laser = turret.createScript<SimpleGun>();
            //            laser.targetFilters = (i % 2) == 0 ? TargetFilters.Player : TargetFilters.Aliens; ;
            //            laser.color = (i % 2) == 1 ? new Color(0.5f, 1f, 0.5f) : new Color(1f, 0.5f, 0.5f);
            //            //laser.color = new Color((i % 2) == 1 ? Color.Green : Color.Red, 0.5f);
            //            laser.velocity = 4f;
            //            laser.energy = 5f;
            //            laser.fireRate = 0.5f;
            //            //laser.length = 400f;
            //            //laser.length = 200f;
            //            //laser.color = Color.Red;

            //        }
            //    }

            //}
            //{
            //    var follow = camera.createScript<Follow>();
            //    follow.gameObjectId = player.id;
            //}

            //var scrolling = rootObject.createScript<ScrollingText>();
            //scrolling.StartColor = Color.Red;
            //scrolling.EndColor = Color.Orange;
            //scrolling.EmitArea = Vector2.One * 10f;
            //scrolling.DirectionStart -= 0.25f;
            //scrolling.DirectionEnd += 0.25f;
            //scrolling.renderQueue = Game.LAYER_SCROLLINGTEXT;
            Root.instance.RootObject.createScript<DebugStats>();

        }

        private void setupCamera()
        {

        }


        private bool isLoaded = false;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);

            if (!isLoaded)
            {
                isLoaded = true;
                Root.instance.RootObject.broadcast("loadsector", 0);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //GraphicsDevice.Clear(Color.CornflowerBlue);
            //Root.instance.graphics.Draw(0, material, new Rectangle(20, 20, 100, 100), Color.Red);
            base.Draw(gameTime);
        }
    }
}
