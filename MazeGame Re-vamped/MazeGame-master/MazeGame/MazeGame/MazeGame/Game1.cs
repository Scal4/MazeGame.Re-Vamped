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
    ///
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font1;
        SpriteFont bigFont;
        KeyboardState oldKb;

        Color CurrentBackgroundC;
        Color splashScreenColor;

        // Class objects
        Map CurrentMap;
        Map firstMap;
        Menus menus;
        Player p1;
        Player p2;
        Audio audio;

        // Enum stuff
        GameState gameState;
        InstructionState instructionState;

        // Rectangles and Textures
        Rectangle selecterArrow;
        Rectangle splashScreen;
        Rectangle p1Border; //p1 has a blue border
        Rectangle p2Border; //p2 has a green border

        Texture2D allPurposeTexture;
        Texture2D selecterArrowTexture;
        Texture2D splashScreenTexture;
        
        // Doubles
        double ScreenWidth;
        double ScreenHeight;

        // Ints
        int red;
        int blue;
        int green;
        int timer;
        int roundOverTimer;
        int gameOverTimer;

        // Lists
        List<Song> songs;
        
        const float stringScale = 0.5625f;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            ScreenHeight = graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            ScreenWidth = graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
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
            gameState = GameState.SplashScreen;
            instructionState = InstructionState.Page1;
            selecterArrow = new Rectangle(GraphicsDevice.Viewport.Width / 3, 500, 50, 50);
            splashScreen = new Rectangle(0, 0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            red = 0;
            blue = 0;
            green = 0;
            timer = 0;
            roundOverTimer = 0;
            gameOverTimer = 0;
            
            CurrentBackgroundC = Color.Black;
            splashScreenColor = new Color(red, green, blue);

            songs = new List<Song>();

            oldKb = Keyboard.GetState();

            IsMouseVisible = true;

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
            p1 = new Player(true, new Rectangle(80, 80, 20, 20), allPurposeTexture);
            p2 = new Player(false, new Rectangle(140, 80, 20, 20), allPurposeTexture);
            p1Border = new Rectangle(p1.pRect.X + 2, p1.pRect.Y + 2, p1.pRect.Width - 4, p1.pRect.Height - 4);
            p2Border = new Rectangle(p2.pRect.X + 2, p2.pRect.Y + 2, p2.pRect.Width - 4, p2.pRect.Height - 4);

            // Songs
            songs.Add(this.Content.Load<Song>("Menu_Audio"));

            CurrentMap = firstMap;

            menus = new Menus(gameState, instructionState, GraphicsDevice, stringScale, ScreenWidth, ScreenHeight, bigFont, font1, selecterArrowTexture, splashScreenTexture, splashScreen);
            audio = new Audio(songs, gameState);

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
            // Add your update logic here
            KeyboardState kb = Keyboard.GetState();

            // Methods from the Menus class to run the menus
            menus.Navigations(gameState, instructionState, kb, CurrentBackgroundC, oldKb, timer, roundOverTimer, gameOverTimer);
            gameState = menus.GameStateValue();
            instructionState = menus.InstructionStateValue();

            if (gameState == GameState.Game)
            {
                CurrentBackgroundC = firstMap.FLOORCOLOR;
                p1.update(1, p2);
                p2.update(2, p1);
                p1Border = new Rectangle(p1.pRect.X + 3, p1.pRect.Y + 3, p1.pRect.Width - 6, p1.pRect.Height - 6);
                p2Border = new Rectangle(p2.pRect.X + 3, p2.pRect.Y + 3, p2.pRect.Width - 6, p2.pRect.Height - 6);
            }

            // Keep so the player can choose to exit the game
            if(gameState == GameState.Exit)
            {
                this.Exit();
            }

            // Splash screen timer
            timer++;

            if (MediaPlayer.State == MediaState.Stopped)
            {
                MediaPlayer.Play(songs[0]);
            }

            oldKb = kb;

            base.Update(gameTime);
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
            // Draws the menus and splash screen
            menus.drawMenus(spriteBatch, gameState, instructionState, timer);

            // Draws game screen
            if (gameState == GameState.Game)
            {
                for (int row = 0; row < CurrentMap.tileMap.GetLength(0); row++)
                {
                    for (int column = 0; column < CurrentMap.tileMap.GetLength(1); column++)
                    {
                        if (CurrentMap.tileMap[row, column] != null)
                            spriteBatch.Draw(CurrentMap.tileMap[row, column].TileTexture, CurrentMap.tileMap[row, column].TileRect, CurrentMap.tileMap[row, column].TileColor);
                    }
                }
                spriteBatch.Draw(allPurposeTexture, p1.pRect, Color.Blue);
                spriteBatch.Draw(allPurposeTexture, p2.pRect, Color.Green);
                if(p1.it==false)
                {
                    spriteBatch.Draw(p1.pText, p1Border, Color.Red);
                    spriteBatch.Draw(p2.pText, p2Border, Color.White);
                }
                else
                {
                    spriteBatch.Draw(p1.pText, p1Border, Color.White);
                    spriteBatch.Draw(p2.pText, p2Border, Color.Red);
                }
                spriteBatch.DrawString(font1, "P1 score: " + p1.points, new Vector2(5, 10), Color.White);
                spriteBatch.DrawString(font1, "P2 score: " + p2.points, new Vector2(GraphicsDevice.Viewport.Width/2, 10), Color.White);
            
            }
            
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
