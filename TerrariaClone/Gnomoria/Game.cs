
#region Using Statements

using GameName1.Gnomoria.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using zSprite;
using zSprite.Scripts;

#endregion Using Statements

namespace GameName1.Gnomoria
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : GameEngine
    {
        public const int QUEUE_COMMAND_MENU = 2;
        public const float WORLD_SCALE = 1f;

        public Game()
            : base()
        {
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

        protected override void LoadContent()
        {
            base.LoadContent();
            Root.instance.resources.addSearchPath("..\\..\\..\\");

            var world = Root.instance.RootObject.prefabWorld();
            var tools = Root.instance.RootObject.prefabTools();
            var player = Root.instance.RootObject.prefabPlayer();
            //var map = Root.instance.RootObject.createScript<Map>();
            var camera = Root.instance.RootObject.prefabCamera();
            Root.instance.RootObject.createScript<DebugStats>();
            Root.instance.RootObject.createScript<MainMenu>();
            //Root.instance.RootObject.createScript<DebugLabel>();
            //Root.instance.RootObject.broadcast("loadmap", 256, 256, 128);
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
    }
}