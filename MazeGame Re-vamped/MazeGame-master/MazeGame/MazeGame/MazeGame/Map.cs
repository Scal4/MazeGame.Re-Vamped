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
            this.screenH = ScreenHeight;
            this.screenW = ScreenWidth;
            this.allPurposeTexture = allPurposeTexture;
            // Made to get a name for the text file and make it as a Tilemap
            String MapStageFileName = MapName;
            FLOORCOLOR = Color.Purple;

            mapSize = makeTileMapArray(MapStageFileName);

            tileMap = new Tile[(int)mapSize.X, (int)mapSize.Y];

            makeFileMazeMap(MapStageFileName);
        }

        //Declares tileMap [,]
        private Vector2 makeTileMapArray(String MapStageFileName)
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
        private void makeFileMazeMap(String MapStageFileName)
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
                                                              screenW / (int)mapSize.X, screenH / (int)mapSize.Y), allPurposeTexture, Color.Wheat, TileType.Floor);
                }

                for (int i = 0; i < c.Length; i++)
                { // Use switch case
                    if (c[i].Equals('1')) // wall
                    {
                        addTile(row, column, Color.Black, TileType.Wall);
                    }
                    else if (c[i].Equals('D')) // door
                    {
                        addTile(row, column, Color.Yellow, TileType.Door);
                    }
                    else // nothing floor
                    {
                        addTile(row, column, FLOORCOLOR, TileType.Floor);
                    }
                    row++;
                }
                column++;
                row = 0;
            }

        }
        // add tile to tilemap
        private void addTile(int row, int column, Color color, TileType type)
        {
            switch (type)
            {
                case TileType.Wall:
                    tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y),
                                                              screenW / (int)mapSize.X, screenH / (int)mapSize.Y), allPurposeTexture, color, type);
                    break;
                case TileType.Door:
                    // Vertical doors, let you go left to right and vice versa.
                    if (tileMap[row - 1,column].Tiletype == TileType.Floor )
                    {
                        int tilewidth = (int)((screenW / (int)mapSize.X) * 0.3);
                        tileMap[row, column] = new Tile(new Rectangle((int)(row * (screenW / (int)mapSize.X)) + (screenW / (int)mapSize.X) / 2 - tilewidth / 2, column * (screenH / (int)mapSize.Y),
                                                                      tilewidth, screenH / (int)mapSize.Y),
                                                                  allPurposeTexture, color, type);
                    }
                    else
                    {
                        int tileHeight = (int)((screenH / (int)mapSize.Y) * 0.3);
                        tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y) + (screenH / (int)mapSize.Y) / 2 - tileHeight / 2,
                                                                  screenW / (int)mapSize.X, tileHeight), allPurposeTexture, color, type);
                    }
                    break;



                default:
                    tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y),
                                                              screenW / (int)mapSize.X, screenH / (int)mapSize.Y), allPurposeTexture, color, type);
                    break;

            }
        }
        
    }
}
