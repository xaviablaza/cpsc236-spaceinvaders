using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Background : Sprite
    {
        const string BACKGROUND_ASSETNAME = "water";
        private int CURRENT_Y = 32;

        TimeSpan IslandTimer = TimeSpan.Zero;
        List<Island> mIslands = new List<Island>();
        ContentManager mContentManager;
        private int IslandTimeRand = new Random().Next(2, 6);

        public Background(GraphicsDevice gDevice)
        {
            graphicsDevice = gDevice;
        }

        //Draw the sprite to the screen
        public override void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.Opaque, SamplerState.LinearWrap,
                DepthStencilState.Default, RasterizerState.CullNone);
            theSpriteBatch.Draw(mSpriteTexture, Vector2.Zero, new Rectangle(0, CURRENT_Y, graphicsDevice.Viewport.Width, 
                graphicsDevice.Viewport.Height), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
            theSpriteBatch.End();

            theSpriteBatch.Begin();
            foreach (Island island in mIslands)
            {
                island.Draw(theSpriteBatch);
            }
            theSpriteBatch.End();
        }

        public void LoadContent(ContentManager theContentManager)
        {
            mContentManager = theContentManager;
            Position = new Vector2(0, 0);
            base.LoadContent(theContentManager, BACKGROUND_ASSETNAME);
        }

        public void Update(GameTime theGameTime)
        {
            CURRENT_Y--;

            if (CURRENT_Y == 0)
                CURRENT_Y = 32;

            UpdateIslands(theGameTime);
        }

        private void UpdateIslands(GameTime theGameTime)
        {
            IslandTimer += theGameTime.ElapsedGameTime;

            if (IslandTimer.Seconds > IslandTimeRand)
            {
                Island island = new Island();
                island.LoadContent(mContentManager);
                island.CreateIsland(Position + new Vector2(new Random().Next(-25, graphicsDevice.Viewport.Width + 25), -100),
                    new Vector2(58, 60), new Vector2(0, 1));
                mIslands.Add(island);

                IslandTimer = TimeSpan.Zero;
                IslandTimeRand = new Random().Next(2, 6);
            }

            foreach (Island island in mIslands)
            {
                island.Update(theGameTime);
            }
        }
    }
}
