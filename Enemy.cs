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
using static Game1.EnemyGroup;

namespace Game1
{
    class Enemy : Sprite
    {
        public string ENEMY_ASSETNAME = "invader1";
        int START_POSITION_X = 125;
        int START_POSITION_Y = 0;
        const int ENEMY_SPEED = 80;
        const int MOVE_DOWN = 1;
        const int MOVE_RIGHT = 1;
        const int MOVE_LEFT = -1;

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
            mSpeed = new Vector2(0, ENEMY_SPEED);

            playerRef = playerReference;

            graphicsDevice = gDevice;
            FrameSize = 32;
        }

        public Enemy(GraphicsDevice gDevice, Player playerReference, int j, int k, String assetName)
        {
            mSpeed = new Vector2(0, ENEMY_SPEED);

            playerRef = playerReference;

            graphicsDevice = gDevice;
            FrameSize = 32;

            // Set the start position according to the height of the viewport
            START_POSITION_X = (k * FrameSize) + 16*k;
            START_POSITION_Y = (j * FrameSize) + 16*j;
            ENEMY_ASSETNAME = assetName;
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

        public void Update(GameTime theGameTime, MoveDirection moveDirection)
        {
            Size.X = (int)Position.X;
            Size.Y = (int)Position.Y;

            switch (moveDirection)
            {
                case MoveDirection.RIGHT:
                    mSpeed.X = ENEMY_SPEED;
                    mDirection.X = MOVE_RIGHT;
                    mSpeed.Y = 0;
                    mDirection.Y = 0;
                    base.Update(theGameTime, mSpeed, mDirection);
                    break;
                case MoveDirection.LEFT:
                    mSpeed.X = ENEMY_SPEED;
                    mDirection.X = MOVE_LEFT;
                    mSpeed.Y = 0;
                    mDirection.Y = 0;
                    base.Update(theGameTime, mSpeed, mDirection);
                    break;
                case MoveDirection.DOWN:
                    mSpeed.Y = ENEMY_SPEED;
                    mDirection.Y = MOVE_DOWN;
                    mSpeed.X = 0;
                    mDirection.X = 0;
                    base.Update(theGameTime, mSpeed, mDirection);
                    break;
            }
        }
    }
}
