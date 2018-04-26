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
        public int points;
        public bool it;
        const int minSpeed = 1;
        const int maxSpeed = 7;
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
            updateSpeed();
        }

        public void updateSpeed()
        {
            if (points / 50.0 == 0)
            {
                speed = 1;
            }
            else
            {
                speed = points / 50 * (speed / Math.Abs(speed));
            }
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
            if (bumpT == 0 || bumpT % 8 == 0)
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
            if (!it || (iFrames == 0 && it))
            {

                pRect.X += (int)(gp.ThumbSticks.Left.X * speed);
                if (pRect.Intersects(otherP.pRect))
                {
                    ifCollide(otherP);
                    pRect.X -= (int)(gp.ThumbSticks.Left.X * speed);
                    otherP.speed *= -1;
                    otherP.bumpT = 20;
                    speed *= -1;
                    bumpT = 20;

                }
                pRect.Y -= (int)(gp.ThumbSticks.Left.Y * speed);
                if (pRect.Intersects(otherP.pRect))
                {
                    ifCollide(otherP);

                    pRect.Y += (int)(gp.ThumbSticks.Left.Y * speed);
                    otherP.speed *= -1;
                    otherP.bumpT = 20;
                    speed *= -1;
                    bumpT = 20;
                }
                if (pindex == 1)
                {
                    if (kb.IsKeyDown(Keys.W))
                    {
                        pRect.Y -= speed;
                        if (pRect.Intersects(otherP.pRect))
                        {
                            ifCollide(otherP);
                            pRect.Y += speed;
                            otherP.speed *= -1;
                            otherP.bumpT += 8;
                            speed *= -1;
                            bumpT += 8;
                        }
                    }
                    if (kb.IsKeyDown(Keys.S))
                    {
                        pRect.Y += speed;
                        if (pRect.Intersects(otherP.pRect))
                        {
                            ifCollide(otherP);
                            pRect.Y -= speed;
                            otherP.speed *= -1;
                            otherP.bumpT += 8;
                            speed *= -1;
                            bumpT += 8;
                        }
                    }
                    if (kb.IsKeyDown(Keys.A))
                    {
                        pRect.X -= speed;
                        if (pRect.Intersects(otherP.pRect))
                        {
                            ifCollide(otherP);
                            pRect.X += speed;
                            otherP.speed *= -1;
                            otherP.bumpT += 8;
                            speed *= -1;
                            bumpT += 8;
                        }
                    }
                    if (kb.IsKeyDown(Keys.D))
                    {
                        pRect.X += speed;
                        if (pRect.Intersects(otherP.pRect))
                        {
                            ifCollide(otherP);
                            pRect.X -= speed;
                            otherP.speed *= -1;
                            otherP.bumpT += 8;
                            speed *= -1;
                            bumpT += 8;
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
                            ifCollide(otherP);
                            pRect.Y += speed;
                            otherP.speed *= -1;
                            otherP.bumpT += 8;
                            speed *= -1;
                            bumpT += 8;
                        }
                    }
                    if (kb.IsKeyDown(Keys.Down))
                    {
                        pRect.Y += speed;
                        if (pRect.Intersects(otherP.pRect))
                        {
                            ifCollide(otherP);

                            pRect.Y -= speed;
                            otherP.speed *= -1;
                            otherP.bumpT += 8;
                            speed *= -1;
                            bumpT += 8;
                        }
                    }
                    if (kb.IsKeyDown(Keys.Left))
                    {
                        pRect.X -= speed;
                        if (pRect.Intersects(otherP.pRect))
                        {
                            ifCollide(otherP);
                            pRect.X += speed;
                            otherP.speed *= -1;
                            otherP.bumpT += 8;
                            speed *= -1;
                            bumpT += 8;
                        }
                    }
                    if (kb.IsKeyDown(Keys.Right))
                    {
                        pRect.X += speed;
                        if (pRect.Intersects(otherP.pRect))
                        {
                            ifCollide(otherP);
                            pRect.X -= speed;
                            otherP.speed *= -1;
                            otherP.bumpT += 8;
                            speed *= -1;
                            bumpT += 8;
                        }
                    }
                }
            }
        }
        private void ifCollide(Player otherP)
        {
            if (it && otherP.iFrames == 0)
            {
                it = false;
                otherP.it = true;
                iFrames = 40;
            }
            if (!it && iFrames == 0)
            {
                it = true;
                otherP.it = false;
                otherP.iFrames = 40;
            }
        }
    }
}
