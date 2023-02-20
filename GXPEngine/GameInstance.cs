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

        Camera p2Camera;

        UIHandler uiHandler;
        Terrain terrain;
        Terrain terrain2;

        int tileSize = 64;

        int offsetY = -32;

        float gravity = 0.03f;
        float movementForce = 0.06f;
        float jumpForce = 10f;

        public float screenShake1;
        public float screenShake2;
        float screenShakeFalloff = 0.97f;

        public float camX = 0;
        public float camY1;
        public float camY2;

        int camOffset;

        LightingOverlay lightingOverlay1;
        LightingOverlay lightingOverlay2;

        Sprite background1;
        Sprite background2;
        Sprite background3;
        Sprite background4;

        bool paused = false;

        public GameInstance(int screenWidth, int screenHeight, string backgroundImage) 
        {
            this.screenWidth = screenWidth;
            this.screenHeight = screenHeight;

            //FIX THIS
            //if time: scrolling background
            background1 = new Sprite(backgroundImage, false, false);
            AddChild(background1);
            background2 = new Sprite(backgroundImage, false, false);
            AddChild(background2);

            background3 = new Sprite(backgroundImage, false, false);
            AddChild(background3);
            background4 = new Sprite(backgroundImage, false, false);
            AddChild(background4);

            mapWidth = screenWidth / tileSize + 1;
            mapHeight = 200;

            player1 = new Player(0, "Player1.jpg", 1.02f, 1.1f, tileSize, 0, offsetY, this, screenWidth);
            player1.SetXY(200, -100);
            player1.setMovementValues(gravity, movementForce, jumpForce);

            player2 = new Player(1, "Player2.jpg", 1.02f, 1.1f, tileSize, 800,  offsetY, this, screenWidth);
            player2.SetXY(800, -100);
            player2.setMovementValues(gravity, movementForce, jumpForce);

            p2Camera = new Camera(screenWidth / 2, 0, screenWidth / 2, screenHeight);
            AddChild(p2Camera);

            AddChild(player1);
            AddChild(player2);

            terrain = new Terrain(mapWidth/2, mapHeight, screenWidth/2, screenHeight, tileSize, 0, offsetY);
            AddChild(terrain);

            terrain2 = new Terrain(mapWidth/2, mapHeight, screenWidth/2, screenHeight, tileSize, 800, offsetY);
            AddChild(terrain2);

            lightingOverlay1 = new LightingOverlay(screenWidth / 2, screenHeight);
            AddChild(lightingOverlay1);
            lightingOverlay2 = new LightingOverlay(screenWidth / 2, screenHeight);
            AddChild(lightingOverlay2);

            lightingOverlay1.AddLightSource(player1, 4);
            lightingOverlay2.AddLightSource(player2, 4);
            lightingOverlay1.AddLightSource(player1, 12);
            lightingOverlay2.AddLightSource(player2, 12);

            uiHandler = new UIHandler(player1, player2);
            AddChild(uiHandler);

            Console.WriteLine("MyGame initialized");
        }

        void Restart()
        {

        }

        void Update()
        {
            if (!paused)
            {
                player1.UpdateGeneral(camY1);
                player2.UpdateGeneral(camY2);

                camY1 += camSmooth * ((-player1.y + screenHeight / 2) - camY1);
                camY2 += camSmooth * ((-player2.y + screenHeight / 2) - camY2);

                //Console.WriteLine(terrain.y);

                player1.UpdateInput(87, 65, 83, 68, 69, 81);
                player2.UpdateInput(73, 74, 75, 76, 85, 79);

                terrain.UpdateTerrain(camY1, false);
                terrain2.UpdateTerrain(camY2, false);



                //Console.WriteLine("CAMERA POSITION : " + camX + ", " + camY);

                SetXY(camX, camY1);
                p2Camera.SetXY(1150, -camY2 + screenHeight/2);
                uiHandler.SetXY(camX, -camY1);

                //hardcoded until i have time to fix this
                background1.SetXY(-500, -camY1);
                background2.SetXY(-500, -camY1 + background1.height);
                background3.SetXY(700, -camY2);
                background4.SetXY(700, -camY2 + background3.height);

                lightingOverlay1.UpdateOverlay(player1.y / tileSize);
                lightingOverlay1.SetXY(camX, -camY1);

                lightingOverlay2.UpdateOverlay(player2.y / tileSize);
                lightingOverlay2.SetXY(1150 - screenWidth / 4, -camY2);

                UpdateScreenShake();
            }



            //if (Input.GetKey(Key.A))
            //{
            //    Console.WriteLine("A PRESSED");
            //    dDelay = 5;
            //}

            //if (dDelay > 0)
            //{
            //    Console.WriteLine("A PRESSED");
            //    dDelay--;
            //}
           

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

        public void DigTile(int x, int y, int player, int offsetX)
        {
            if (player == 0)
            {
                if (x > -1 && x < mapWidth && y > -1 && y < mapHeight)
                {
                    if (terrain.terrainData[x, y] > -1 && terrain.terrainData[x, y] < 4)
                    {
                        terrain.terrainData[x, y] = -1;
                        terrain.UpdateTerrain(camY1, true);

                        SpawnDigParticles(x * tileSize + offsetX, y * tileSize);

                        screenShake1 = 1;
                    }
                    else if (terrain.terrainData[x, y] > 7 && terrain.terrainData[x, y] < 12)
                    {
                        terrain.terrainData[x, y] += 4;
                        terrain.UpdateTerrain(camY1, true);

                        SpawnStoneSparkParticles(x * tileSize + offsetX, y * tileSize);

                        screenShake1 = 5;
                    }
                    else if (terrain.terrainData[x, y] > 11 && terrain.terrainData[x, y] < 16)
                    {
                        terrain.terrainData[x, y] = -1;
                        terrain.UpdateTerrain(camY1, true);

                        SpawnStoneParticles(x * tileSize + offsetX, y * tileSize);

                        screenShake1 = 3;
                    }
                }
            }
            else
            {
                if (x > -1 && x < mapWidth && y > -1 && y < mapHeight)
                {
                    if (terrain2.terrainData[x, y] > -1 && terrain2.terrainData[x, y] < 4)
                    {
                        terrain2.terrainData[x, y] = -1;
                        terrain2.UpdateTerrain(camY2, true);

                        SpawnDigParticles(x * tileSize + offsetX, y * tileSize);

                        screenShake2 = 1;
                    }
                    else if (terrain.terrainData[x, y] > 7 && terrain.terrainData[x, y] < 12)
                    {
                        terrain.terrainData[x, y] += 4;
                        terrain.UpdateTerrain(camY1, true);

                        SpawnStoneSparkParticles(x * tileSize + offsetX, y * tileSize);

                        screenShake2 = 5;
                    }
                    else if (terrain.terrainData[x, y] > 11 && terrain.terrainData[x, y] < 16)
                    {
                        terrain.terrainData[x, y] = -1;
                        terrain.UpdateTerrain(camY1, true);

                        SpawnStoneParticles(x * tileSize + offsetX, y * tileSize);

                        screenShake2 = 3;
                    }
                }
            }
        }

        public void ExplodeTile(int x, int y, int player, int offsetX)
        {
            if (player == 0)
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
                screenShake1 = 20;
                terrain.UpdateTerrain(camY1, true);
            }
            else
            {
                for (int ix = x - 1; ix <= x + 1; ix++)
                {
                    for (int iy = y - 1; iy <= y + 1; iy++)
                    {
                        if (ix >= 0 && ix < mapWidth && iy >= 0 && iy < mapHeight)
                        {
                            terrain2.terrainData[ix, iy] = -1;
                        }
                    }
                }
                screenShake2 = 20;
                terrain2.UpdateTerrain(camY2, true);
            }

            SpawnExplosionParticles(x * tileSize + offsetX, y * tileSize);

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
            if (screenShake1 > 1)
            {
                screenShake1 *= screenShakeFalloff / Mathf.Clamp((Time.deltaTime / 20), 1, 100);
                y += Utils.Random(-screenShake1, screenShake1);
                x += Utils.Random(-screenShake1, screenShake1);

                //Console.WriteLine(Time.deltaTime);
            }

            if (screenShake2 > 1)
            {
                screenShake2 *= screenShakeFalloff / Mathf.Clamp((Time.deltaTime / 20), 1, 100);
                p2Camera.y += Utils.Random(-screenShake2, screenShake2);
                p2Camera.x += Utils.Random(-screenShake2, screenShake2);

                //Console.WriteLine(Time.deltaTime);
            }
        }

        void SpawnDigParticles(int x, int y)
        {
            ParticleSystem particleSystem = new ParticleSystem(50, 30, 500, "DirtParticle.png", 1);
            particleSystem.opacitySettings(0.8f, 0.0f);
            particleSystem.setScale(0.3f, 0.6f);
            particleSystem.setforces(3, 3, 0, 0.1f, 0.98f);
            AddChildAt(particleSystem, 40);
            particleSystem.SetXY(x, y);
        }

        void SpawnStoneSparkParticles(int x, int y)
        {
            ParticleSystem particleSystem5 = new ParticleSystem(15, 40, 2000, "circle2.png", 1);
            particleSystem5.colorSettings(1, 0.0f);
            particleSystem5.setScale(0.1f, 0.2f);
            particleSystem5.setforces(6, 6, 0, 0.1f, 0.99f);
            particleSystem5.setBlendMode(BlendMode.LIGHTING);
            AddChildAt(particleSystem5, 40);
            particleSystem5.SetXY(x, y);

            ParticleSystem particleSystem = new ParticleSystem(8, 40, 1000, "StoneParticle.png", 1);
            particleSystem.opacitySettings(0.8f, 0.0f);
            particleSystem.setScale(0.3f, 0.6f);
            particleSystem.setforces(5, 5, 0, 0.1f, 0.98f);
            AddChildAt(particleSystem, 40);
            particleSystem.SetXY(x, y);
        }

        void SpawnStoneParticles(int x, int y)
        {
            ParticleSystem particleSystem = new ParticleSystem(60, 40, 1000, "StoneParticle.png", 1);
            particleSystem.opacitySettings(0.8f, 0.0f);
            particleSystem.setScale(0.3f, 0.6f);
            particleSystem.setforces(8, 8, 0, 0.1f, 0.98f);
            AddChildAt(particleSystem, 40);
            particleSystem.SetXY(x, y);
        }

        void SpawnExplosionParticles(int x, int y)
        {
            ParticleSystem particleSystem3 = new ParticleSystem(4, 40, 300, "ExplosionParticle.png", 1);
            particleSystem3.opacitySettings(0.2f, 0.0f);
            particleSystem3.setScale(3, 1);
            particleSystem3.setforces(40, 40, 0, 0, 0.6f);
            AddChildAt(particleSystem3, 40);
            particleSystem3.SetXY(x, y);


            ParticleSystem particleSystem = new ParticleSystem(80, 40, 1000, "DirtParticle.png", 1);
            particleSystem.opacitySettings(0.8f, 0.0f);
            particleSystem.setScale(0.3f, 0.6f);
            particleSystem.setforces(10, 10, 0, 0.1f, 0.98f);
            AddChildAt(particleSystem, 40);
            particleSystem.SetXY(x, y);


            ParticleSystem particleSystem5 = new ParticleSystem(20, 40, 2000, "circle2.png", 1);
            particleSystem5.colorSettings(1, 0.0f);
            particleSystem5.setScale(0.2f, 0.3f);
            particleSystem5.setforces(8, 8, 0, 0.1f, 0.99f);
            particleSystem5.setBlendMode(BlendMode.LIGHTING);
            AddChildAt(particleSystem5, 40);
            particleSystem5.SetXY(x, y);


            for (int i = 0; i < Utils.Random(4, 12); i++)
            {
                ParticleSystem particleSystem2 = new ParticleSystem(20, 50, 2000, "DirtParticle.png", 1);
                particleSystem2.opacitySettings(0.3f, 0.0f);
                particleSystem2.setScale(0.1f, 0.9f);
                particleSystem2.setforcesSync(12, 12, 0.4f, 0, 0.1f, 0.99f);
                AddChildAt(particleSystem2, 40);
                particleSystem2.SetXY(x, y);
            }

            ParticleSystem particleSystem4 = new ParticleSystem(6, 40, 200, "circle2.png", 1);
            particleSystem4.colorSettings(1, 0.0f);
            particleSystem4.setScale(12, 2);
            particleSystem4.setforces(90, 90, 0, 0, 0.4f);
            particleSystem4.setBlendMode(BlendMode.LIGHTING);
            AddChildAt(particleSystem4, 40);
            particleSystem4.SetXY(x, y);
        }
    }
}
