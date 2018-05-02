using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.IO;

namespace MazeGame
{
    class Player
    {
        Rectangle pBorder;
        public int points;
        public bool it;
        const int minSpeed = 2;
        const int maxSpeed = 8;
        public int iFrames = 0;
        int pMovex;
        int pMoveY;
        public Texture2D pText;
        public Rectangle pRect;
        int bumpT = -1;
        int speed = 4;
        KeyboardState old;
        int itTime = 0;
        public Player(bool i, Rectangle rect, Texture2D text)
        {
            old = Keyboard.GetState();
            pText = text;
            pRect = rect;
            it = i;
            points = 400;
            pBorder = new Rectangle(pRect.X + 2, pRect.Y + 2, pRect.Width - 4,pRect.Height - 4);
            updateSpeed();
        }

        public void updateSpeed()
        {
            if (((maxSpeed) - (points / 100)) != 0)
            {
                speed = ((maxSpeed) - (points / 100)) * (speed / Math.Abs(speed));
            }
            /*if (speed == 0)
            {
                speed = 1;
            }*/
            if (Math.Abs(speed) < minSpeed)
            {
                speed = minSpeed * (speed / Math.Abs(speed));
            }
            if (Math.Abs(speed) > maxSpeed)
            {
                speed = maxSpeed * (speed / Math.Abs(speed));
            }
        }

        public void update(int pindex, Player otherP)
        {
            updateSpeed();
            if (iFrames != 0)
            {
                iFrames--;
            }
            if (it)
            {
                itTime++;
                if (itTime % 10 == 0)
                {
                    points--;
                    otherP.points++;
                }
            }
            if (bumpT > 0)
            {
                bumpT--;
            }
            if (bumpT == 0 || bumpT % 15 == 0)
            {
                if (bumpT == 0)
                {
                    speed = Math.Abs(speed);
                }
                else
                {
                    bumpT--;
                    speed *= -1;
                }
            }
            moveP(pindex, otherP);
            pBorder = new Rectangle(pRect.X + 3, pRect.Y + 3, pRect.Width - 6, pRect.Height - 6);
        }

        private void moveP(int pindex, Player otherP)
        {
            GamePadState gp;
            if (pindex == 1)
            {
                gp = GamePad.GetState(PlayerIndex.One);
            }
            else
            {
                gp = GamePad.GetState(PlayerIndex.Two);
            }
            KeyboardState kb = Keyboard.GetState();
            pRect.X += (int)(gp.ThumbSticks.Left.X * speed);
            if (pRect.Intersects(otherP.pRect))
            {
                pRect.X -= (int)(gp.ThumbSticks.Left.X * speed);
                ifCollide(otherP);
            }
            pRect.Y -= (int)(gp.ThumbSticks.Left.Y * speed);
            if (pRect.Intersects(otherP.pRect))
            {
                pRect.Y += (int)(gp.ThumbSticks.Left.Y * speed);
                ifCollide(otherP);
            }
            if (pindex == 1)
            {
                if (kb.IsKeyDown(Keys.W))
                {
                    pRect.Y -= speed;
                    if (pRect.Intersects(otherP.pRect))
                    {
                        pRect.Y += speed;
                        ifCollide(otherP);    
                    }
                }
                if (kb.IsKeyDown(Keys.S))
                {
                    pRect.Y += speed;
                    if (pRect.Intersects(otherP.pRect))
                    {
                        pRect.Y -= speed;
                        ifCollide(otherP);
                    }
                }
                if (kb.IsKeyDown(Keys.A))
                {
                    pRect.X -= speed;
                    if (pRect.Intersects(otherP.pRect))
                    {
                        pRect.X += speed;
                        ifCollide(otherP);       
                    }
                }
                if (kb.IsKeyDown(Keys.D))
                {
                    pRect.X += speed;
                    if (pRect.Intersects(otherP.pRect))
                    {
                        pRect.X -= speed;
                        ifCollide(otherP);
                    }
                }
            }
            if (pindex == 2)
            {
                if (kb.IsKeyDown(Keys.Up))
                {
                    pRect.Y -= speed;
                    if (pRect.Intersects(otherP.pRect))
                    {
                        pRect.Y += speed;
                        ifCollide(otherP);
                    }
                }
                if (kb.IsKeyDown(Keys.Down))
                {
                    pRect.Y += speed;
                    if (pRect.Intersects(otherP.pRect))
                    {
                        pRect.Y -= speed;
                        ifCollide(otherP);
                    }
                }
                if (kb.IsKeyDown(Keys.Left))
                {
                    pRect.X -= speed;
                    if (pRect.Intersects(otherP.pRect))
                    {
                        pRect.X += speed;
                        ifCollide(otherP);
                    }
                }
                if (kb.IsKeyDown(Keys.Right))
                {
                    pRect.X += speed;
                    if (pRect.Intersects(otherP.pRect))
                    {
                        pRect.X -= speed;
                        ifCollide(otherP);
                    }
                }
            }
        }
        public void ifCollide(Rectangle r)
        {
           speed *= -1;
           bumpT += 15;
        }
        private void ifCollide(Player otherP)
        {
            if (otherP.iFrames == 0 && iFrames == 0)
            {
                otherP.speed *= -1;
                otherP.bumpT += 15;
                speed *= -1;
                bumpT +=15;
            }
            if (it && otherP.iFrames == 0)
            {
                it = false;
                otherP.it = true;
                otherP.points -= 10;
                iFrames = 40;
            }
            if (!it && iFrames == 0)
            {
                it = true;
                otherP.it = false;
                points -= 10;
                otherP.iFrames = 40;
            }
        }

        public void DrawP(SpriteBatch spriteBatch, int pIndex, SpriteFont font1,GraphicsDeviceManager graphics)
        {
            if(pIndex==1)
            {
                spriteBatch.Draw(pText, pRect, Color.Blue);
            }
            else if(pIndex==2)
            {
                spriteBatch.Draw(pText, pRect, Color.Green);
            }
            if(it)
            {
                spriteBatch.Draw(pText, pBorder, Color.Red);
            }
            else
            {
                spriteBatch.Draw(pText, pBorder, Color.White);
            }
            spriteBatch.DrawString(font1, "P"+pIndex+" score: " + (points - 50), new Vector2((pIndex-1)*(graphics.GraphicsDevice.Viewport.Width/2)+8, 10), Color.White);
        }
    }
}
