using GXPEngine.Particles;
using System;
using System.Collections.Generic;
using System.Drawing;
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

        public Camera p2Camera;

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

        Sprite depthMeter;
        Sprite depthMeter2;

        Sprite p1DepthIndicator;
        Sprite p2DepthIndicator;

        Sprite bombIcon1;
        Sprite bombIcon2;

        EasyDraw bombCount1;
        EasyDraw bombCount2;

        string gameFont = "Machine Gunk.otf";

        AnimationSprite startNumbers;
        AnimationSprite startNumbers2;

        Sprite background1;
        Sprite background2;
        Sprite background3;
        Sprite background4;


        Sound digSound;
        Sound stoneSound;
        Sound explosionSound;
        Sound failSound;


        public float[] volumes = new float[] {0.5f, 0.5f};

        float MusicVolume = 0.5f;


        public bool paused = false;
        bool starting = false;

        int startCounter;

        Digging main;

        public GameInstance(int screenWidth, int screenHeight, string backgroundImage, Digging main) 
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

            player1 = new Player(0, "Zoolander-SheetSML.png", 1.02f, 1.1f, tileSize, 0, offsetY, this, screenWidth/2);
            player1.SetXY(screenWidth / 4, -100);
            player1.setMovementValues(gravity, movementForce, jumpForce);

            player2 = new Player(1, "molesprite-SheetSML.png", 1.02f, 1.1f, tileSize, 800,  offsetY, this, screenWidth/2);
            player2.SetXY(126 + 3 * screenWidth / 4, -100);
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
            AddChildAt(lightingOverlay1, 101);
            lightingOverlay2 = new LightingOverlay(screenWidth / 2, screenHeight);
            AddChildAt(lightingOverlay2, 101);

            lightingOverlay1.AddLightSource(player1, 4);
            lightingOverlay2.AddLightSource(player2, 4);
            lightingOverlay1.AddLightSource(player1, 12);
            lightingOverlay2.AddLightSource(player2, 12);

            depthMeter = new Sprite("DepthMeter.png", false, false);
            AddChild(depthMeter);
            depthMeter2 = new Sprite("DepthMeter.png", false, false);
            depthMeter2.x = 1492 - depthMeter2.width;
            AddChild(depthMeter2);

            p1DepthIndicator = new Sprite("P1DepthIndicator.png", false, false);
            p1DepthIndicator.SetOrigin(p1DepthIndicator.width/2, p1DepthIndicator.height/2);
            p1DepthIndicator.x = screenWidth / 2;
            AddChild(p1DepthIndicator);

            p2DepthIndicator = new Sprite("P2DepthIndicator.png", false, false);
            p2DepthIndicator.SetOrigin(p2DepthIndicator.width / 2, p2DepthIndicator.height / 2);
            p2DepthIndicator.x = 1150 - screenWidth / 4;
            AddChild(p2DepthIndicator);

            uiHandler = new UIHandler(player1, player2);
            AddChild(uiHandler);

            bombIcon1 = new Sprite("tnt.png", false, false);
            AddChild(bombIcon1);
            bombIcon2 = new Sprite("tnt.png", false, false);
            AddChild(bombIcon2);

            bombCount1 = new EasyDraw(300, 100, false);
            bombCount1.TextFont(Utils.LoadFont(gameFont, 40));
            bombCount1.TextAlign(CenterMode.Min, CenterMode.Center);
            AddChild(bombCount1);
            bombCount2 = new EasyDraw(300, 100, false);
            bombCount2.TextFont(Utils.LoadFont(gameFont, 40));
            bombCount2.TextAlign(CenterMode.Max, CenterMode.Center);
            AddChild(bombCount2);

            digSound = new Sound("dig.ogg");
            stoneSound = new Sound("stoneDig.ogg");
            explosionSound = new Sound("explosion.ogg");
            failSound = new Sound("fail.ogg");

            startNumbers = new AnimationSprite("StartNumbers.png", 4, 1);
            startNumbers.x = -screenWidth / 4;
            startNumbers2 = new AnimationSprite("StartNumbers.png", 4, 1);
            startNumbers2.x = 126 + screenWidth / 4;

            this.main = main;

            Console.WriteLine("MyGame initialized");
        }

        void Restart()
        {

        }

        public void StartGame()
        {
            paused = false;
            Update();
            paused = true;

            starting = true;

            AddChild(startNumbers);
            AddChild(startNumbers2);
        }

        void Update()
        {
            if (starting)
            {
                startCounter += Time.deltaTime;

                startNumbers.currentFrame = (int)startCounter / 1000;
                startNumbers2.currentFrame = (int)startCounter / 1000;

                if ((int)startCounter / 1000 > 3)
                {
                    RemoveChild(startNumbers);
                    RemoveChild(startNumbers2);
                    starting = false;
                    paused = false;
                }
            }

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


                //BAZINGA

                depthMeter.SetXY(-x, -y);
                depthMeter2.SetXY(p2Camera.x - screenWidth * 3 / 4, p2Camera.y- screenHeight/2);

                p1DepthIndicator.x = -x + screenWidth / 2;
                p2DepthIndicator.x = p2Camera.x - screenWidth / 4;

                p1DepthIndicator.y = -y + 30 + ((player1.y / tileSize) / 200 * 620);
                p2DepthIndicator.y = p2Camera.y - screenHeight/2 + 30 + ((player2.y / tileSize) / 200 * 620);


                bombIcon1.SetXY(-x + 30, -y + 30);
                bombIcon2.SetXY(p2Camera.x + screenWidth / 4 - 64 - 30, p2Camera.y - screenHeight / 2 + 30);

                bombCount1.SetXY(-x + 100, -y + 20);
                bombCount2.SetXY(p2Camera.x + screenWidth / 4 - 64 - 340, p2Camera.y - screenHeight / 2 + 20);

                bombCount1.ClearTransparent();
                bombCount1.Text(player1.bombs.ToString());
                bombCount2.ClearTransparent();
                bombCount2.Text(player2.bombs.ToString());


                if (Input.GetKeyDown(Key.K))
                {
                    RemoveChild(p2Camera);
                    main.PauseGame();
                }

                if (player1.y > 190 * tileSize)
                //if (player1.y > 20 * tileSize)
                {
                    RemoveChild(p2Camera);
                    main.GameOver(player1.playerID);
                }
                if (player2.y > 190 * tileSize)
                //if (player2.y > 20 * tileSize)
                {
                    RemoveChild(p2Camera);
                    main.GameOver(player2.playerID);
                }
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
                if (x > -1 && x < mapWidth / 2 && y > -1 && y < mapHeight)
                {
                    if (terrain.terrainData[x, y] > -1 && terrain.terrainData[x, y] < 4)
                    {
                        terrain.terrainData[x, y] = -1;
                        terrain.UpdateTerrain(camY1, true);

                        SpawnDigParticles(x * tileSize + tileSize / 2 + offsetX, y * tileSize);
                        digSound.Play(false, 0, volumes[0]);

                        screenShake1 = 1;
                    }
                    else if (terrain.terrainData[x, y] > 7 && terrain.terrainData[x, y] < 12)
                    {
                        terrain.terrainData[x, y] += 4;
                        terrain.UpdateTerrain(camY1, true);

                        SpawnStoneSparkParticles(x* tileSize +tileSize / 2 + offsetX, y* tileSize);
                        digSound.Play(false, 0, volumes[0]);
                        stoneSound.Play(false, 0, volumes[0]);

                        screenShake1 = 5;
                    }
                    else if (terrain.terrainData[x, y] > 11 && terrain.terrainData[x, y] < 16)
                    {
                        terrain.terrainData[x, y] = -1;
                        terrain.UpdateTerrain(camY1, true);

                        SpawnStoneParticles(x * tileSize + tileSize / 2 + offsetX, y * tileSize);
                        digSound.Play(false, 0, volumes[0]);
                        stoneSound.Play(false, 0, volumes[0]);

                        screenShake1 = 3;
                    }
                    else if (terrain.terrainData[x, y] > 3 && terrain.terrainData[x, y] < 8)
                    {
                        failSound.Play(false, 0, volumes[0]);
                    }
                }
            }
            else
            {
                if (x > -1 && x < mapWidth / 2 && y > -1 && y < mapHeight)
                {
                    if (terrain2.terrainData[x, y] > -1 && terrain2.terrainData[x, y] < 4)
                    {
                        terrain2.terrainData[x, y] = -1;
                        terrain2.UpdateTerrain(camY2, true);

                        SpawnDigParticles(x * tileSize + tileSize / 2 + offsetX, y * tileSize);
                        digSound.Play(false, 0, volumes[0]);

                        screenShake2 = 1;
                    }
                    else if (terrain2.terrainData[x, y] > 7 && terrain2.terrainData[x, y] < 12)
                    {
                        terrain2.terrainData[x, y] += 4;
                        terrain2.UpdateTerrain(camY2, true);

                        SpawnStoneSparkParticles(x * tileSize + tileSize / 2 + offsetX, y * tileSize);
                        digSound.Play(false, 0, volumes[0]);
                        stoneSound.Play(false, 0, volumes[0]);

                        screenShake2 = 5;
                    }
                    else if (terrain2.terrainData[x, y] > 11 && terrain2.terrainData[x, y] < 16)
                    {
                        terrain2.terrainData[x, y] = -1;
                        terrain2.UpdateTerrain(camY2, true);

                        SpawnStoneParticles(x * tileSize + tileSize / 2 + offsetX, y * tileSize);
                        digSound.Play(false, 0, volumes[0]);
                        stoneSound.Play(false, 0, volumes[0]);

                        screenShake2 = 3;
                    }
                    else if (terrain2.terrainData[x, y] > 3 && terrain2.terrainData[x, y] < 8)
                    {
                        failSound.Play(false, 0, volumes[0]);
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
                        if (ix >= 0 && ix < mapWidth / 2 && iy >= 0 && iy < mapHeight)
                        {
                            terrain.terrainData[ix, iy] = -1;
                        }
                    }
                }
                explosionSound.Play(false, 0, volumes[0]);
                screenShake1 = 20;
                terrain.UpdateTerrain(camY1, true);
            }
            else
            {
                for (int ix = x - 1; ix <= x + 1; ix++)
                {
                    for (int iy = y - 1; iy <= y + 1; iy++)
                    {
                        if (ix >= 0 && ix < mapWidth / 2 && iy >= 0 && iy < mapHeight)
                        {
                            terrain2.terrainData[ix, iy] = -1;
                        }
                    }
                }
                explosionSound.Play(false, 0, volumes[0]);
                screenShake2 = 20;
                terrain2.UpdateTerrain(camY2, true);
            }

            SpawnExplosionParticles(x * tileSize + tileSize/2 + offsetX, y * tileSize - tileSize / 2);

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
            AddChildAt(particleSystem, 9);
            particleSystem.SetXY(x, y);
        }

        void SpawnStoneSparkParticles(int x, int y)
        {
            ParticleSystem particleSystem5 = new ParticleSystem(15, 40, 2000, "circle2.png", 1);
            particleSystem5.colorSettings(1, 0.0f);
            particleSystem5.setScale(0.1f, 0.2f);
            particleSystem5.setforces(6, 6, 0, 0.1f, 0.99f);
            particleSystem5.setBlendMode(BlendMode.LIGHTING);
            AddChildAt(particleSystem5, 9);
            particleSystem5.SetXY(x, y);

            ParticleSystem particleSystem = new ParticleSystem(8, 40, 1000, "StoneParticle.png", 1);
            particleSystem.opacitySettings(0.8f, 0.0f);
            particleSystem.setScale(0.3f, 0.6f);
            particleSystem.setforces(5, 5, 0, 0.1f, 0.98f);
            AddChildAt(particleSystem, 9);
            particleSystem.SetXY(x, y);
        }

        void SpawnStoneParticles(int x, int y)
        {
            ParticleSystem particleSystem = new ParticleSystem(60, 40, 1000, "StoneParticle.png", 1);
            particleSystem.opacitySettings(0.8f, 0.0f);
            particleSystem.setScale(0.3f, 0.6f);
            particleSystem.setforces(8, 8, 0, 0.1f, 0.98f);
            AddChildAt(particleSystem, 9);
            particleSystem.SetXY(x, y);
        }

        void SpawnExplosionParticles(int x, int y)
        {
            ParticleSystem particleSystem3 = new ParticleSystem(4, 40, 300, "ExplosionParticle.png", 1);
            particleSystem3.opacitySettings(0.2f, 0.0f);
            particleSystem3.setScale(3, 1);
            particleSystem3.setforces(40, 40, 0, 0, 0.6f);
            AddChildAt(particleSystem3, 9);
            particleSystem3.SetXY(x, y);


            ParticleSystem particleSystem = new ParticleSystem(80, 40, 1000, "DirtParticle.png", 1);
            particleSystem.opacitySettings(0.8f, 0.0f);
            particleSystem.setScale(0.3f, 0.6f);
            particleSystem.setforces(10, 10, 0, 0.1f, 0.98f);
            AddChildAt(particleSystem, 9);
            particleSystem.SetXY(x, y);


            ParticleSystem particleSystem5 = new ParticleSystem(20, 40, 2000, "circle2.png", 1);
            particleSystem5.colorSettings(1, 0.0f);
            particleSystem5.setScale(0.2f, 0.3f);
            particleSystem5.setforces(8, 8, 0, 0.1f, 0.99f);
            particleSystem5.setBlendMode(BlendMode.LIGHTING);
            AddChildAt(particleSystem5, 9);
            particleSystem5.SetXY(x, y);


            for (int i = 0; i < Utils.Random(4, 12); i++)
            {
                ParticleSystem particleSystem2 = new ParticleSystem(20, 50, 2000, "DirtParticle.png", 1);
                particleSystem2.opacitySettings(0.3f, 0.0f);
                particleSystem2.setScale(0.1f, 0.9f);
                particleSystem2.setforcesSync(12, 12, 0.4f, 0, 0.1f, 0.99f);
                AddChildAt(particleSystem2, 9);
                particleSystem2.SetXY(x, y);
            }

            ParticleSystem particleSystem4 = new ParticleSystem(6, 40, 200, "circle2.png", 1);
            particleSystem4.colorSettings(1, 0.0f);
            particleSystem4.setScale(12, 2);
            particleSystem4.setforces(90, 90, 0, 0, 0.4f);
            particleSystem4.setBlendMode(BlendMode.LIGHTING);
            AddChildAt(particleSystem4, 9);
            particleSystem4.SetXY(x, y);
        }
    }
}
