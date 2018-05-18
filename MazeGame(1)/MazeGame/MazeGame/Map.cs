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
        private Color InvisibleColor;
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

            // Colors of the Floor and Invisible Color
            FLOORCOLOR = Color.Purple;
            InvisibleColor = new Color();
            InvisibleColor.A = 0;

            // Makes The map and  stores values to the Tile Places
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

                // Makes tiles so that null is not seen
                for (int i = 0; i < c.Length; i++)
                {
                    tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y),
                                                              screenW / (int)mapSize.X, screenH / (int)mapSize.Y), allPurposeTexture, Color.Wheat, TileType.Floor, 0);
                }

                // ADDS Different types of Tiles
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
                    else if (c[i].Equals('C'))
                    {
                        AddTile(row, column, Color.Gray, TileType.ChanceTile);
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

            makeMapMutations();

        }

        // add Different types of tiles to tilemap
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
                    if (tileMap[row - 1, column].Tiletype == TileType.Floor || tileMap[row - 1, column].Tiletype == TileType.AdjacentDoorTiles)
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
                // increase/decrease speed block 
                case TileType.ChanceTile:
                    tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y),
                                                              screenW / (int)mapSize.X, screenH / (int)mapSize.Y), allPurposeTexture, color, type, 1);
                    break;
                // floor is default
                default:
                    tileMap[row, column] = new Tile(new Rectangle(row * (screenW / (int)mapSize.X), column * (screenH / (int)mapSize.Y),
                                                              screenW / (int)mapSize.X, screenH / (int)mapSize.Y), allPurposeTexture, color, type, 1);
                    break;

            }
        }

        // played after basic map elements are set in to change them in the TileMap 2D array such as tile dimensions
        private void makeMapMutations()
        {
            for (int row = 0; row < tileMap.GetLength(0); row++)
            {
                for (int column = 0; column < tileMap.GetLength(1); column++)
                {
                    // Adds door adjacent tiles for palyer to interact with and open the door.
                    if (tileMap[row, column].Tiletype == TileType.Door)
                    {
                        if (tileMap[row - 1, column].Tiletype == TileType.Floor || tileMap[row - 1, column].Tiletype == TileType.AdjacentDoorTiles)
                        {
                            tileMap[row - 1, column].Tiletype = TileType.AdjacentDoorTiles;
                            tileMap[row + 1, column].Tiletype = TileType.AdjacentDoorTiles;
                        }
                        else
                        {
                            tileMap[row, column - 1].Tiletype = TileType.AdjacentDoorTiles;
                            tileMap[row, column + 1].Tiletype = TileType.AdjacentDoorTiles;
                        }
                    }
                }
            }
        }

        // Player interaction with the objects / tiles on the map 
        public void MapPlayerCollisions(Player player)
        {
            for (int row = 0; row < tileMap.GetLength(0); row++)
            {
                for (int column = 0; column < tileMap.GetLength(1); column++)
                {
                    // Player interaction with the map.
                    if (player.pRect.Intersects(tileMap[row, column].TileRect))
                    {
                        Rectangle CurrentTileRect = tileMap[row, column].TileRect;
                        if (tileMap[row, column].Tiletype == TileType.ChanceTile)
                        {
                            int rand = player.r.Next(0, 100);
                            if (rand < 50 && player.speedMod == PlayerSpeedState.Normal)
                            {
                                player.speedMod = PlayerSpeedState.Slow;
                                player.effectTime = 300;
                            }
                            else if (player.speedMod == PlayerSpeedState.Normal)
                            {
                                player.speedMod = PlayerSpeedState.Fast;
                                player.effectTime = 300;
                            }
                        }
                        //interaction with tile types
                        if (tileMap[row, column].Tiletype == TileType.Wall) // Wall
                        {
                            player.pRect.X = (int)player.previousPos.X;
                            player.pRect.Y = (int)player.previousPos.Y;
                        }
                        if (tileMap[row, column].Tiletype == TileType.Door && tileMap[row, column].TileColor != InvisibleColor) // Door
                        {
                            player.pRect.X = (int)player.previousPos.X;
                            player.pRect.Y = (int)player.previousPos.Y;
                        }
                        if (tileMap[row, column].Tiletype == TileType.AdjacentDoorTiles || tileMap[row, column].Tiletype == TileType.Door) // Tiles next to Door
                        {
                            // Upper tile is door
                            if (tileMap[row - 1, column].Tiletype == TileType.Door)
                            {
                                this.MoveDoors(row - 1, column, tileMap[row - 1, column].TileTimer);
                            }
                            // Lower tile is door
                            if (tileMap[row + 1, column].Tiletype == TileType.Door)
                            {
                                this.MoveDoors(row + 1, column, tileMap[row + 1, column].TileTimer);
                            }
                            if (tileMap[row, column].Tiletype == TileType.Door)
                            {
                                this.MoveDoors(row, column, tileMap[row, column].TileTimer);
                            }
                            // Left tile is door
                            if (tileMap[row, column - 1].Tiletype == TileType.Door)
                            {
                                this.MoveDoors(row, column - 1, tileMap[row, column - 1].TileTimer);
                            }
                            // Right tile is door
                            if (tileMap[row, column + 1].Tiletype == TileType.Door)
                            {
                                this.MoveDoors(row, column + 1, tileMap[row, column + 1].TileTimer);
                            }
                        }

                    }
                    // Do Map Updates for any tile type
                    // Door Updates called
                    if (tileMap[row, column].Tiletype == TileType.Door)
                    {
                        UpdateDoors(row, column);
                    }
                }
            }
        }

        // Updates Doors when interacted with
        private void UpdateDoors(int row, int column)
        {
            // done thorugh giving invisible color to the draw method and making it invisible
            Tile Door = tileMap[row, column];

            if (Door.TileTimer > 1 && Door.TileTimer < 240) // increments timer (Need to normalize it to all fps's
            {
                Door.TileTimer -= 1;
            }
            else if (Door.TileTimer == 1) // Resets timer
            {
                Door.TileTimer = 240;
            }
            if (Door.TileTimer == 120) //  Bring door back
            {
                Door.TileColor = Color.Yellow;
            }
        }

        // Need to return doors as well: changes from invisible to corporeal
        public void MoveDoors(int row, int column, int doorTimer)
        {
            if (doorTimer == 240)
            {
                tileMap[row, column].TileTimer -= 1;
                tileMap[row, column].TileColor = InvisibleColor;
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
                                             Vector2.Zero, SpriteEffects.None, this.tileMap[row, column].TileDrawLayer);
                    }
                }
            }


        }

    }
}
