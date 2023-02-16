using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.Particles
{
    internal class ParticleSystem : GameObject
    {
        int spawnedParticles;

        float numParticles;
        float spawningTime;
        int maxLifeTime;
        String fileName;
        int frames;

        int forceType;
        float velX;
        float velY;
        float forceX;
        float forceY;
        float friction;

        float random;

        float beginOpacity = 1;
        float endOpacity = 1;

        float maxScale = 1;
        float minScale = 1;

        public ParticleSystem(int numParticles, int spawningTime, int maxLifeTime, String fileName, int frames) : base(false)
        {
            this.numParticles = numParticles;
            this.spawningTime = spawningTime;
            this.maxLifeTime = maxLifeTime;
            this.fileName = fileName;
            this.frames = frames;
        }

        public void opacitySettings(float beginOpacity, float endOpacity)
        {
            this.beginOpacity = beginOpacity;
            this.endOpacity = endOpacity;
        }

        public void setforces(float velX, float velY, float forceX, float forceY, float friction)
        {
            forceType = 0;
            this.velX = velX;
            this.velY = velY;
            this.forceX = forceX;
            this.forceY = forceY;
            this.friction = friction;
        }

        public void setforcesSync(float velX, float velY, float random, float forceX, float forceY, float friction)
        {
            forceType = 1;
            this.velX = Utils.Random(-velX, velX);
            this.velY = Utils.Random(-velY, velY);
            this.random = random;
            this.forceX = forceX;
            this.forceY = forceY;
            this.friction = friction;
        }

        public void setScale(float maxScale, float minScale)
        {
            this.maxScale = maxScale;
            this.minScale = minScale;
        }

        public void Update()
        {
            int particlesToSpawn = Convert.ToInt32((Time.deltaTime / spawningTime) * numParticles);
            spawnedParticles += particlesToSpawn;

            if (spawnedParticles <= numParticles)
            {
                for (int i = 0; i < particlesToSpawn; i++)
                {
                    Particle p = new Particle(beginOpacity, endOpacity, maxLifeTime, fileName, frames);
                    switch (forceType)
                    {
                        case 0:
                            p.setForces(velX, velY, forceX, forceY, friction);
                        break;
                        case 1:
                            p.setForcesSync(velX, velY, random, forceX, forceY, friction);
                        break;
                    }
                    p.SetScaleXY(Utils.Random(minScale, maxScale));
                    AddChild(p);
                }
            }
            else
            {
                if (GetChildCount() == 0)
                {
                    Destroy();
                }
            }
        }
    }
}
