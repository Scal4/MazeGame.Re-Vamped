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
    enum GameState
    {
        StartScreen,
        Instructions,
        Game
    }

    enum InstructionState
    {
        Page1,
        Page2,
        Page3
    }

    class Menus
    {
        Rectangle selecterArrow;
        GraphicsDevice g;
        float stringScale;
        double ScreenWidth;
        double ScreenHeight;
        SpriteFont bigFont;
        SpriteFont font1;
        float AspectRatio;
        Texture2D selecterArrowTexture;
        GameState gameState;
        InstructionState instructionState;

        public Menus(GameState gameState, InstructionState instructionState, GraphicsDevice graphicsDevice, float stringScale, double ScreenWidth, double ScreenHeight, SpriteFont bigFont, SpriteFont font1, Texture2D selecterArrowTexture)
        {
            g = graphicsDevice;
            selecterArrow = new Rectangle(g.Viewport.Width / 3, 500, 50, 50);
            this.stringScale = stringScale;
            this.ScreenWidth = ScreenWidth;
            this.ScreenHeight = ScreenHeight;
            this.bigFont = bigFont;
            this.font1 = font1;
            AspectRatio = (float)ScreenWidth / (float)ScreenHeight;
            this.selecterArrowTexture = selecterArrowTexture;
        }

        public void Navigations(GameState gameState, InstructionState instructionState, KeyboardState kb, Color CurrentBackgroundC, KeyboardState oldKb)
        {
            // Code for start screen
            if (gameState == GameState.StartScreen)
            {
                CurrentBackgroundC = Color.Black;

                if (kb.IsKeyDown(Keys.Up) && oldKb != kb)
                {
                    selecterArrow.Y = selecterArrow.Y - 100;
                }

                if (kb.IsKeyDown(Keys.Down) && oldKb != kb)
                {
                    selecterArrow.Y = selecterArrow.Y + 100;
                }

                if (selecterArrow.Y < 500)
                {
                    selecterArrow.Y = 700;
                }

                if (selecterArrow.Y > 700)
                {
                    selecterArrow.Y = 500;
                }

                if (kb.IsKeyDown(Keys.Enter) && selecterArrow.Y == 500)
                {
                    this.gameState =  GameState.Instructions;
                }

                if (kb.IsKeyDown(Keys.Enter) && selecterArrow.Y == 600)
                {
                    this.gameState =  GameState.Game;
                }

                if (kb.IsKeyDown(Keys.Enter) && selecterArrow.Y == 700)
                {
                    //this.Exit();
                }
            }

            // Code for insturction screen
            if (gameState == GameState.Instructions)
            {
                if (kb.IsKeyDown(Keys.Escape) && oldKb != kb)
                {
                    this.gameState = GameState.StartScreen;
                }

                if (kb.IsKeyDown(Keys.Right) && instructionState == InstructionState.Page1 && oldKb != kb)
                {
                    this.instructionState = InstructionState.Page2;
                }

                else if (kb.IsKeyDown(Keys.Right) && instructionState == InstructionState.Page2 && oldKb != kb)
                {
                    this.instructionState = InstructionState.Page3;
                }

                else if (kb.IsKeyDown(Keys.Left) && instructionState == InstructionState.Page3 && oldKb != kb)
                {
                    this.instructionState = InstructionState.Page2;
                }

                else if (kb.IsKeyDown(Keys.Left) && instructionState == InstructionState.Page2 && oldKb != kb)
                {
                    this.instructionState = InstructionState.Page1;
                }
            }
        }

        public InstructionState InstructionStateValue()
        {
            return instructionState;
        }

        public GameState GameStateValue()
        {
            return gameState;
        }


        public void drawMenus(SpriteBatch spriteBatch, GameState gameState, InstructionState instructionState)
        {
            if (gameState == GameState.StartScreen)
            {
                spriteBatch.DrawString(bigFont, "Crazy Mazey Tag", new Vector2(g.Viewport.Width / 5, 25), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(bigFont, "Crazy Mazey Tag", new Vector2(g.Viewport.Width / 5, 25), Color.White, 0f, new Vector2(10, 0), (float)AspectRatio * stringScale, SpriteEffects.None,
                                        0f);
                spriteBatch.DrawString(font1, "Use the Arrow Keys to move the arrow", new Vector2(g.Viewport.Width / 4 - 30, 200), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale,
                                        SpriteEffects.None, 0f);
                spriteBatch.DrawString(font1, "Use the Enter Key to select an option", new Vector2(g.Viewport.Width / 4 - 30, 250), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale,
                                        SpriteEffects.None, 0f);
                spriteBatch.DrawString(font1, "Instructions", new Vector2(g.Viewport.Width / 3 + 100, 500), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font1, "Start Game", new Vector2(g.Viewport.Width / 3 + 100, 600), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font1, "Exit", new Vector2(g.Viewport.Width / 3 + 100, 700), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                spriteBatch.Draw(selecterArrowTexture, selecterArrow, Color.White);
            }
            if (gameState == GameState.Instructions)
            {
                if (instructionState == InstructionState.Page1)
                {
                    spriteBatch.DrawString(bigFont, "Instructions", new Vector2((int)ScreenWidth / 4, 25), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(bigFont, "Instructions", new Vector2((int)ScreenWidth / 4, 25), Color.White, 0f, new Vector2(10, 0), (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Controls:\nPlayer 1: W - Up, A - Left, S - Down, D - Right\nPlayer 2: Arrow Keys", new Vector2(g.Viewport.Width / 4, 200),
                                            Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Red Square - Tagger", new Vector2(g.Viewport.Width / 4, 400), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale,
                                            SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "White Square - Runner", new Vector2(g.Viewport.Width / 4, 450), Color.White, 0f, Vector2.Zero, (float)AspectRatio * stringScale,
                                            SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Player 1 has a Blue Border", new Vector2(g.Viewport.Width / 4, 500), Color.CornflowerBlue, 0f, Vector2.Zero, (float)AspectRatio * stringScale,
                                           SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Player 2 has a Green Border", new Vector2(g.Viewport.Width / 4, 550), Color.Green, 0f, Vector2.Zero, (float)AspectRatio * stringScale,
                                            SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Press Escape to go back", new Vector2(g.Viewport.Width / 4, 800), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale,
                                            SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Next Page - Right Arrow Key", new Vector2(g.Viewport.Width / 4, 850), Color.Red, 0f, Vector2.Zero,
                                            (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                }

                else if (instructionState == InstructionState.Page2)
                {
                    spriteBatch.DrawString(bigFont, "Instructions", new Vector2((int)ScreenWidth / 4, 25), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(bigFont, "Instructions", new Vector2((int)ScreenWidth / 4, 25), Color.White, 0f, new Vector2(10, 0), (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Tiles:", new Vector2(g.Viewport.Width / 6, 200), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Door - Opens for a limited time when player uses it", new Vector2(g.Viewport.Width / 6, 250), Color.Red, 0f, Vector2.Zero,
                                            (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Speed Tiles - Has the chance to either increase or decrease\nthe player's speed temporarily", new Vector2(g.Viewport.Width / 6, 300),
                                            Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Slow Block - Player's speed will be decreased for a short time", new Vector2(g.Viewport.Width / 6, 400), Color.Red, 0f, Vector2.Zero,
                                            (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Speed Block - Player's speed will be increased for a short time", new Vector2(g.Viewport.Width / 6, 450), Color.Red, 0f, Vector2.Zero,
                                            (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Press Escape to go back", new Vector2(g.Viewport.Width / 4, 800), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale,
                                            SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Next Page - Right Arrow Key\nPrevious Page - Left Arrow Key", new Vector2(g.Viewport.Width / 4, 850), Color.Red, 0f, Vector2.Zero,
                                            (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                }

                else if (instructionState == InstructionState.Page3)
                {
                    spriteBatch.DrawString(bigFont, "Instructions", new Vector2((int)ScreenWidth / 4, 25), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(bigFont, "Instructions", new Vector2((int)ScreenWidth / 4, 25), Color.White, 0f, new Vector2(10, 0), (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Power-Ups:", new Vector2(g.Viewport.Width / 6, 200), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Thunderbolt - Stuns other players for one second.\nRunner and Tagger can use this", new Vector2(g.Viewport.Width / 6, 250), Color.Red, 0f,
                                            Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Ghost - Ignore tile effects. Can pass through doors at any time.\nGhost cannot be tagged. Lasts for a few seconds",
                                            new Vector2(g.Viewport.Width / 6, 350), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Flicker - Screen flashes, making it more difficult for\nplayers to see for 5 seconds", new Vector2(g.Viewport.Width / 6, 450),
                                            Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Press Escape to go back", new Vector2(g.Viewport.Width / 4, 800), Color.Red, 0f, Vector2.Zero, (float)AspectRatio * stringScale,
                                            SpriteEffects.None, 0f);
                    spriteBatch.DrawString(font1, "Previous Page - Left Arrow Key", new Vector2(g.Viewport.Width / 4, 850), Color.Red, 0f, Vector2.Zero,
                                            (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                }
            }
        }
    }
}
