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
    public class Shield : Sprite
    {
        const string PLAYER_ASSETNAME = "shield";
        int START_POSITION_X;
        int START_POSITION_Y;

        const int FRAME_COUNT = 3;
        TimeSpan FrameLength = TimeSpan.FromSeconds(0.25 / (double)FRAME_COUNT);
        TimeSpan FrameTimer = TimeSpan.Zero;

        public enum ShieldType
        {
            LEFT, MID, RIGHT
        }

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;
        ContentManager mContentManager;
        private int FrameNum = 0;

        ShieldType type;

        public Shield(GraphicsDevice gDevice, ShieldType type)
        {
            graphicsDevice = gDevice;
            FrameSize = 65;

            START_POSITION_X = gDevice.Viewport.Width; ;
            START_POSITION_Y = gDevice.Viewport.Height-FrameSize*2;
            this.type = type;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;
            switch (type)
            {
                case ShieldType.LEFT:
                    Position = new Vector2(START_POSITION_X/4-50, START_POSITION_Y);
                    break;
                case ShieldType.MID:
                    Position = new Vector2(START_POSITION_X/2, START_POSITION_Y);
                    break;
                case ShieldType.RIGHT:
                    Position = new Vector2(START_POSITION_X*3/4+50, START_POSITION_Y);
                    break;
            }
            base.LoadContent(theContentManager, PLAYER_ASSETNAME);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(mSpriteTexture, Position,
                new Rectangle(0 + (FrameSize * FrameNum), 0, FrameSize, mSpriteTexture.Height),
                Color.White, 0.0f, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public void Update(GameTime theGameTime)
        {
            base.Update(theGameTime, mSpeed, mDirection);
        }
    }
}
