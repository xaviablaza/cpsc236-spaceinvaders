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
    public class Bullet : Sprite
    {
        string BULLET_ASSETNAME = "bullet";

        Vector2 mDirection;
        Vector2 mSpeed;
        Vector2 mStartPosition;

        public Bullet()
        {
        }

        public Bullet(String assetName)
        {
            BULLET_ASSETNAME = assetName;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, BULLET_ASSETNAME);
            Scale = 0.5f;
            FrameSize = Size.Width;
        }

        public void Update(GameTime theGameTime)
        {
            base.Update(theGameTime, mSpeed, mDirection);
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            base.Draw(theSpriteBatch);
        }

        public void Fire(Vector2 theStartPosition, Vector2 theSpeed, Vector2 theDirection)
        {
            Position = theStartPosition;
            mStartPosition = theStartPosition;
            mSpeed = theSpeed;
            mDirection = theDirection;
        }
    }
}
