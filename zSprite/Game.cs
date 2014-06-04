#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

#endregion Using Statements

public class User32
{
    [DllImport("user32.dll")]
    public static extern void SetWindowPos(uint Hwnd, uint Level, int X, int Y, int W, int H, uint Flags);
} 

namespace zSprite
{
    public partial class GameEngine : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;

        //SpriteBatch spriteBatch;
        private Root root;

        private bool skipDraw = true;

        public GameEngine()
            : base()
        {
            IsFixedTimeStep = false;
            graphics = new GraphicsDeviceManager(this);
            graphics.SynchronizeWithVerticalRetrace = false;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 768;
            graphics.GraphicsDevice.PresentationParameters.RenderTargetUsage = RenderTargetUsage.DiscardContents;
            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
            //content = this.Content;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (skipDraw)
                skipDraw = false;
            else
            {
                root.draw();
                base.Draw(gameTime);
            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            IsFixedTimeStep = false;
            root = new Root();
            base.Initialize();

            //spriteBatch.Begin(
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            root.start(this.GraphicsDevice, this.Content);
            root.screen.SetResolution(this.graphics.PreferredBackBufferWidth, this.graphics.PreferredBackBufferHeight, this.graphics.IsFullScreen);
            //spriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
            root.cleanup();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            root.update(gameTime.TotalGameTime.TotalSeconds);
            base.Update(gameTime);
            //User32.SetWindowPos((uint)this.Window.Handle, 0, 0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, 0);
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.DiscardContents;
        }
    }
}