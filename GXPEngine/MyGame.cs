using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions

public class Digging : Game
{

    EasyDraw[,] tiles;
    int mapWidth;
    int mapHeight;

    Player player;
    Player player2;

    int tileSize = 80;

    int offsetY = 400;

    float gravity = 0.03f;
    float movementForce = 0.08f;
    float jumpForce = 10f;

    int camOffset;

    public Digging() : base(1000, 800, false)
    {     // Create a window that's 800x600 and NOT fullscreen


        player = new Player("background.jpg", 1.02f, 1.1f, tileSize, offsetY);
        AddChild(player);


        tiles = new EasyDraw[width / tileSize, height / tileSize];
        mapWidth = width / tileSize;
        mapHeight = height / tileSize;

        for (int x = 0; x < width / tileSize; x++)
        {
            for (int y = 0; y < height / tileSize; y++)
            {
                tiles[x,y] = new EasyDraw(tileSize, tileSize);
                tiles[x,y].Clear((80 - y * 2), (45 - y * 2), 0);
                tiles[x, y].SetXY(x * tileSize, offsetY + y * tileSize);

            }
        }

        foreach(EasyDraw tile in tiles)
        {
            tile.Stroke(Color.White);
            AddChild(tile);
        }

        Console.WriteLine("MyGame initialized");
    }

    // For every game object, Update is called every frame, by the engine:
    void Update()
    {
        UpdateMovement();

    }

    void UpdateMovement()
    {
        if (!player.grounded)
        {
            player.velocityY += gravity * Time.deltaTime;
        }
        else
        {
            player.velocityY = 0;
        }

        //if (player.GetCollisions().Length != 0)
        //{
        //    player.velocityY = 0;
        //}

        //a
        if (Input.GetKey(Key.D)) 
        {
            player.velocityX += movementForce * Time.deltaTime;

            if (Input.GetKeyDown(Key.ENTER))
            {
                if (player.tileX + 1 > -1 && player.tileX + 1 < mapWidth && player.tileY > -1 && player.tileY < mapHeight)
                {
                    if (tiles[player.tileX + 1, player.tileY] != null)
                    {
                        tiles[player.tileX + 1, player.tileY].Destroy();
                    }
                }
            }
        }
        //d
        if (Input.GetKey(Key.A)) 
        {
            player.velocityX -= movementForce * Time.deltaTime;

            if (Input.GetKeyDown(Key.ENTER))
            {
                if (player.tileX - 1 > -1 && player.tileX - 1 < mapWidth && player.tileY > -1 && player.tileY < mapHeight)
                {
                    if (tiles[player.tileX - 1, player.tileY] != null)
                    {
                        tiles[player.tileX - 1, player.tileY].Destroy();
                    }
                }
            }
        }
        //w
        if (Input.GetKeyDown(Key.W) && player.grounded)
        {
            player.y -= 10;
            player.velocityY -= jumpForce;
            player.grounded = false;
        }

        if (Input.GetKey(Key.W) && Input.GetKeyDown(Key.ENTER))
        {
            if (player.tileX > -1 && player.tileX < mapWidth && player.tileY - 1 > -1 && player.tileY - 1 < mapHeight)
            {
                if (tiles[player.tileX, player.tileY - 1] != null)
                {
                    tiles[player.tileX, player.tileY - 1].Destroy();
                }
            }
        }

        if (Input.GetKey(Key.S) && Input.GetKeyDown(Key.ENTER))
        {
            if (player.tileX > -1 && player.tileX < mapWidth && player.tileY + 1 > -1 && player.tileY + 1 < mapHeight)
            {
                if (tiles[player.tileX, player.tileY + 1] != null)
                {
                    tiles[player.tileX, player.tileY + 1].Destroy();
                }
            }
        }

    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new Digging().Start();
    }
}
