using GXPEngine.Core;
using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    internal class Player : AnimationSprite
    {

        GameInstance main;

        int tileSize;
        int offsetX;
        int offsetY;

        public int tileX;
        public int tileY;

        int minX;
        int maxX;

        public EasyDraw groundCheck;
        public EasyDraw ceilingCheck;

        public float velocityX;
        public float velocityY;

        float airFriction;
        float groundFriction;

        float gravity = 0.03f;
        float movementForce = 0.08f;
        float jumpForce = 10f;

        public bool grounded = false;

        public float cooldown = 1;

        public int bombs = 3;

        public int gameWidth;

        bool goingToExplode;
        int explosionTimer;

        Sprite light;

        public int playerID;

        int downDiggingDelay;
        int sidewaysDiggingDelay;
        int direction;

        public Player(int playerID, string image, float airFriction, float groundFriction, int tileSize, int offsetX, int offsetY, GameInstance main, int gameWidth) : base(image, 4, 6)
        {
            SetOrigin(width / 2, height / 2);
            this.playerID = playerID;
            this.airFriction = airFriction;
            this.groundFriction = groundFriction;
            this.tileSize = tileSize;
            this.offsetX = offsetX;
            this.offsetY = offsetY;
            this.main = main;
            this.gameWidth = gameWidth;

            minX = offsetX;
            maxX = offsetX + gameWidth;

            light = new Sprite("circle2.png", false, false);
            light.SetOrigin(light.width / 2, light.height / 2);
            light.scale = 5;
            //main.AddChildAt(light, 8);
            light.blendMode = BlendMode.LIGHTING;

            //collider.isTrigger = true;

            groundCheck = new EasyDraw(width - 1, 10);
            AddChild(groundCheck);
            //groundCheck.Clear(255);
            groundCheck.SetOrigin(groundCheck.width /2, groundCheck.height /2);
            //groundCheck.collider.isTrigger = true;
            groundCheck.SetXY(0, height / 2);

            ceilingCheck = new EasyDraw(width - 1, 10);
            AddChild(ceilingCheck);
            //ceilingCheck.Clear(255);
            ceilingCheck.SetOrigin(ceilingCheck.width / 2, ceilingCheck.height / 2);
            //groundCheck.collider.isTrigger = true;
            ceilingCheck.SetXY(0, -height / 2);

            SetCycle(4, 4, 8);
        }

        public void setMovementValues(float gravity, float movementForce, float jumpForce)
        {
            this.gravity = gravity;
            this.movementForce = movementForce;
            this.jumpForce = jumpForce;
        }

        public void UpdateGeneral(float camY)
        {

            light.SetXY(x, y);

            //Console.WriteLine("PLAYER GROUNDED : " + grounded);

            tileX = Convert.ToInt32((((x - offsetX) - (tileSize / 2)) / tileSize));
            tileY = Convert.ToInt32((((y - offsetY) - (tileSize / 2)) / tileSize));

            if (groundCheck.GetCollisions(false).Length > 1 && groundCheck.GetCollisions(false)[0].y != 0)
            {
                grounded = true;

                y = groundCheck.GetCollisions(false)[0].y - groundCheck.y - (Convert.ToInt32(camY / tileSize) * tileSize) - tileSize / 2;
            } 
            else
            {
                grounded = false;
            }

            //Console.WriteLine(grounded);

            if (ceilingCheck.GetCollisions(false).Length > 1)
            {
                y += ceilingCheck.height / 2;
                y += velocityY;
                velocityY = 0;
            }



            velocityX /= groundFriction;
            velocityY /= airFriction;

            x += velocityX;
            y += velocityY;

            if (GetCollisions(false).Length > 2)
            {
                x -= velocityX;
            }

            if (GetCollisions(true, false).Length > 1)
            {
                if (GetCollisions(true, false)[0] is DynamitePickup)
                {
                    GetCollisions(true, false)[0].Destroy();
                    bombs++;

                    main.tntPickupSound.Play(false, 0, main.volumes[0]);
                }
            }

            //x = Mathf.Clamp(x, 0, gameWidth);

            //Console.WriteLine("PLAYER POSITION : " + x);

            x = Mathf.Clamp(x, minX, maxX);

        }

        public void UpdateInput(int up, int left, int down, int right, int dig, int bomb)
        {

            if (!goingToExplode)
            {
                bool isMoving = false;
                bool alreadyDug = false;

                if (!grounded)
                {
                    velocityY += gravity * Time.deltaTime;
                }
                else
                {
                    velocityY = 0;
                }

                //if (player.GetCollisions().Length != 0)
                //{
                //    player.velocityY = 0;
                //}

                //a
                if (Input.GetKey(right))
                {
                    velocityX += movementForce * Time.deltaTime;
                    direction = 1;
                    SetCycle(0 + 12 * direction, 4);
                    isMoving = true;

                    if (Input.GetKeyDown(dig))
                    {
                        main.DigTile(tileX + 1, tileY, playerID, offsetX);
                        SetCycle(8 + 12 * direction, 1);
                        sidewaysDiggingDelay = 16;
                        alreadyDug = true;
                    }
                }
                //d
                if (Input.GetKey(left))
                {
                    velocityX -= movementForce * Time.deltaTime;
                    direction = 0;
                    SetCycle(0 + 12 * direction, 4);
                    isMoving = true;

                    if (Input.GetKeyDown(dig))
                    {
                        main.DigTile(tileX - 1, tileY, playerID, offsetX);
                        SetCycle(8 + 12 * direction, 1);
                        sidewaysDiggingDelay = 16;
                        alreadyDug = true;
                    }
                }
                ////w
                //if (Input.GetKeyDown(up) && grounded)
                //{
                //    y -= 10;
                //    velocityY -= jumpForce;
                //    grounded = false;
                //}

                //if (Input.GetKey(up) && Input.GetKeyDown(dig))
                //{
                //    main.DigTile(tileX, tileY - 1);
                //}

                if (Input.GetKeyDown(dig) && !alreadyDug)
                {
                    main.DigTile(tileX, tileY + 1, playerID, offsetX);
                    SetCycle(8 + 12 * direction, 1);
                    downDiggingDelay = 16;
                    alreadyDug = true;
                }

                if (!isMoving && !alreadyDug)
                {
                    SetCycle(4 + 12 * direction, 4);
                }

                if (sidewaysDiggingDelay > 0)
                {
                    sidewaysDiggingDelay--;
                    SetCycle(8 + 12 * direction, 1);
                }
                if (downDiggingDelay > 0)
                {
                    downDiggingDelay--;
                    SetCycle(9 + 12 * direction, 1);
                }

                if (Input.GetKeyDown(bomb) && bombs > 0)
                {
                    goingToExplode = true;
                    explosionTimer = 0;

                    SetCycle(10 + 12 * direction, 2, 16);
                }
            }
            else
            {
                explosionTimer += Time.deltaTime;

                if (explosionTimer > 500)
                {
                    explosionTimer = 0;
                    goingToExplode = false;
                    main.ExplodeTile(tileX, tileY, playerID, offsetX);
                    bombs--;
                    SetCycle(4, 4, 8); 
                }
            }
            AnimateFixed();
        }

    }
}
