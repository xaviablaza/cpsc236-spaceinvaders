using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class Island : Sprite
    {
        const string BACKGROUND_ISLAND = "island";
        Random IslandNum = new Random();

        Vector2 mDirection;
        Vector2 mSpeed;
        Vector2 mStartPosition;

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, BACKGROUND_ISLAND + IslandNum.Next(1, 3));
            Scale = 1f;
        }

        public void Update(GameTime theGameTime)
        {
            base.Update(theGameTime, mSpeed, mDirection);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch);
        }

        public void CreateIsland(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {
            Position = theStartPosition;
            mStartPosition = theStartPosition;
            mSpeed = theSpeed;
            mDirection = theDirection;
        }
    }
}
