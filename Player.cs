using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
// using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Player : Sprite
    {
        const string PLAYER_ASSETNAME = "myplane_strip3";
        const int START_POSITION_X = 125;
        const int START_POSITION_Y = 245;
        const int PLAYER_SPEED = 160;
        const int MOVE_UP = -1;
        const int MOVE_DOWN = 1;
        const int MOVE_LEFT = -1;
        const int MOVE_RIGHT = 1;

        const int FRAME_COUNT = 3;
        TimeSpan FrameLength = TimeSpan.FromSeconds(0.25 / (double)FRAME_COUNT);
        TimeSpan FrameTimer = TimeSpan.Zero;

        enum State
        {
            Walking
        }
        State mCurrentState = State.Walking;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        KeyboardState mPreviousKeyboardState;
        GamePadState mPreviousGamepadState;

        public List<Bullet> mBullets = new List<Bullet>();
        public List<Bullet> mDeadBullets = new List<Bullet>();
        ContentManager mContentManager;
        private sbyte bulletFlip = 1;

        private int FrameNum = 0;

        public Player(GraphicsDevice gDevice)
        {
            graphicsDevice = gDevice;
            FrameSize = 65;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            foreach (Bullet aBullet in mBullets)
            {
                aBullet.LoadContent(theContentManager);
            }

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);
            base.LoadContent(theContentManager, PLAYER_ASSETNAME);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Bullet aBullet in mBullets)
            {
                aBullet.Draw(theSpriteBatch);
            }

            theSpriteBatch.Draw(mSpriteTexture, Position,
                new Rectangle(0 + (FrameSize * FrameNum), 0, FrameSize, mSpriteTexture.Height),
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public void Update(GameTime theGameTime)
        {
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            GamePadState aCurrentGamepadState = GamePad.GetState(PlayerIndex.One);

            UpdateMovement(aCurrentKeyboardState);
            UpdateBullet(theGameTime, aCurrentKeyboardState, aCurrentGamepadState);

            mPreviousKeyboardState = aCurrentKeyboardState;
            mPreviousGamepadState = aCurrentGamepadState;

            base.Update(theGameTime, mSpeed, mDirection);

            /* Stop the player from moving off the screen correction */
            if (Position.X + FrameSize > graphicsDevice.Viewport.Width)
            {
                Position.X = graphicsDevice.Viewport.Width - FrameSize;
            }

            if (Position.X < 0)
            {
                Position.X = 0;
            }

            if (Position.Y + Size.Height > graphicsDevice.Viewport.Height)
            {
                Position.Y = graphicsDevice.Viewport.Height - Size.Height;
            }

            if (Position.Y < 0)
            {
                Position.Y = 0;
            }
            /* End player off screen correction */

            FrameTimer += theGameTime.ElapsedGameTime;
            if (FrameTimer >= FrameLength)
            {
                FrameTimer = TimeSpan.Zero;
                FrameNum = (FrameNum + 1) % FRAME_COUNT;
            }

            if (FrameNum >= FRAME_COUNT)
                FrameNum = 0;
        }

        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            if (mCurrentState == State.Walking)
            {
                mSpeed = Vector2.Zero;
                mDirection = Vector2.Zero;

                if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true)
                {
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_LEFT;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
                {
                    mSpeed.X = PLAYER_SPEED;
                    mDirection.X = MOVE_RIGHT;
                }

                if (aCurrentKeyboardState.IsKeyDown(Keys.Up) == true)
                {
                    mSpeed.Y = PLAYER_SPEED;
                    mDirection.Y = MOVE_UP;
                }
                else if (aCurrentKeyboardState.IsKeyDown(Keys.Down) == true)
                {
                    mSpeed.Y = PLAYER_SPEED;
                    mDirection.Y = MOVE_DOWN;
                }
            }
        }

        private void UpdateBullet(GameTime theGameTime, KeyboardState aCurrentKeyboardState, GamePadState aCurrentGamepadState)
        {
            foreach (Bullet aBullet in mBullets)
            {
                aBullet.Update(theGameTime);
            }

            if ((aCurrentKeyboardState.IsKeyDown(Keys.Space) == true && mPreviousKeyboardState.IsKeyDown(Keys.Space) == false)
                || (aCurrentGamepadState.Buttons.A == ButtonState.Pressed && mPreviousGamepadState.Buttons.A == ButtonState.Released))
            {
                ShootBullet();
            }
        }

        private void ShootBullet()
        {
            if (mCurrentState == State.Walking)
            {
                bool aCreateNew = true;
                for (int i = 0; i < mBullets.Count; i++)
                {
                    if (mBullets[i].Position.Y < 0)
                        mBullets.RemoveAt(i);
                }

                if (aCreateNew == true)
                {
                    Bullet aBullet = new Bullet();
                    aBullet.LoadContent(mContentManager);
                    aBullet.Fire(Position + new Vector2((FrameSize / 2 - aBullet.Size.Width / 2) + (bulletFlip * 12), 12),
                        new Vector2(200, 200), new Vector2(0, -1));
                    mBullets.Add(aBullet);

                    bulletFlip *= -1;
                }
            }
        }
    }
}
