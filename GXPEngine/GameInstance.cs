using GXPEngine.Particles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    internal class GameInstance : Pivot
    {

        int screenWidth;
        int screenHeight;

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


        Sprite background;
        Sprite background2;

        public GameInstance(int screenWidth, int screenHeight, string backgroundImage) 
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            //FIX THIS
            background = new Sprite(backgroundImage, false, false);
            AddChild(background);
            background2 = new Sprite(backgroundImage, false, false);
            AddChild(background2);

            mapWidth = screenWidth / tileSize + 1;
            mapHeight = 200;

            player1 = new Player("Player1.jpg", 1.02f, 1.1f, tileSize, offsetY, this, screenWidth);
            player1.SetXY(200, -100);
            player1.setMovementValues(gravity, movementForce, jumpForce);

            player2 = new Player("Player2.jpg", 1.02f, 1.1f, tileSize, offsetY, this, screenWidth);
            player2.SetXY(800, -100);
            player2.setMovementValues(gravity, movementForce, jumpForce);

            AddChild(player1);
            AddChild(player2);

            terrain = new Terrain(mapWidth, mapHeight, screenWidth, screenHeight, tileSize, offsetY);
            AddChild(terrain);

            uiHandler = new UIHandler(player1, player2);
            AddChild(uiHandler);

            Console.WriteLine("MyGame initialized");
        }

        void Update()
        {
            camY += camSmooth * (((-player1.y + screenHeight / 2) + (-player2.y + screenHeight / 2)) / 2 - camY);

            player1.UpdateInput(87, 65, 83, 68, 69, 81);
            player2.UpdateInput(73, 74, 75, 76, 85, 79);

            terrain.UpdateTerrain(camY, false);

            Console.WriteLine("CAMERA POSITION : " + camX + ", " + camY);

            SetXY(camX, camY);
            uiHandler.SetXY(camX, -camY);

            background.SetXY(camX, -camY);
            background2.SetXY(camX, -camY + background.height);

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
                    if (ix >= 0 && ix < mapWidth && iy >= 0 && iy < mapHeight)
                    {
                        terrain.terrainData[ix, iy] = -1;
                    }
                }
            }

            ParticleSystem particleSystem = new ParticleSystem(80, 10, 1000, "DirtParticle.png", 1);
            particleSystem.opacitySettings(0.8f, 0.0f);
            particleSystem.setScale(0.3f, 0.6f);
            particleSystem.setforces(10, 10, 0, 0.1f, 0.98f);
            AddChild(particleSystem);
            particleSystem.SetXY(x * tileSize, y * tileSize);

            for (int i = 0; i < Utils.Random(4, 12); i++)
            {
                ParticleSystem particleSystem2 = new ParticleSystem(20, 50, 2000, "DirtParticle.png", 1);
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

        void UpdateScreenShake()
        {
            if (screenShake > 1)
            {
                screenShake *= screenShakeFalloff / Mathf.Clamp((Time.deltaTime / 20), 1, 100);
                y += Utils.Random(-screenShake, screenShake);
                x += Utils.Random(-screenShake, screenShake);

                Console.WriteLine(Time.deltaTime);
            }
        }
    }
}
