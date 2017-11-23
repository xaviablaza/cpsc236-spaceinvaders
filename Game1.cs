using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Player mPlayerSprite;
        Background mBackgroundSprite;

        List<Enemy> mEnemyList = new List<Enemy>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            mPlayerSprite = new Player(graphics.GraphicsDevice);
            mBackgroundSprite = new Background(graphics.GraphicsDevice);

            mEnemyList.Add(new Enemy(graphics.GraphicsDevice, mPlayerSprite));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            mPlayerSprite.LoadContent(this.Content);
            mBackgroundSprite.LoadContent(this.Content);

            foreach (Enemy enemy in mEnemyList)
            {
                enemy.LoadContent(this.Content);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
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

            // TODO: Add your update logic here
            mPlayerSprite.Update(gameTime);
            mBackgroundSprite.Update(gameTime);

            // Basic management of enemies and collision of bullets
            List<Enemy> deadEnemy = new List<Enemy>();

            foreach (Enemy enemy in mEnemyList)
            {
                enemy.Update(gameTime);

                foreach (Bullet bullet in mPlayerSprite.mBullets)
                {
                    if (enemy.Size.Contains(bullet.Position.X + bullet.FrameSize / 2, bullet.Position.Y))
                    {
                        deadEnemy.Add(enemy);
                    }
                }
            }

            foreach (Enemy enemy in deadEnemy)
            {
                mEnemyList.Remove(enemy);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            mBackgroundSprite.Draw(this.spriteBatch); // has a custom spritebatch begin method

            spriteBatch.Begin();

            foreach (Enemy enemy in mEnemyList)
            {
                enemy.Draw(this.spriteBatch);
            }

            mPlayerSprite.Draw(this.spriteBatch);
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
