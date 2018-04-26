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
        SpriteFont font1;
        SpriteFont bigFont;
        KeyboardState oldKb;
        Color CurrentBackgroundC;

        Map CurrentMap;
        Map firstMap;

        // Enum stuff
        GameState gameState;
        InstructionState instructionState;

        // Rectangles
        Rectangle selecterArrow;
        Texture2D allPurposeTexture;
        Texture2D selecterArrowTexture;
        double ScreenWidth;
        double ScreenHeight;
        
        const float stringScale = 0.5625f;

        Rectangle p1Border;
        Rectangle p2Border;
        Player p1;
        Player p2;
        //p1 has a blue border
        //p2 has a green border

        Menus menus;
        //for the game states

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
            gameState = GameState.StartScreen;
            instructionState = InstructionState.Page1;
            selecterArrow = new Rectangle(GraphicsDevice.Viewport.Width / 3, 500, 50, 50);

            
            
            CurrentBackgroundC = Color.Black;

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
            font1 = this.Content.Load<SpriteFont>("SpriteFont1");
            bigFont = this.Content.Load<SpriteFont>("SpriteFont2");

            firstMap = new Map(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, allPurposeTexture, "Content/MazeGameMap.txt");
            p1 = new Player(true, new Rectangle(80, 80, 20, 20), allPurposeTexture);
            p2 = new Player(false, new Rectangle(140, 80, 20, 20), allPurposeTexture);
            p1Border = new Rectangle(p1.pRect.X + 2, p1.pRect.Y + 2, p1.pRect.Width - 4, p1.pRect.Height - 4);
            p2Border = new Rectangle(p2.pRect.X + 2, p2.pRect.Y + 2, p2.pRect.Width - 4, p2.pRect.Height - 4);

            CurrentMap = firstMap;

            menus = new Menus(gameState, instructionState, GraphicsDevice, stringScale, ScreenWidth, ScreenHeight, bigFont, font1, selecterArrowTexture);
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

            menus.Navigations(gameState, instructionState, kb, CurrentBackgroundC, oldKb);
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
            menus.drawMenus(spriteBatch, gameState, instructionState);

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
