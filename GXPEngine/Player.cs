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

        public bool grounded = false;

        public Player(string image, float airFriction, float groundFriction, int tileSize, int offsetY) : base(image)
        {
            SetOrigin(width / 2, height / 2);
            this.airFriction = airFriction;
            this.groundFriction = groundFriction;
            this.tileSize = tileSize;
            this.offsetY = offsetY;

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

        public void Update()
        {

            tileX = Convert.ToInt32(((x - (tileSize / 2)) / tileSize));
            tileY = Convert.ToInt32((((y - offsetY) - (tileSize / 2)) / tileSize));

            Console.WriteLine(velocityY);
            Console.WriteLine(x + " " + y);
            Console.WriteLine(tileX + " " + tileY);

            Console.WriteLine(groundCheck.GetCollisions().Length);

            if (groundCheck.GetCollisions().Length > 1)
            {
                grounded = true;
                y = groundCheck.GetCollisions()[0].y - groundCheck.y;
            } 
            else
            {
                grounded = false;
            }

            if (ceilingCheck.GetCollisions().Length > 1)
            {
                y += ceilingCheck.height / 2;
                y += velocityY;
                velocityY = 0;
            }



            velocityX /= groundFriction;
            velocityY /= airFriction;

            x += velocityX;
            y += velocityY;

            if (GetCollisions().Length > 2)
            {
                x -= velocityX;
            }

            Console.WriteLine(grounded);
        }

    }
}
