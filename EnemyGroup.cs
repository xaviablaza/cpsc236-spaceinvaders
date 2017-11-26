using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Game1
{
    class EnemyGroup
    {

        Enemy[][] mEnemyArray = new Enemy[5][];
        GraphicsDevice graphicsDevice;
        private Game1 game;

        public List<Bullet> mBullets = new List<Bullet>();
        public List<Bullet> mDeadBullets = new List<Bullet>();
        ContentManager mContentManager;

        public MoveDirection moveDirection { get; set; }
        public EdgeTouch edgeTouch { get; set; }

        public enum MoveDirection
        {
            RIGHT, LEFT, DOWN
        }

        public enum EdgeTouch
        {
            RIGHT, LEFT, NONE
        }
        
        public EnemyGroup(GraphicsDevice gDevice, Game1 game)
        {
            moveDirection = MoveDirection.RIGHT;
            edgeTouch = EdgeTouch.NONE;
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
            mContentManager = content;
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
            // Update the bullets already fired
            foreach (Bullet aBullet in mBullets)
            {
                aBullet.Update(gameTime);
            }

            // Shoot new bullets
            if (new Random().Next(100) > 75)
            {
                Enemy enemy = mEnemyArray[new Random().Next(5)][new Random().Next(11)];

                if (new Random().Next(100) <= 75)
                {
                    enemy = mEnemyArray[0][new Random().Next(6)];
                }
                if (enemy != null)
                {
                    ShootBullet(enemy);
                }
            }

            // Basic management of enemies and collision of bullets
            List<EnemyCoord> deadEnemies = new List<EnemyCoord>();

            // Checking the move direction of the enemy group
            if (moveDirection == MoveDirection.RIGHT)
            {
                for (int j = 10; j >= 0; --j)
                {
                    for (int k = 4; k >= 0; --k)
                    {
                        if (mEnemyArray[k][j] != null)
                        {
                            // Check if touching edge of right screen
                            if (mEnemyArray[k][j].Position.X + mEnemyArray[k][j].FrameSize > graphicsDevice.Viewport.Width)
                            {
                                mEnemyArray[k][j].Position.X = graphicsDevice.Viewport.Width - mEnemyArray[k][j].FrameSize;
                                edgeTouch = EdgeTouch.RIGHT;
                                moveDirection = MoveDirection.DOWN;
                            }
                            mEnemyArray[k][j].Update(gameTime, moveDirection);
                            foreach (Bullet bullet in game.mPlayerSprite.mBullets)
                            {
                                if (mEnemyArray[k][j].Size.Contains(bullet.Position.X + bullet.FrameSize / 2, bullet.Position.Y))
                                {
                                    deadEnemies.Add(new EnemyCoord(k, j));
                                    game.mPlayerSprite.mDeadBullets.Add(bullet);
                                }
                            }
                        }
                    }
                }
            }
            else if (moveDirection == MoveDirection.DOWN)
            {
                for (int j = 10; j >= 0; --j)
                {
                    for (int k = 4; k >= 0; --k)
                    {
                        if (mEnemyArray[k][j] != null)
                        {
                            // Check if moved one enemy down
                            if ((int)mEnemyArray[k][j].Position.Y % 8 == 0)
                            {
                                // Check if enemy is touching right side of screen
                                if (edgeTouch == EdgeTouch.RIGHT)
                                {
                                    moveDirection = MoveDirection.LEFT;
                                    edgeTouch = EdgeTouch.NONE;
                                }
                                // Check if enemy is touching left side of screen
                                else if (edgeTouch == EdgeTouch.LEFT)
                                {
                                    moveDirection = MoveDirection.RIGHT;
                                    edgeTouch = EdgeTouch.NONE;
                                }
                            }
                            mEnemyArray[k][j].Update(gameTime, moveDirection);
                            foreach (Bullet bullet in game.mPlayerSprite.mBullets)
                            {
                                if (mEnemyArray[k][j].Size.Contains(bullet.Position.X + bullet.FrameSize / 2, bullet.Position.Y))
                                {
                                    deadEnemies.Add(new EnemyCoord(k, j));
                                    game.mPlayerSprite.mDeadBullets.Add(bullet);
                                }
                            }
                        }
                    }
                }
            }
            else if (moveDirection == MoveDirection.LEFT)
            {
                for (int j = 0; j < 11; ++j)
                {
                    for (int k = 4; k >= 0; --k)
                    {
                        if (mEnemyArray[k][j] != null)
                        {
                            // Check if touching edge of left screen
                            if ((int)mEnemyArray[k][j].Position.X < 0)
                            {
                                mEnemyArray[k][j].Position.X = 0;
                                edgeTouch = EdgeTouch.LEFT;
                                moveDirection = MoveDirection.DOWN;
                            }
                            mEnemyArray[k][j].Update(gameTime, moveDirection);
                            foreach (Bullet bullet in game.mPlayerSprite.mBullets)
                            {
                                if (mEnemyArray[k][j].Size.Contains(bullet.Position.X + bullet.FrameSize / 2, bullet.Position.Y))
                                {
                                    deadEnemies.Add(new EnemyCoord(k, j));
                                    game.mPlayerSprite.mDeadBullets.Add(bullet);
                                }
                            }
                        }
                    }
                }
            }

            // Remove dead enemies from the enemy array
            foreach (EnemyCoord coord in deadEnemies)
            {
                mEnemyArray[coord.X][coord.Y] = null;
            }

            // Check if bullets have touched shield
            foreach (Bullet bullet in mBullets)
            {
                foreach (Shield shield in game.mShieldSprites)
                {
                    if (shield.Size.Contains(bullet.Position.X + bullet.FrameSize / 2, bullet.Position.Y))
                    {
                        mDeadBullets.Add(bullet);
                    }
                }
            }

            // Remove bullets that have been hit by the enemy from the player sprite
            foreach (Bullet bullet in game.mPlayerSprite.mDeadBullets)
            {
                game.mPlayerSprite.mBullets.Remove(bullet);
            }

            // Remove bullets that collided with the shield
            foreach (Bullet bullet in mDeadBullets)
            {
                mBullets.Remove(bullet);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Draw enemy bulllets
            foreach (Bullet aBullet in mBullets)
            {
                aBullet.Draw(spriteBatch);
            }

            // Draw the enemies
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

        private void ShootBullet(Enemy enemy)
        {
            for (int i = 0; i < mBullets.Count; i++)
            {
                if (mBullets[i].Position.Y > graphicsDevice.Viewport.Height)
                    mBullets.RemoveAt(i);
            }
            Bullet aBullet = new Bullet("laser");
            aBullet.LoadContent(mContentManager);
            aBullet.Fire(enemy.Position - new Vector2((enemy.FrameSize / 2 - aBullet.Size.Width / 2), 12), new Vector2(200, 200), new Vector2(0, 1));
            mBullets.Add(aBullet);
        }
    }
}
