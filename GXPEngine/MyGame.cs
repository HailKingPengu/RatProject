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

    UIHandler uiHandler;
    Terrain terrain;

    int tileSize = 64;

    int offsetY = -32;

    float gravity = 0.03f;
    float movementForce = 0.06f;
    float jumpForce = 10f;

    public float screenShake;
    float screenShakeFalloff = 0.7f;

    public float camX = 0;
    public float camY;

    int camOffset;

    public Digging() : base(1000, 800, false)
    {     // Create a window that's 800x600 and NOT fullscreen
        player1 = new Player("Player1.jpg", 1.02f, 1.1f, tileSize, offsetY, this);
        AddChild(player1);
        player1.setMovementValues(gravity, movementForce, jumpForce);

        player2 = new Player("Player2.jpg", 1.02f, 1.1f, tileSize, offsetY, this);
        AddChild(player2);
        player2.setMovementValues(gravity, movementForce, jumpForce);

        mapWidth = width / tileSize + 1;
        mapHeight = 200;

        terrain = new Terrain(mapWidth, mapHeight, width, height, tileSize, offsetY);
        AddChild(terrain);

        uiHandler = new UIHandler(player1, player2);
        AddChild(uiHandler);

        Console.WriteLine("MyGame initialized");
    }

    // For every game object, Update is called every frame, by the engine:
    void Update()
    {
        camY += camSmooth * (((-player1.y + height / 2)+(-player2.y + height / 2)) / 2 - camY);

        player1.UpdateMovement(87, 65, 83, 68, 69);
        player2.UpdateMovement(73, 74, 75, 76, 85);

        terrain.UpdateTerrain(camY, false);

        SetXY(camX, camY);
        uiHandler.SetXY(camX, -camY);

        Console.WriteLine(camX);

        UpdateScreenShake();

        //if (Input.GetMouseButtonDown(0))
        //{
        //    screenShake = 20;
        //}

        //simulated lag
        //for(int i = 0; i < 5000; i++) 
        //{
        //    Console.WriteLine("LAG!");
        //}
    }

    public void DigTile(int x, int y)
    {
        if (x > -1 && x < mapWidth && y > -1 && y < mapHeight)
        {
            if (terrain.terrainData[x, y] > -1 && terrain.terrainData[x, y] < 4)
            {
                terrain.terrainData[x, y] = -1;
                terrain.UpdateTerrain(camY, true);
            }
        }
    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new Digging().Start();
    }

    void UpdateScreenShake()
    {
        if (screenShake > 1)
        {
            screenShake *= screenShakeFalloff * (1 / Time.deltaTime);
            y += Utils.Random(-screenShake, screenShake);
            x += Utils.Random(-screenShake, screenShake);
        }
    }
}
