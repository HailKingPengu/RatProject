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
    internal class Player : Sprite
    {

        GameInstance main;

        int tileSize;
        int offsetY;

        public int tileX;
        public int tileY;

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

        public int bombs = 10;

        public int gameWidth;

        Sprite light;

        public Player(string image, float airFriction, float groundFriction, int tileSize, int offsetY, GameInstance main, int gameWidth) : base(image)
        {
            SetOrigin(width / 2, height / 2);
            this.airFriction = airFriction;
            this.groundFriction = groundFriction;
            this.tileSize = tileSize;
            this.offsetY = offsetY;
            this.main = main;
            this.gameWidth = width;

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
        }

        public void setMovementValues(float gravity, float movementForce, float jumpForce)
        {
            this.gravity = gravity;
            this.movementForce = movementForce;
            this.jumpForce = jumpForce;
        }

        public void Update()
        {

            light.SetXY(x, y);

            Console.WriteLine("PLAYER GROUNDED : " + grounded);

            tileX = Convert.ToInt32(((x - (tileSize / 2)) / tileSize));
            tileY = Convert.ToInt32((((y - offsetY) - (tileSize / 2)) / tileSize));

            if (groundCheck.GetCollisions(false).Length > 1)
            {
                grounded = true;
                y = groundCheck.GetCollisions(false)[0].y - groundCheck.y - (Convert.ToInt32(main.camY / tileSize) * tileSize) - tileSize / 2;
            } 
            else
            {
                grounded = false;
            }

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

            //x = Mathf.Clamp(x, 0, gameWidth);

            //Console.WriteLine("PLAYER POSITION : " + x);

        }

        public void UpdateInput(int up, int left, int down, int right, int dig, int bomb)
        {

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

                if (Input.GetKeyDown(dig))
                {
                    main.DigTile(tileX + 1, tileY);
                    alreadyDug = true;
                }
            }
            //d
            if (Input.GetKey(left))
            {
                velocityX -= movementForce * Time.deltaTime;

                if (Input.GetKeyDown(dig))
                {
                    main.DigTile(tileX - 1, tileY);
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
                main.DigTile(tileX, tileY + 1);
            }

            if (Input.GetKeyDown(bomb) && bombs > 0)
            {
                main.ExplodeTile(tileX, tileY);
                bombs--;
            }

        }

    }
}
