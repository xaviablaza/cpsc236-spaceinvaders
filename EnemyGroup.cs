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
    class EnemyGroup
    {

        Enemy[][] mEnemyArray = new Enemy[5][];
        GraphicsDevice graphicsDevice;
        private Game1 game;
        
        public EnemyGroup(GraphicsDevice gDevice, Game1 game)
        {
            graphicsDevice = gDevice;
            this.game = game;
            for (int j = 0; j < mEnemyArray.Length; ++j)
            {
                mEnemyArray[j] = new Enemy[11];
            }
            for (int j = 0; j < mEnemyArray.Length; ++j)
            {
                for (int k = 0; k < mEnemyArray[j].Length; ++k)
                {
                    mEnemyArray[j][k] = new Enemy(graphicsDevice, game.mPlayerSprite, j, k);
                }
            }
        }

        public void LoadContent(ContentManager content)
        {
            for (int j = 0; j < mEnemyArray.Length; ++j)
            {
                for (int k = 0; k < mEnemyArray[j].Length; ++k)
                {
                    mEnemyArray[j][k].LoadContent(content);
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            // Basic management of enemies and collision of bullets
            List<EnemyCoord> deadEnemies = new List<EnemyCoord>();

            for (int j = 0; j < mEnemyArray.Length; ++j)
            {
                for (int k = 0; k < mEnemyArray[j].Length; ++k)
                {
                    if (mEnemyArray[j][k] != null)
                    {
                        mEnemyArray[j][k].Update(gameTime);
                        foreach (Bullet bullet in game.mPlayerSprite.mBullets)
                        {
                            if (mEnemyArray[j][k].Size.Contains(bullet.Position.X + bullet.FrameSize / 2, bullet.Position.Y))
                            {
                                deadEnemies.Add(new EnemyCoord(j, k));
                                game.mPlayerSprite.mDeadBullets.Add(bullet);
                            }
                        }
                    }
                }
            }

            foreach (EnemyCoord coord in deadEnemies)
            {
                mEnemyArray[coord.X][coord.Y] = null;
            }

            foreach (Bullet bullet in game.mPlayerSprite.mDeadBullets)
            {
                game.mPlayerSprite.mBullets.Remove(bullet);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int j = 0; j < mEnemyArray.Length; ++j)
            {
                for (int k = 0; k < mEnemyArray[j].Length; ++k)
                {
                    if (mEnemyArray[j][k] != null)
                    {
                        mEnemyArray[j][k].Draw(spriteBatch);
                    }
                }
            }
        }
    }
}
