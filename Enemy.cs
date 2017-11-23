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
    class Enemy : Sprite
    {
        const string ENEMY_ASSETNAME = "enemy1_strip3";
        int START_POSITION_X = 125;
        int START_POSITION_Y = 0;
        const int ENEMY_SPEED = 80;
        const int MOVE_DOWN = 1;
        const int MOVE_RIGHT = 1;

        const int FRAME_COUNT = 3;
        TimeSpan FrameLength = TimeSpan.FromSeconds(0.25 / (double)FRAME_COUNT);
        TimeSpan FrameTimer = TimeSpan.Zero;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        ContentManager mContentManager;
        Player playerRef;

        private int FrameNum = 0;

        public Enemy(GraphicsDevice gDevice, Player playerReference)
        {
            mDirection = new Vector2(MOVE_RIGHT, 0);
            mSpeed = new Vector2(0, ENEMY_SPEED);

            playerRef = playerReference;

            graphicsDevice = gDevice;
            FrameSize = 32;
        }

        public Enemy(GraphicsDevice gDevice, Player playerReference, int j, int k)
        {
            mDirection = new Vector2(MOVE_RIGHT, 0);
            mSpeed = new Vector2(0, ENEMY_SPEED);

            playerRef = playerReference;

            graphicsDevice = gDevice;
            FrameSize = 32;

            // Set the start position according to the height of the viewport
            START_POSITION_X = (k * FrameSize) + 16*k;
            START_POSITION_Y = (j * FrameSize) + 16*j;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;

            Position = new Vector2(START_POSITION_X, START_POSITION_Y);

            base.LoadContent(theContentManager, ENEMY_ASSETNAME);

            Size = new Rectangle(new Point(0, 0), new Point(FrameSize));
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position,
                new Rectangle(0 + (FrameSize * FrameNum), 0, FrameSize, mSpriteTexture.Height),
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public void Update(GameTime theGameTime)
        {
            Size.X = (int)Position.X;
            Size.Y = (int)Position.Y;

            base.Update(theGameTime, mSpeed, mDirection);

            FrameTimer += theGameTime.ElapsedGameTime;
            if (FrameTimer >= FrameLength)
            {
                FrameTimer = TimeSpan.Zero;
                FrameNum = (FrameNum + 1) % FRAME_COUNT;
            }

            if (FrameNum >= FRAME_COUNT)
                FrameNum = 0;
        }
    }
}
