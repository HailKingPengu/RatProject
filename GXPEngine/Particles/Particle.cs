using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Particles
{
    internal class Particle : AnimationSprite
    {

        int lifeTime;
        int maxLifeTime;
        float beginOpacity;
        float deltaOpacity;

        float velocityX;
        float velocityY;

        float forceX;
        float forceY;

        float friction;

        public Particle(float beginOpacity, float endOpacity, int maxLifeTime, String fileName, int frames) : base(fileName, 1, frames, -1, false, false)
        {
            alpha = beginOpacity;
            this.beginOpacity = beginOpacity;
            deltaOpacity = beginOpacity - endOpacity;
            this.maxLifeTime = maxLifeTime;

            currentFrame = Utils.Random(0, frames - 1);
        }

        public void setForces(float velocityX, float velocityY, float forceX, float forceY, float friction)
        {
            this.velocityX = Utils.Random(-velocityX, velocityX);
            this.velocityY = Utils.Random(-velocityY, velocityY);
            this.forceX = forceX;
            this.forceY = forceY;
            this.friction = friction;
        }

        public void setForcesSync(float velocityX, float velocityY, float random, float forceX, float forceY, float friction)
        {
            this.velocityX = Utils.Random(velocityX - random, velocityX + random);
            this.velocityY = Utils.Random(velocityY - random, velocityY + random);
            this.forceX = forceX;
            this.forceY = forceY;
            this.friction = friction;
        }

        public void Update()
        {
            velocityX = (velocityX * friction) + forceX;
            velocityY = (velocityY * friction) + forceY;


            SetXY(x + velocityX, y + velocityY);
            lifeTime += Time.deltaTime;
            alpha = beginOpacity - deltaOpacity * lifeTime / maxLifeTime;
            if (lifeTime > maxLifeTime)
            {
                this.Destroy();
            }
        }
    }
}
