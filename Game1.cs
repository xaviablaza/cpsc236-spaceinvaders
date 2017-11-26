using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Player mPlayerSprite;
        Background mBackgroundSprite;
        public Shield[] mShieldSprites;

        EnemyGroup mEnemyGroup;

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
            mEnemyGroup = new EnemyGroup(graphics.GraphicsDevice, this);
            mShieldSprites = new Shield[3];
            mShieldSprites[0] = new Shield(graphics.GraphicsDevice, Shield.ShieldType.LEFT);
            mShieldSprites[1] = new Shield(graphics.GraphicsDevice, Shield.ShieldType.MID);
            mShieldSprites[2] = new Shield(graphics.GraphicsDevice, Shield.ShieldType.RIGHT);

            // Make the window full screen
            //graphics.ToggleFullScreen();

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
            mEnemyGroup.LoadContent(this.Content);
            foreach (Shield shield in mShieldSprites)
            {
                shield.LoadContent(this.Content);
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
            foreach (Shield shield in mShieldSprites)
            {
                shield.Update(gameTime);
            }
            mPlayerSprite.Update(gameTime);
            mBackgroundSprite.Update(gameTime);
            mEnemyGroup.Update(gameTime);
            

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
            foreach (Shield shield in mShieldSprites)
            {
                shield.Draw(this.spriteBatch);
            }
            mEnemyGroup.Draw(this.spriteBatch);
            mPlayerSprite.Draw(this.spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
