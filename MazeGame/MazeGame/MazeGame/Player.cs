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
    enum MoveDirectionV // N
    {
        North, South, None
    }
    enum MoveDirectionH // N
    {
        West, East, None
    }

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
        public bool moveR = true; // N
        public bool moveL = true; // N
        public bool moveU = true; // N
        public bool moveD = true; // N
        public MoveDirectionV moveDirectionV; // N
        public MoveDirectionH moveDirectionH; // N
        public Vector2 previousPos; // N
        public Texture2D pText;
        public Rectangle pRect;
        int bumpT = -1;
        public int speed = 4;
        KeyboardState old;
        int itTime = 0;

        public Player(bool i, Rectangle rect, Texture2D text)
        {
            old = Keyboard.GetState();
            pText = text;
            pRect = rect;
            it = i;
            points = 400;
            pBorder = new Rectangle(pRect.X + 2, pRect.Y + 2, pRect.Width - 4, pRect.Height - 4);
            updateSpeed();
            previousPos = new Vector2(rect.X, rect.Y); // N
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
            CalcPreviousPos(); // N

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
                    pRect.X -= (int)(gp.ThumbSticks.Left.X * speed);
                    ifCollide(otherP);
                }
                pRect.Y -= (int)(gp.ThumbSticks.Left.Y * speed);
                if (pRect.Intersects(otherP.pRect))
                {
                    pRect.Y += (int)(gp.ThumbSticks.Left.Y * speed);
                    ifCollide(otherP);
                }
                // Player 1
                if (pindex == 1)
                {   
                    //Vertical
                    if(kb.IsKeyDown(Keys.W) && moveU == true && kb.IsKeyDown(Keys.S) && moveD == true)
                    {
                        moveDirectionV = MoveDirectionV.None; // N
                    }
                    else if (kb.IsKeyDown(Keys.W) && moveU == true)
                    {
                        pRect.Y -= speed;
                        moveDirectionV = MoveDirectionV.North; // New in all of the directions
                        if (pRect.Intersects(otherP.pRect))
                        {
                            pRect.Y += speed;
                            ifCollide(otherP);
                        }
                    }
                    else if (kb.IsKeyDown(Keys.S) && moveD == true)
                    {
                        pRect.Y += speed;
                        moveDirectionV = MoveDirectionV.South;// N
                        if (pRect.Intersects(otherP.pRect))
                        {
                            pRect.Y -= speed;
                            ifCollide(otherP);
                        }
                    }
                    else
                    {
                        moveDirectionV = MoveDirectionV.None;// N
                    }
                    //Horizontal
                    if (kb.IsKeyDown(Keys.A) && moveL == true && kb.IsKeyDown(Keys.D) && moveR == true)
                    {
                        moveDirectionH = MoveDirectionH.None; // N
                    }
                    else if (kb.IsKeyDown(Keys.A) && moveL == true)
                    {
                        pRect.X -= speed;
                        moveDirectionH = MoveDirectionH.West; // N
                        if (pRect.Intersects(otherP.pRect))
                        {
                            pRect.X += speed;
                            ifCollide(otherP);
                        }
                    }
                    else if (kb.IsKeyDown(Keys.D) && moveR == true)
                    {
                        pRect.X += speed;
                        moveDirectionH = MoveDirectionH.East; // N
                        if (pRect.Intersects(otherP.pRect))
                        {
                            pRect.X -= speed;
                            ifCollide(otherP);
                        }
                    }
                    else
                    {
                        moveDirectionH = MoveDirectionH.None; // N
                    }
                }
                // Player 2
                if (pindex == 2)
                {
                    // Vertical movement
                    if(kb.IsKeyDown(Keys.Up) && moveU == true && kb.IsKeyDown(Keys.Down) && moveD == true)
                    {
                        moveDirectionV = MoveDirectionV.None; // N
                    }
                    else if (kb.IsKeyDown(Keys.Up) && moveU == true)
                    {
                        pRect.Y -= speed;
                        moveDirectionV = MoveDirectionV.North; // N
                        if (pRect.Intersects(otherP.pRect))
                        {
                            pRect.Y += speed;
                            ifCollide(otherP);
                        }
                    }
                    else if (kb.IsKeyDown(Keys.Down) && moveD == true)
                    {
                        pRect.Y += speed;
                        moveDirectionV = MoveDirectionV.South; // N
                        if (pRect.Intersects(otherP.pRect))
                        {
                            pRect.Y -= speed;
                            ifCollide(otherP);
                        }
                    }
                    else
                    {
                        moveDirectionV = MoveDirectionV.None;
                    }
                    // Horizontal movement
                    if(kb.IsKeyDown(Keys.Left) && moveL == true && kb.IsKeyDown(Keys.Right) && moveR == true)
                    {
                        moveDirectionH = MoveDirectionH.None; // N
                    }
                    else if (kb.IsKeyDown(Keys.Left) && moveL == true)
                    {
                        pRect.X -= speed;
                        moveDirectionH = MoveDirectionH.West; // N
                        if (pRect.Intersects(otherP.pRect))
                        {
                            pRect.X += speed;
                            ifCollide(otherP);
                        }
                    }
                    else if (kb.IsKeyDown(Keys.Right) && moveR == true)
                    {
                        pRect.X += speed;
                        moveDirectionH = MoveDirectionH.East; // New 
                        if (pRect.Intersects(otherP.pRect))
                        {
                            pRect.X -= speed;
                            ifCollide(otherP);
                        }
                    }
                    else
                    {
                        moveDirectionH = MoveDirectionH.None; // N
                    }
                }
            }
        }
        private void ifCollide(Player otherP)
        {
            if (otherP.iFrames == 0 && iFrames == 0)
            {
                otherP.speed *= -1;
                otherP.bumpT += 15;
                speed *= -1;
                bumpT += 15;
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

        //Draw player
        public void DrawP(SpriteBatch spriteBatch, int pIndex, SpriteFont font1, GraphicsDeviceManager graphics)
        {
            if (pIndex == 1)
            {
                spriteBatch.Draw(pText, pRect, Color.Blue);
            }
            else if (pIndex == 2)
            {
                spriteBatch.Draw(pText, pRect, Color.Green);
            }
            if (it)
            {
                spriteBatch.Draw(pText, pBorder, Color.Red);
            }
            else
            {
                spriteBatch.Draw(pText, pBorder, Color.White);
            }
            spriteBatch.DrawString(font1, "P" + pIndex + " score: " + (points - 50), new Vector2((pIndex - 1) * (graphics.GraphicsDevice.Viewport.Width / 2) + 8, 10), Color.White);
        }

        // calculates the player previous position based on their speed and direction.
        public void CalcPreviousPos() // N
        {
            Vector2 previousPosCheck = previousPos;
            // Horizontal prev pos
            if(moveDirectionH == MoveDirectionH.East)
            {
                previousPosCheck.X = pRect.X - speed;
            }
            else if(moveDirectionH == MoveDirectionH.West)
            {
                previousPosCheck.X = pRect.X + speed;
            }
            else
            {
                previousPosCheck.X = pRect.X;
            }
            // Vertical prev pos
            if(moveDirectionV == MoveDirectionV.North)
            {
                previousPosCheck.Y = pRect.Y + speed;
            }
            else if(moveDirectionV == MoveDirectionV.South)
            {
                previousPosCheck.Y = pRect.Y - speed;
            }
            else
            {
                previousPosCheck.Y = pRect.Y;
            }

            //final check to make sure previous pos is not null or off screen?
            previousPos = previousPosCheck;
        }
    }
}
