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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        // All Fonts used in game
        SpriteFont font1;
        SpriteFont bigFont;

        KeyboardState oldKb;
        
        Color CurrentBackgroundC;
        Color splashScreenColor;

        // All Maps in game
        Map CurrentMap;
        Map firstMap;

        Menus menus;

        // Players in the game
        Player p1;
        Player p2;
        String winner;
                
        // Enum stuff
        GameState gameState;
        InstructionState instructionState;

        // Rectangles
        Rectangle splashScreen;
        Rectangle selecterArrow;
        Rectangle p1Border;
        Rectangle p2Border;

        // Textures for the game ( possible to put them in their resprective classes?
        Texture2D allPurposeTexture;
        Texture2D selecterArrowTexture;
        Texture2D splashScreenTexture;

        // Can we put these in Menu's?
        double ScreenWidth;
        double ScreenHeight;

        // Colors
        int red;
        int blue;
        int green;

        // Timers
        int timer;
        int gameOverTimer;

        // Lists
        List<Song> songs;
        int pSize;

        // bools
        bool changeSongs;

        const float stringScale = 0.5625f;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ScreenHeight = graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            ScreenWidth = graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            graphics.ToggleFullScreen();
            graphics.ApplyChanges();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            pSize = GraphicsDevice.Viewport.Width / 80;
            gameState = GameState.StartScreen;
            instructionState = InstructionState.Page1;
            selecterArrow = new Rectangle(GraphicsDevice.Viewport.Width / 3, 500, 50, 50);
            splashScreen = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            red = 0;
            blue = 0;
            green = 0;
            timer = 0;
            gameOverTimer = 0;

            CurrentBackgroundC = Color.Black;
            splashScreenColor = new Color(red, green, blue);

            songs = new List<Song>();

            oldKb = Keyboard.GetState();

            IsMouseVisible = true;

            changeSongs = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            allPurposeTexture = this.Content.Load<Texture2D>("white");
            selecterArrowTexture = this.Content.Load<Texture2D>("arrow");
            splashScreenTexture = this.Content.Load<Texture2D>("splashScreen");
            font1 = this.Content.Load<SpriteFont>("SpriteFont1");
            bigFont = this.Content.Load<SpriteFont>("SpriteFont2");

            firstMap = new Map(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, allPurposeTexture, "Content/MazeGameMap.txt");
            CurrentMap = firstMap;

            p1 = new Player(true, pSize, allPurposeTexture, CurrentMap,1);
            p2 = new Player(false, pSize, allPurposeTexture, CurrentMap,2);
            while(p1.pRect.Intersects(p2.pRect))
            {
                p2.newRandomStart();
            }
            p1Border =new Rectangle(p1.pRect.X + p1.pRect.Width / 7, p1.pRect.Y + p1.pRect.Width / 7, p1.pRect.Width - ((p1.pRect.Width / 7) * 2), p1.pRect.Height - ((p1.pRect.Width / 7) * 2));
            p2Border = new Rectangle(p2.pRect.X + p2.pRect.Width / 7, p2.pRect.Y + p2.pRect.Width / 7, p2.pRect.Width - ((p2.pRect.Width / 7) * 2), p2.pRect.Height - ((p2.pRect.Width / 7) * 2));

            // Songs
            songs.Add(this.Content.Load<Song>("Menu_Audio"));
            songs.Add(this.Content.Load<Song>("In_Game_Song"));


            menus = new Menus(gameState, instructionState, GraphicsDevice, stringScale, ScreenWidth, ScreenHeight, bigFont, font1, selecterArrowTexture, splashScreenTexture, allPurposeTexture, splashScreen);

            MediaPlayer.Play(songs[0]);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // escape key missing

            // Add your update logic here
            KeyboardState kb = Keyboard.GetState();

            if (kb.IsKeyDown(Keys.Back) && !oldKb.IsKeyDown(Keys.Back))
            {
                this.Exit();
            }

            // Methods from the Menus class to run the menus
            menus.Navigations(gameState, instructionState, kb, CurrentBackgroundC, oldKb, timer, gameOverTimer);
            gameState = menus.GameStateValue();
            instructionState = menus.InstructionStateValue();

            if (gameState == GameState.Game)
            {
                if (changeSongs == false)
                {
                    changeSongs = true;
                    MediaPlayer.Stop();
                }

                // Pauses game. Currently does not work
                if(kb.IsKeyDown(Keys.P) && oldKb != kb)
                {
                    gameState = GameState.Paused;
                }
                
                // Update Player collision with map obstacles.
                firstMap.MapPlayerCollisions(p1);
                firstMap.MapPlayerCollisions(p2);

                CurrentBackgroundC = firstMap.FLOORCOLOR;

                p1.update(1, p2);
                p2.update(2, p1);


                RoundOverCheck();
            }

                if(gameState == GameState.StartScreen)
                {
                    p1.Reset();
                    p2.Reset();
                    p1.TotalReset();
                    p2.TotalReset();
                    CurrentBackgroundC = Color.Black;
                }

            if (menus.ReturnTimer() >= 240)
            {
                //Reset
                p1.Reset();
                p2.Reset();
                while (p1.pRect.Intersects(p2.pRect))
                {
                    p2.newRandomStart();
                }
                //CurrentBackgroundC = Color.Black;
            }

            // Keep so the player can choose to exit the game
            if (gameState == GameState.Exit)
            {
                this.Exit();
            }

            // Splash screen timer
            timer++;

            if ((gameState == GameState.StartScreen || gameState == GameState.Instructions) && (MediaPlayer.State == MediaState.Stopped || changeSongs == true))
            {
                if (changeSongs == true)
                {
                    changeSongs = false;
                    MediaPlayer.Stop();
                }

               MediaPlayer.Play(songs[0]);
            }

            if(gameState == GameState.Game && MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(songs[1]);
            }

            if(gameState == GameState.RoundOver)
            {
                MediaPlayer.Pause();
            }
            else
            {
                MediaPlayer.Resume();
            }
            if (gameState == GameState.GameOver)
            {
                MediaPlayer.Pause();
                changeSongs = true;
            }

            oldKb = kb;

            base.Update(gameTime);
        }

        public void RoundOverCheck()
        {
            if (p1.winCheck() == GameState.RoundOver)
            {
                gameState = GameState.RoundOver;
            }

            else if (p2.winCheck() == GameState.RoundOver)
            {
                gameState = GameState.RoundOver;
            }
            if (p1.NumOfLosses > p2.NumOfLosses)
            {
                winner = "Player 2 has won";
            }
            else
            {
                winner = "Player 1 has won";
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(CurrentBackgroundC);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            // Draws the Map
            CurrentMap.MapDraw(gameState, spriteBatch);

            // Draws the menus and splash screen
            menus.drawMenus(spriteBatch, gameState, instructionState, timer, winner);

            // Draws game screen
            if (gameState == GameState.Game)
            {

                // Draws the players
                p1.DrawP(spriteBatch, 1, font1, graphics);
                p2.DrawP(spriteBatch, 2, font1, graphics);
                // put all the above in a method in player class
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
