using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Terrain : Pivot
    {

        int offsetY;
        int tileSize;

        int displayWidth;
        int displayHeight;

        public int[,] terrainData;
        public int[,] terrainRotations;

        public AnimationSprite[,] tiles;

        public Terrain(int mapWidth, int mapHeight, int screenWidth, int screenHeight, int tileSize, int offset)
        {
            terrainData = new int[mapWidth, mapHeight];
            terrainRotations = new int[mapWidth, mapHeight]; 
            tiles = new AnimationSprite[mapWidth, mapHeight];

            displayWidth = screenWidth / tileSize + 1;
            displayHeight = screenHeight / tileSize + 2;

            this.tileSize = tileSize;

            for (int x = 0; x < displayWidth; x++)
            {
                for (int y = 0; y < displayHeight; y++)
                {
                    tiles[x, y] = new AnimationSprite("tiles.png", 8, 1);
                    tiles[x, y].SetOrigin(tiles[x, y].width / 2, tiles[x, y].height / 2);
                    tiles[x, y].SetXY((x + 0.5f) * tileSize, offset + (y + 0.5f) * tileSize);
                    AddChild(tiles[x, y]);
                }

                for (int y = 0; y < mapHeight; y++)
                {
                    terrainData[x, y] = Utils.Random(0, 4);

                    terrainData[x, y] = Convert.ToInt32(Mathf.Clamp(((y * 4f / mapHeight) + Utils.Random(-0.2f, 0.2f)), 0, 3));

                    //if(Utils.Random(0,25 - (y / 10)) == 0)
                    //{
                    //    terrainData[x, y] += 4;
                    //}

                    if (Utils.Random(0, Convert.ToInt32(15 - (3 / 2) * Math.Sqrt(y))) == 1)
                    {
                        terrainData[x, y] += 4;
                    }

                    terrainRotations[x, y] = Utils.Random(0, 4);

                }
            }



            //for (int x = 0; x < mapWidth; x++)
            //{
            //    for (int y = 0; y < mapHeight; y++)
            //    {
            //        terrainData[x, y] = Utils.Random(0, 4);
            //        tiles[x, y] = new AnimationSprite("tiles.png", 4, 1);
            //        tiles[x, y].SetXY(x * tileSize, offset + y * tileSize);
            //        tiles[x, y].currentFrame = terrainData[x, y];
            //        AddChild(tiles[x, y]);
            //    }
            //}

        }

        public void UpdateTerrain(float camY, bool forceUpdate)
        {
            int oldOffsetY = offsetY;
            offsetY = Convert.ToInt32(-camY / tileSize);

            //y = Mathf.Clamp((offsetY * tileSize), -20,  0);
            y = offsetY * tileSize;

            Console.WriteLine(offsetY);

            if (offsetY != oldOffsetY || forceUpdate)
            {
                for (int x = 0; x < displayWidth; x++)
                {
                    for (int y = 0; y < displayHeight; y++)
                    {
                        if (y + offsetY >= 0)
                        {
                            if (terrainData[x, y + offsetY] == -1)
                            {
                                tiles[x, y].visible = false;
                                tiles[x, y].collider.isTrigger = true;
                            }
                            else
                            {
                                tiles[x, y].visible = true;
                                tiles[x, y].currentFrame = terrainData[x, y + offsetY];
                                tiles[x, y].rotation = 90 * terrainRotations[x, y + offsetY];
                                tiles[x, y].collider.isTrigger = false;
                            }
                        }
                        else
                        {
                            tiles[x, y].visible = false;

                            tiles[x, y].collider.isTrigger = true;
                        }
                    }
                }
            }
        }





    }
}