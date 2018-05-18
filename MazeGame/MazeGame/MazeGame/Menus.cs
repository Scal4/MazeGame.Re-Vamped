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
        SplashScreen,
        StartScreen,
        Instructions,
        Game,
        RoundOver,
        GameOver,
        Exit,
        End,
        Paused
    }

    enum InstructionState
    {
        Page1,
        Page2,
        Page3
    }

    class Menus
    {
        Player p1;
        Player p2;
        GameState MenuGameState;
        InstructionState instructionState;
        Rectangle selecterArrow;
        Rectangle splashScreen;
        GraphicsDevice g;
        SpriteFont bigFont;
        SpriteFont font1;
        Texture2D selecterArrowTexture;
        Texture2D splashScreenTexture;
        Texture2D allPurposeTexture;
        Color splashScreenColor;
        Color CurrentBackgroundC;
        float stringScale;
        double ScreenWidth;
        double ScreenHeight;
        float AspectRatio;
        int timer;
        int roundOverTimer;
        int gameOverTimer;
        int red;
        int blue;
        int green;
        public int round;
        int seconds;
        int gameOverSeconds;
        int p1Wins;
        int p2Wins;
        int currentP1Points;
        int currentP2Points;
        string roundString;
        string roundCountString;
        string gameOverString;
        string gameOverCountString;
        string p1RoundWin;
        string p2RoundWin;
        string p1GameWin;
        string p2GameWin;

        public Menus(GameState gameState, InstructionState instructionState,
                     GraphicsDevice graphicsDevice,
                     float stringScale, double ScreenWidth, double ScreenHeight,
                     SpriteFont bigFont, SpriteFont font1,
                     Texture2D selecterArrowTexture, Texture2D splashScreenTexture, Texture2D allPurposeTexture,
                     Rectangle splashScreen)
        {
            this.g = graphicsDevice;
            this.stringScale = stringScale;
            this.ScreenWidth = ScreenWidth;
            this.ScreenHeight = ScreenHeight;
            this.bigFont = bigFont;
            this.font1 = font1;
            this.selecterArrowTexture = selecterArrowTexture;
            this.splashScreenTexture = splashScreenTexture;
            this.splashScreen = splashScreen;
            this.allPurposeTexture = allPurposeTexture;

            red = 0;
            green = 0;
            blue = 0;
            splashScreenColor = new Color(red, green, blue);

            roundOverTimer = 0;
            gameOverTimer = 0;
            round = 1;
            seconds = 0;
            gameOverSeconds = 0;
            p1Wins = 0;
            p2Wins = 0;

            roundString = "Round Over";
            roundCountString = "Next round starts in ";
            gameOverString = "Game Over";
            gameOverCountString = "Return to Start Screen in ";
            p1RoundWin = "Player 1 wins the round!";
            p2RoundWin = "Player 2 wins the round!";
            p1GameWin = "Player 1 Wins!";
            p2GameWin = "Player 2 Wins!";

            selecterArrow = new Rectangle(g.Viewport.Width / 3, 500, 50, 50);
            timer = 0;
            AspectRatio = (float)ScreenWidth / (float)ScreenHeight;

            p1 = new Player(true, new Rectangle(200, 100, 20, 20), allPurposeTexture);
            p2 = new Player(false, new Rectangle(240, 100, 20, 20), allPurposeTexture);

            currentP1Points = p1.points;
            currentP2Points = p1.points;
        }

        // Method for menu navigations and splash screen. Round over and game over screens still need work
        public void Navigations(GameState gameState, InstructionState instructionState, KeyboardState kb, Color CurrentBackgroundC, KeyboardState oldKb,
                                int timer, int gameOverTimer)
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
                    this.MenuGameState = GameState.Instructions;
                }

                if (kb.IsKeyDown(Keys.Enter) && selecterArrow.Y == 600)
                {
                    this.MenuGameState = GameState.Game;
                }

                if (kb.IsKeyDown(Keys.Enter) && selecterArrow.Y == 700)
                {
                    this.MenuGameState = GameState.Exit;
                }
            }

            // Code for instruction screen
            if (gameState == GameState.Instructions)
            {
                if (kb.IsKeyDown(Keys.Escape) && oldKb != kb)
                {
                    this.MenuGameState = GameState.StartScreen;
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

            // Splash Screen code
            if (gameState == GameState.SplashScreen)
            {
                timer++;

                if (timer >= 370)
                {
                    this.MenuGameState = GameState.StartScreen;
                }
            }

            // Code for round over screen
            if (gameState == GameState.RoundOver)
            {
                if (round == 3)
                {
                    MenuGameState = GameState.GameOver;
                }

                if (round != 3)
                {
                    if (roundOverTimer <= 60)
                    {
                        seconds = 3;
                    }

                    else if (roundOverTimer <= 120)
                    {
                        seconds = 2;
                    }

                    else if (roundOverTimer <= 180)
                    {
                        seconds = 1;
                    }

                    if (roundOverTimer >= 240)
                    {
                        if (p1.points == 50)
                        {
                            p2Wins += 1;
                        }

                        else if (p2.points == 50)
                        {
                            p1Wins += 1;
                        }

                        seconds = 0;
                        // Resets game
                        this.MenuGameState = GameState.Game;
                        roundOverTimer = 0;
                        round++;
                    }
                }

                roundOverTimer++;
            }

            // Code for game over screen
            if (gameState == GameState.GameOver)
            {
                if (gameOverTimer <= 60)
                {
                    gameOverSeconds = 3;
                }

                else if (gameOverTimer <= 120)
                {
                    gameOverSeconds = 2;
                }

                else if (gameOverTimer <= 180)
                {
                    gameOverSeconds = 1;
                }

                if (gameOverTimer >= 240)
                {
                    gameOverSeconds = 0;
                    this.MenuGameState = GameState.StartScreen;
                    gameOverTimer = 0;
                }

                gameOverTimer++;
            }
        }

        public int ReturnTimer()
        {
            return roundOverTimer;
        }

        // Methods to run the states 
        public InstructionState InstructionStateValue()
        {
            return instructionState;
        }

        public GameState GameStateValue()
        {
            return MenuGameState;
        }

        // Draws the splash screen, menus, round over and game over screens
        public void drawMenus(SpriteBatch spriteBatch, GameState gameState, InstructionState instructionState, int timer)
        {
            // Start screen
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

            // Instruction screen
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
                    spriteBatch.DrawString(font1, "Speed Tiles - Has the chance to either increase or decrease\nthe player's speed temporarily(gray)", new Vector2(g.Viewport.Width / 6, 300),
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

            // Splash screen
            if (gameState == GameState.SplashScreen)
            {
                if (timer > 0 && timer < 120)
                {
                    red = red + 3;
                    blue = blue + 3;
                    green = green + 3;
                    splashScreenColor = new Color(red, green, blue);
                    spriteBatch.Draw(splashScreenTexture, splashScreen, splashScreenColor);
                }

                if (timer > 119 && timer < 241)
                {
                    spriteBatch.Draw(splashScreenTexture, splashScreen, splashScreenColor);
                }

                if (timer > 240 && timer < 360)
                {
                    red = red - 3;
                    blue = blue - 3;
                    green = green - 3;
                    splashScreenColor = new Color(red, green, blue);
                    spriteBatch.Draw(splashScreenTexture, splashScreen, splashScreenColor);
                }
            }

            // Round over screen
            if (gameState == GameState.RoundOver)
            {
                spriteBatch.DrawString(bigFont, roundString, new Vector2((int)ScreenWidth / 3 - 50, (int)ScreenHeight / 3), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(bigFont, roundString, new Vector2((int)ScreenWidth / 3 - 50, (int)ScreenHeight / 3), Color.White, 0f, new Vector2(10, 0), (float)AspectRatio * stringScale, SpriteEffects.None, 0f);

                if (p2.points == 50)
                {
                    spriteBatch.DrawString(font1, p1RoundWin, new Vector2((int)ScreenWidth / 3 + 25, (int)ScreenHeight / 2), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                }

                else if (p1.points == 50)
                {
                    spriteBatch.DrawString(font1, p2RoundWin, new Vector2((int)ScreenWidth / 3 + 25, (int)ScreenHeight / 2), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                }

                spriteBatch.DrawString(font1, roundCountString + seconds, new Vector2((int)ScreenWidth / 3 + 25, (int)ScreenHeight / 2 + 30), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
            }

            // Game over screen
            if (gameState == GameState.GameOver)
            {
                spriteBatch.DrawString(bigFont, gameOverString, new Vector2((int)ScreenWidth / 3 - 50, (int)ScreenHeight / 3), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(bigFont, gameOverString, new Vector2((int)ScreenWidth / 3 - 50, (int)ScreenHeight / 3), Color.White, 0f, new Vector2(10, 0), (float)AspectRatio * stringScale, SpriteEffects.None, 0f);

                if (round == 3 && (p2Wins == 2 || p2Wins == 3))
                {
                    spriteBatch.DrawString(font1, p2GameWin, new Vector2((int)ScreenWidth / 3 + 25, (int)ScreenHeight / 2), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                }

                else if (round == 3 && (p1Wins == 2 || p1Wins == 3))
                {
                    spriteBatch.DrawString(font1, p1GameWin, new Vector2((int)ScreenWidth / 3 + 25, (int)ScreenHeight / 2), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                }

                spriteBatch.DrawString(font1, gameOverCountString + seconds, new Vector2((int)ScreenWidth / 3 + 25, (int)ScreenHeight / 2), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
            }

            // Pause screen
            if (gameState == GameState.Paused)
            {
                spriteBatch.DrawString(bigFont, "Paused", new Vector2((int)ScreenWidth / 3 - 50, (int)ScreenHeight / 3), Color.Blue, 0f, Vector2.Zero, (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
                spriteBatch.DrawString(bigFont, "Paused", new Vector2((int)ScreenWidth / 3 - 50, (int)ScreenHeight / 3), Color.White, 0f, new Vector2(10, 0), (float)AspectRatio * stringScale, SpriteEffects.None, 0f);
            }
        }
    }
}
