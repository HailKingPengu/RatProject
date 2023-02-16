using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using TiledMapParser;
using System.Xml;
using GXPEngine.Particles;

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
    float screenShakeFalloff = 0.97f;

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

        player1.UpdateInput(87, 65, 83, 68, 69, 81);
        player2.UpdateInput(73, 74, 75, 76, 85, 79);

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

                ParticleSystem particleSystem = new ParticleSystem(50, 30, 500, "DirtParticle.png", 1);
                particleSystem.opacitySettings(0.8f, 0.0f);
                particleSystem.setScale(0.3f, 0.6f);
                particleSystem.setforces(3, 3, 0, 0.1f, 0.98f);
                AddChild(particleSystem);
                particleSystem.SetXY(x * tileSize, y * tileSize);
            }
        }
    }

    public void ExplodeTile(int x, int y)
    {
        for (int ix = x - 1; ix <= x + 1; ix++) 
        {
            for (int iy = y - 1; iy <= y + 1; iy++)
            {
                if(ix >= 0 && ix < mapWidth && iy >= 0 && iy < mapHeight)
                {
                    terrain.terrainData[ix, iy] = -1;
                }
            }
        }

        ParticleSystem particleSystem = new ParticleSystem(200, 10, 1000, "DirtParticle.png", 1);
        particleSystem.opacitySettings(0.8f, 0.0f);
        particleSystem.setScale(0.3f, 0.6f);
        particleSystem.setforces(10, 10, 0, 0.1f, 0.98f);
        AddChild(particleSystem);
        particleSystem.SetXY(x * tileSize, y * tileSize);

        for (int i = 0; i < Utils.Random(4, 12); i++)
        {
            ParticleSystem particleSystem2 = new ParticleSystem(40, 50, 2000, "DirtParticle.png", 1);
            particleSystem2.opacitySettings(0.3f, 0.0f);
            particleSystem2.setScale(0.1f, 0.9f);
            particleSystem2.setforcesSync(12, 12, 0.4f, 0, 0.1f, 0.99f);
            AddChild(particleSystem2);
            particleSystem2.SetXY(x * tileSize, y * tileSize);
        }

        ParticleSystem particleSystem3 = new ParticleSystem(4, 10, 100, "ExplosionParticle.png", 1);
        particleSystem3.opacitySettings(1, 0.0f);
        particleSystem3.setScale(3, 1);
        particleSystem3.setforces(100, 100, 0, 0, 0.4f);
        AddChild(particleSystem3);
        particleSystem3.SetXY(x * tileSize, y * tileSize);

        screenShake = 30;
        terrain.UpdateTerrain(camY, true);

        //if (x > -1 && x < mapWidth && y > -1 && y < mapHeight)
        //{
        //    if (terrain.terrainData[x, y] > -1 && terrain.terrainData[x, y] < 4)
        //    {
        //        terrain.terrainData[x, y] = -1;
        //        terrain.UpdateTerrain(camY, true);
        //    }
        //}
    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new Digging().Start();
    }

    void UpdateScreenShake()
    {
        if (screenShake > 1)
        {
            screenShake *= screenShakeFalloff;// * (1 / Time.deltaTime);
            y += Utils.Random(-screenShake, screenShake);
            x += Utils.Random(-screenShake, screenShake);
        }
    }
}
