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
    class Map
    {
        // 2d array of all the tiles.
        public Tile[,] tileMap;
        public Vector2 mapSize;
        public Color FLOORCOLOR;
        int screenW;
        int screenH;
        Texture2D allPurposeTexture;

        public Map(int ScreenWidth, int ScreenHeight, Texture2D allPurposeTexture, String MapName)
        {
            // Add aspect Ratio??
            this.screenH = ScreenHeight;
            this.screenW = ScreenWidth;
            this.allPurposeTexture = allPurposeTexture;
            // Made to get a name for the text file and make it as a Tilemap
            String MapStageFileName = MapName;
            FLOORCOLOR = Color.Purple;

            mapSize = MakeTileMapArray(MapStageFileName);

            tileMap = new Tile[(int)mapSize.X, (int)mapSize.Y];

            MakeFileMazeMap(MapStageFileName);
        }

        //Declares tileMap [,]
        private Vector2 MakeTileMapArray(String MapStageFileName)
        {
            StreamReader reader = new StreamReader(@"" + MapStageFileName);

            int row = 0;
            int column = 0;

            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();
                row = s.Length;
                column++;
            }

            return new Vector2(row, column);

        }

        //Take map from text file and produces it
        private void MakeFileMazeMap(String MapStageFileName)
        {
            StreamReader reader = new StreamReader(@"" + MapStageFileName);

            int row = 0;
            int column = 0;

            while (!reader.EndOfStream)
            {
                string s = reader.ReadLine();

                char[] c = s.ToCharArray();

                for (int i = 0; i < c.Length; i++)
                {
                    tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y),
                                                              screenW / (int)mapSize.X, screenH / (int)mapSize.Y), allPurposeTexture, Color.Wheat, TileType.Floor, 0);
                }

                for (int i = 0; i < c.Length; i++)
                { // Use switch case
                    if (c[i].Equals('1')) // wall
                    {
                        AddTile(row, column, Color.Black, TileType.Wall);
                    }
                    else if (c[i].Equals('D')) // door
                    {
                        AddTile(row, column, Color.Yellow, TileType.Door);
                    }
                    else // nothing floor
                    {
                        AddTile(row, column, FLOORCOLOR, TileType.Floor);
                    }
                    row++;
                }
                column++;
                row = 0;
            }

        }

        // add tile to tilemap
        private void AddTile(int row, int column, Color color, TileType type)
        {
            switch (type)
            {
                case TileType.Wall:
                    tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y),
                                                              screenW / (int)mapSize.X, screenH / (int)mapSize.Y), allPurposeTexture, color, type, 1);
                    break;
                case TileType.Door:
                    // Vertical doors, let you go left to right and vice versa.
                    if (tileMap[row - 1,column].Tiletype == TileType.Floor )
                    {
                        int tilewidth = (int)((screenW / (int)mapSize.X) * 0.3);
                        tileMap[row, column] = new Tile(new Rectangle((int)(row * (screenW / (int)mapSize.X)) + (screenW / (int)mapSize.X) / 2 - tilewidth / 2, column * (screenH / (int)mapSize.Y),
                                                                      tilewidth, screenH / (int)mapSize.Y),
                                                                  allPurposeTexture, color, type, 1);
                    }
                    // Horizontal doors, up to down vice versa
                    else
                    {
                        int tileHeight = (int)((screenH / (int)mapSize.Y) * 0.3);
                        tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y) + (screenH / (int)mapSize.Y) / 2 - tileHeight / 2,
                                                                  screenW / (int)mapSize.X, tileHeight), allPurposeTexture, color, type, 1);
                    }
                    break;
                // more cases 


                // floor is default
                default:
                    tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y),
                                                              screenW / (int)mapSize.X, screenH / (int)mapSize.Y), allPurposeTexture, color, type, 1);
                    break;

            }
        }
        
        // Need to return doors as well
        public void MoveDoors(int row, int column, int timer)
        {
            //row --------
            //column |||||||
            // if leftTile is a floor then you move the door to the tile.
            if (tileMap[row - 1,column].Tiletype == TileType.Wall)// FLOOR
                 //LEFT OFF HERE WORKS WITH 
            {
                // Do I have to animate the doors for movement?
                tileMap[row, column].TileRect.X = tileMap[row - 1, column].TileRect.X;
                tileMap[row, column].TileDrawLayer = 2; // 2 is hidden layer.
            }
            //to move doors right
            else if (tileMap[row + 1 , column].Tiletype == TileType.Wall)
            {
                tileMap[row, column].TileRect.X = tileMap[row + 1, column].TileRect.X;
                tileMap[row, column].TileDrawLayer = 2;
            }
            // to move doors up
            else if (tileMap[row, column - 1].Tiletype == TileType.Wall)
            {
                tileMap[row, column].TileRect.Y = tileMap[row, column - 1].TileRect.Y;
                tileMap[row, column].TileDrawLayer = 2;
            }
            // to move doors down
            else if (tileMap[row, column + 1].Tiletype == TileType.Wall)
            {
                tileMap[row, column].TileRect.Y = tileMap[row, column + 1].TileRect.Y;
                tileMap[row, column].TileDrawLayer = 2;
            }
            // Need one if door is only between doors. 
            else
            {

            }
        }

        // Player interaction 
        public void MapPlayerCollisions(Player player)
        {
            for (int row = 0; row < tileMap.GetLength(0); row++)
            {
                for (int column = 0; column < tileMap.GetLength(1); column++)
                {
                    if (player.pRect.Intersects(tileMap[row, column].TileRect))
                    {
                        Rectangle CurrentTileRect = tileMap[row, column].TileRect;

                        //interaction with tile types
                        if (tileMap[row,column].Tiletype == TileType.Wall)
                        {
                            //Right check
                            if (player.pRect.X <= CurrentTileRect.X + CurrentTileRect.Width &&
                                player.pRect.X >= CurrentTileRect.X + CurrentTileRect.Width * 0.1)
                            {
                                player.pRect.X += player.speed;// should be move amount
                            }
                            // Left
                            else if (player.pRect.X + player.pRect.Width >= CurrentTileRect.X &&
                                     player.pRect.X + player.pRect.Width <= CurrentTileRect.X + CurrentTileRect.Width * 0.1)
                            {
                                player.pRect.X -= player.speed;// should be move amount
                            }
                            // Top
                            else if (player.pRect.Y + player.pRect.Height >= CurrentTileRect.Y &&
                                player.pRect.Y < CurrentTileRect.Y + CurrentTileRect.Height * 0.1)
                            {
                                player.pRect.Y += player.speed;// should be move amount
                            }
                            /*// Bottom
                            else if (player.pRect.Y  <= CurrentTileRect.Y + CurrentTileRect.Height &&
                                     player.pRect.Y > CurrentTileRect.Y + CurrentTileRect.Height  * 0.1)
                            {
                                player.pRect.Y -= player.speed;// should be move amount
                            }*/
                        }
                        else
                        {

                        }
                      }                     
                   }
               }

         }

        // Method to draw map on screen
        public void MapDraw(GameState gameState, SpriteBatch spriteBatch)
        {
            if (gameState == GameState.Game)
            {
                for (int row = 0; row < this.tileMap.GetLength(0); row++)
                {
                    for (int column = 0; column < this.tileMap.GetLength(1); column++)
                    {
                        if (this.tileMap[row, column] != null)
                            spriteBatch.Draw(this.tileMap[row, column].TileTexture, this.tileMap[row, column].TileRect, null, this.tileMap[row, column].TileColor, 0,
                                             Vector2.Zero, SpriteEffects.None, this.tileMap[row,column].TileDrawLayer);
                    }
                }
            }

        }
        


    }
}
