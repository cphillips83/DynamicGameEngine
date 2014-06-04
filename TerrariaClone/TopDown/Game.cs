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
using zSprite.Scripts;
using GameName1.TopDown.Components;
using GameName1.TopDown.Controllers;
using GameName1.TopDown.Components.World;
#endregion

namespace GameName1.TopDown
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : GameEngine
    {

        public Game()
            : base()
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Root.instance.resources.addSearchPath("..\\..\\..\\");



           // var camera = GameEngine.RootObject.createChild();
            var player = Root.instance.RootObject.prefabPlayer();
            //camera.createComponent<Gravity>();
            //camera.transform().Position = new Vector2(505, 500) * Settings.TileSizeV;

            //camera.createComponent<TerrainCollider>();
            //camera.createComponent<Camera>();
            //camera.createComponent<PlayerController>();
            //camera.createComponent<MovementComponent>();

            {
                var obj = Root.instance.RootObject.createScript<MapRenderer>();
            }


            //var tex2d = new Texture2D(this.GraphicsDevice, 1, 1);
            //tex2d.SetData(new Color[] { Color.White });
            //var go = GameEngine.RootObject.createChild();
            //go.createComponent<Rotate>();
            //var test = go.createChild();
            //test.transform().Orientation = MathHelper.PiOver2;
            //{
            //    var sprite = test.createComponent<TiledSprite>();
            //    sprite.origin = new Vector2(0.5f, 0.5f);
            //    sprite.color = Color.Red;
            //    sprite.size = new Vector2(109 * 2, 142 * 2);

            //    sprite.material = Root.instance.materials.Create();
            //    sprite.material.texture = "textures/ship.png";
            //    sprite.material.sampler = SamplerState.LinearWrap;
            //    //sprite.material.blend = BlendState.AlphaBlend;
            //    sprite.color = new Color(1f, 1f, 1f, 1f);

            //    test.createComponent<LookAtMouse>();
            //}

            //var starfield = go.createChild();
            //{
            //    var sprite = starfield.createComponent<ParallaxBackground>();
            //    sprite.material = Root.instance.materials.Create();
            //    sprite.material.texture = "textures/starfield.png";
            //    sprite.material.sampler = SamplerState.LinearWrap;
            //    sprite.speed = 0.1f;
            //}
            //var fog1 = go.createChild();
            //fog1.transform().Depth = 0.1f;
            //fog1.transform().Scale = Vector2.One / 2f;
            //{
            //    var sprite = fog1.createComponent<ParallaxBackground>();
            //    sprite.material = Root.instance.materials.Create();
            //    sprite.material.texture = "textures/fog6.png";
            //    sprite.material.sampler = SamplerState.LinearWrap;
            //    sprite.color = new Color(0.00f, 0.10f, 0.07f, 0f);
            //    sprite.speed = 1f;

            //}

            //var fog2 = go.createChild();
            //fog2.transform().Depth = 0.1f;
            //fog2.transform().Scale = Vector2.One / 5;
            //{
            //    var sprite = fog2.createComponent<ParallaxBackground>();
            //    sprite.material = Root.instance.materials.Create();
            //    sprite.material.texture = "textures/fog6.png";
            //    sprite.material.sampler = SamplerState.LinearWrap;
            //    //sprite.color = new Color(0.075f, 0.05f, 0.125f, 0f);
            //    sprite.color = new Color(0.00f, 0.0f, 0.25f, 0f);
            //    sprite.speed = 4f;
            //}

            //var fog3 = go.createChild();
            //fog3.transform().Depth = 0.1f;
            //fog3.transform().Scale = Vector2.One / 3;
            //{
            //    var sprite = fog3.createComponent<ParallaxBackground>();
            //    sprite.material = Root.instance.materials.Create();
            //    sprite.material.texture = "textures/fog6.png";
            //    sprite.material.sampler = SamplerState.LinearWrap;
            //    //sprite.color = new Color(0.075f, 0.05f, 0.125f, 0f);
            //    sprite.color = new Color(0.10f, 0.0f, 0.07f, 0f);
            //    sprite.speed = 2f;
            //}

            //var fog4 = go.createChild();
            //fog4.transform().Depth = 0.1f;
            //fog4.transform().Scale = Vector2.One / 10;
            //{
            //    var sprite = fog4.createComponent<ParallaxBackground>();
            //    sprite.material = Root.instance.materials.Create();
            //    sprite.material.texture = "textures/fog6.png";
            //    sprite.material.sampler = SamplerState.LinearWrap;
            //    //sprite.color = new Color(0.075f, 0.05f, 0.125f, 0f);
            //    sprite.color = new Color(0.10f, 0.05f, 0.07f, 0f);
            //    sprite.speed = 3f;
            //}


            //var player = GameEngine.RootObject.createChild();
            //{
            //    //var move = player.createComponent<Move>();
            //}

            //var targetReticle = player.createChild("reticle");
            //{
            //    var sprite = targetReticle.createComponent<Sprite>();
            //    sprite.material = Root.instance.materials.Create();
            //    sprite.material.texture = "textures/selection.png";
            //    sprite.size = new Vector2(20, 20);

            //    var rotate = targetReticle.createComponent<Rotate>();
            //    rotate.speed = 2f;

            //    var expander = targetReticle.createComponent<Expander>();
            //    expander.maxSize = 1.5f;
            //    expander.speed = 1.25f;

            //    var trackmouse = targetReticle.createComponent<TrackMouse>();
            //    trackmouse.maxDistance = 200f;
            //}
            //{
            //    var ship = player.createChild();
            //    {
            //        var sprite = ship.createComponent<Sprite>();
            //        sprite.material = Root.instance.materials.Create();
            //        sprite.material.texture = "textures/ship.png";
            //        sprite.size = new Vector2(50, 50);

            //        var reticleTracker = ship.createComponent<Track>();
            //        reticleTracker.speed = 2.5f;
            //        reticleTracker.gameObjectId = player.find("reticle").id;

            //        var controller = player.createComponent<PlayerController>();
            //        controller.ship = ship.transform();

            //    }

            //    var turret1 = ship.createChild();
            //    {
            //        turret1.transform().Position = new Vector2(-15, 2.5f);
            //        var reticleTracker = turret1.createComponent<Track>();
            //        reticleTracker.speed = 0.5f;
            //        reticleTracker.gameObjectId = player.find("reticle").id;

            //        var laser = turret1.createComponent<SimpleLaser>();
            //        laser.length = 200f;
            //        laser.color = Color.Red;

            //    }

            //    var turret2 = ship.createChild();
            //    {
            //        turret2.transform().Position = new Vector2(15, 2.5f);
            //        var reticleTracker = turret2.createComponent<Track>();
            //        reticleTracker.speed = 0.5f;
            //        reticleTracker.gameObjectId = player.find("reticle").id;

            //        var laser = turret2.createComponent<SimpleLaser>();
            //        laser.length = 200f;
            //        laser.color = Color.Red;
            //    }

            //    var hardpoint = ship.createChild();
            //    {
            //        hardpoint.transform().Position = new Vector2(0, -10f);
            //        //hardpoint.transform().Orientation = MathHelper.Pi;

            //        //{
            //        //    var sprite = hardpoint.createComponent<TiledSprite>();
            //        //    sprite.origin = new Vector2(0.5f, 1f);
            //        //    sprite.color = Color.Red;
            //        //    sprite.size = new Vector2(16, 200);

            //        //    sprite.material = Root.instance.materials.Create();
            //        //    sprite.material.texture = "textures/wavebeam.png";
            //        //    sprite.material.sampler = SamplerState.LinearWrap;
            //        //    sprite.material.blend = BlendState.AlphaBlend;
            //        //    sprite.color = new Color(0.25f, 0f, 0f, 0f);
            //        //}

            //        //{
            //        //    var sprite = hardpoint.createComponent<TiledSprite>();
            //        //    sprite.origin = new Vector2(0.5f, 0f);
            //        //    sprite.color = Color.Red;
            //        //    sprite.size = new Vector2(16, 200);
            //        //    sprite.offset = new Vector2(0f, 0.5f);
            //        //    sprite.material = Root.instance.materials.Create();
            //        //    sprite.material.texture = "textures/wavebeam.png";
            //        //    sprite.material.sampler = SamplerState.LinearWrap;
            //        //    sprite.material.blend = BlendState.AlphaBlend;
            //        //    sprite.color = new Color(0.25f, 0f, 0f, 0f);
            //        //}

            //        var laser = hardpoint.createComponent<SimpleLaser>();
            //        laser.isaltfire = true;
            //        laser.length = 200f;
            //        laser.color = Color.Green;

            //    }
            //}

            //var enemy = GameEngine.RootObject.createChild();
            //{
            //    //var ship = enemy.createChild();
            //    {
            //        var sprite = enemy.createComponent<Sprite>();
            //        sprite.material = Root.instance.materials.Create();
            //        sprite.material.texture = "textures/ship.png";
            //        sprite.size = new Vector2(40, 40);

            //        var controller = enemy.createComponent<LinearVelocity>();
            //        var ai = enemy.createComponent<AIController>();
            //        var track = enemy.createComponent<Track>();
            //        track.gameObjectId = player.id;
            //    }

            //    //var turret1 = ship.createChild();
            //    //{
            //    //    turret1.transform().Position = new Vector2(-15, 2.5f);
            //    //    var reticleTracker = turret1.createComponent<Track>();
            //    //    reticleTracker.speed = 0.5f;
            //    //    reticleTracker.gameObjectId = player.find("reticle").id;

            //    //    var laser = turret1.createComponent<SimpleLaser>();
            //    //    laser.length = 200f;
            //    //    laser.color = Color.Red;

            //    //}

            //    //var turret2 = ship.createChild();
            //    //{
            //    //    turret2.transform().Position = new Vector2(15, 2.5f);
            //    //    var reticleTracker = turret2.createComponent<Track>();
            //    //    reticleTracker.speed = 0.5f;
            //    //    reticleTracker.gameObjectId = player.find("reticle").id;

            //    //    var laser = turret2.createComponent<SimpleLaser>();
            //    //    laser.length = 200f;
            //    //    laser.color = Color.Red;
            //    //}
            //}


            //{
            //    var follow = camera.createComponent<Follow>();
            //    follow.gameObjectId = player.id;
            //}
        }
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
