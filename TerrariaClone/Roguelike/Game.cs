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
using GameName1.Roguelike.Scripts;
#endregion

namespace GameName1.Roguelike
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game : GameEngine
    {
        public const float WORLD_SCALE = 1f;

        public Game()
            : base()
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            Root.instance.resources.addSearchPath("..\\..\\..\\");

            //TODO: Player weapon attack, left and right
            //          -Dual weilding causes 2 weapons to swing at once doing combined damage
            //          Bow + spell
            //          dual spells
            //          shield off hand with one hand weapon
            //          two hand weapon
            //TODO: *Enemy attack, pursue ai
            //TODO: Generated maps with stairs to other generated maps
            //TODO: Health, mana, exp
            //TODO: Map display
            //TODO: Inventory
            //TODO: Items
            //TODO: AI movement using opensteer
            //TODO: Enemy category types using different opensteer behaviors
            //TODO: Different weapon types, swords, bow, spells
            //*TODO: Floating combat text

            Root.instance.RootObject.prefabPlayer();
            Root.instance.RootObject.prefabCamera();
            Root.instance.RootObject.prefabEnemy();
            //Root.instance.RootObject.prefabEnemy();
            //Root.instance.RootObject.prefabEnemy();
            //Root.instance.RootObject.prefabEnemy();
            //Root.instance.RootObject.prefabEnemy();
            //Root.instance.RootObject.prefabEnemy();
            //Root.instance.RootObject.prefabEnemy();
            //Root.instance.RootObject.prefabEnemy();
            Root.instance.RootObject.createScript<Map>();
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
