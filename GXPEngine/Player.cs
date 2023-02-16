using GXPEngine.Core;
using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledMapParser;

namespace GXPEngine
{
    internal class Player : Sprite
    {

        Digging main;

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

        public Player(string image, float airFriction, float groundFriction, int tileSize, int offsetY, Digging main) : base(image)
        {
            SetOrigin(width / 2, height / 2);
            this.airFriction = airFriction;
            this.groundFriction = groundFriction;
            this.tileSize = tileSize;
            this.offsetY = offsetY;
            this.main = main;

            groundCheck = new EasyDraw(width - 1, 10);
            AddChild(groundCheck);
            groundCheck.Clear(255);
            groundCheck.SetOrigin(groundCheck.width /2, groundCheck.height /2);
            groundCheck.SetXY(0, height / 2);

            ceilingCheck = new EasyDraw(width - 1, 10);
            AddChild(ceilingCheck);
            ceilingCheck.Clear(255);
            ceilingCheck.SetOrigin(ceilingCheck.width / 2, ceilingCheck.height / 2);
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

            tileX = Convert.ToInt32(((x - (tileSize / 2)) / tileSize));
            tileY = Convert.ToInt32((((y - offsetY) - (tileSize / 2)) / tileSize));

            if (groundCheck.GetCollisions(false).Length > 1)
            {
                grounded = true;
                y = groundCheck.GetCollisions(false)[0].y - groundCheck.y - (Convert.ToInt32(main.y / tileSize) * tileSize);
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

        }

        public void UpdateMovement(int up, int left, int down, int right, int dig)
        {
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
                }
            }
            //d
            if (Input.GetKey(left))
            {
                velocityX -= movementForce * Time.deltaTime;

                if (Input.GetKeyDown(dig))
                {
                    main.DigTile(tileX - 1, tileY);
                }
            }
            //w
            if (Input.GetKeyDown(up) && grounded)
            {
                y -= 10;
                velocityY -= jumpForce;
                grounded = false;
            }

            if (Input.GetKey(up) && Input.GetKeyDown(dig))
            {
                main.DigTile(tileX, tileY - 1);
            }

            if (Input.GetKey(down) && Input.GetKeyDown(dig))
            {
                main.DigTile(tileX, tileY + 1);
            }

        }

    }
}
