using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using TiledMapParser;

public class Digging : Game
{

    float camSmooth = 0.02f;

    EasyDraw[,] tiles;
    int mapWidth;
    int mapHeight;

    Player player1;
    Player player2;

    UIHandler uiHandler = new UIHandler();
    Terrain terrain;

    int tileSize = 64;

    int offsetY = -32;

    float gravity = 0.03f;
    float movementForce = 0.08f;
    float jumpForce = 10f;

    float screenShake;
    float screenShakeFalloff;

    int camOffset;

    public Digging() : base(1000, 800, false)
    {     // Create a window that's 800x600 and NOT fullscreen

        player1 = new Player("Player1.jpg", 1.02f, 1.1f, tileSize, offsetY, this);
        AddChild(player1);
        player1.setMovementValues(gravity, movementForce, jumpForce);

        player2 = new Player("Player2.jpg", 1.02f, 1.1f, tileSize, offsetY, this);
        AddChild(player2);
        player2.setMovementValues(gravity, movementForce, jumpForce);


        tiles = new EasyDraw[width / tileSize, height / tileSize];
        mapWidth = width / tileSize + 1;
        mapHeight = 200;

        //for (int x = 0; x < mapWidth; x++)
        //{
        //    for (int y = 0; y < mapHeight; y++)
        //    {
        //        tiles[x,y] = new EasyDraw(tileSize, tileSize);
        //        tiles[x,y].Clear((80 - y * 2), (45 - y * 2), 0);
        //        tiles[x, y].SetXY(x * tileSize, offsetY + y * tileSize);
        //    }
        //}

        terrain = new Terrain(mapWidth, mapHeight, width, height, tileSize, offsetY);
        AddChild(terrain);

        //foreach(EasyDraw tile in tiles)
        //{
        //    tile.Stroke(Color.White);
        //    AddChild(tile);
        //}

        Console.WriteLine("MyGame initialized");
    }

    // For every game object, Update is called every frame, by the engine:
    void Update()
    {
        y += camSmooth * (((-player1.y + height / 2)+(-player2.y + height / 2)) / 2 - y);

        player1.UpdateMovement(87, 65, 83, 68, 69);
        player2.UpdateMovement(73, 74, 75, 76, 85);

        terrain.UpdateTerrain(y, false);

        //UpdateScreenShake();
    }

    public void DigTile(int x, int y)
    {
        if (x > -1 && x < mapWidth && y > -1 && y < mapHeight)
        {
            if (terrain.terrainData[x, y] > -1 && terrain.terrainData[x, y] < 4)
            {
                terrain.terrainData[x, y] = -1;
                terrain.UpdateTerrain(y, true);
            }
        }
    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new Digging().Start();
    }

    void UpdateScreenShake()
    {
        screenShake /= screenShake * screenShakeFalloff;
        y += Utils.Random(-screenShake, screenShake);
        x += Utils.Random(-screenShake, screenShake);
    }
}
