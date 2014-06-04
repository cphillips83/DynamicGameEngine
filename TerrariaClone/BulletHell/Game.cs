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
using GameName1.BulletHell.Scripts;
#endregion

namespace GameName1.BulletHell
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : GameEngine
    {
        public static Game instance { get; private set; }

        public Game()
            : base()
        {
            instance = this;
        }

        public interface ITest : IEntity
        {
            string Name { get; }
        }

        public class Test : IEntity, ITest
        {
            public string Name { get { return "WOOT!"; } }
        } 

        public class TestSystem : zSprite.Systems.RequiresSystem
        {
            protected override void oninit()
            {
                require<Test>(x =>
                {
                    Console.WriteLine(x.Name);
                });
            }
        }

        protected override void LoadContent()
        {
            IsMouseVisible = false;
            GameWindowExtensions.SetPosition(this.Window, new Point(200, 100));

            base.LoadContent();

            Root.instance.addSystem(new TestSystem());
            Root.instance.addEntity(new Test());


            Root.instance.RootObject.createScript<Init>();
            GraphicsDevice.Clear(Color.Black);
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
