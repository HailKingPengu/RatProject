using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class DynamitePickup : Sprite
    {

        public int pickupDepth;

        public DynamitePickup() : base("tnt.png")
        {
            //SetOrigin(width/2, height/2);
            SetScaleXY(0.8f, 0.8f);
            collider.isTrigger = true;

            Sprite glow2 = new Sprite("circle3.png", false, false);
            glow2.SetOrigin(glow2.width / 2, glow2.height / 2);
            glow2.SetXY(width / 2, height / 2);
            glow2.scale = 2;
            //glow2.SetColor(200, 50, 50);
            glow2.alpha = 0.4f;
            AddChild(glow2);

            Sprite glow = new Sprite("circle2.png", false, false);
            glow.SetOrigin(glow.width / 2, glow.height / 2);
            glow.SetXY(width/ 2, height / 2);
            glow.scale = 3;
            glow.blendMode = BlendMode.LIGHTING;
            AddChild(glow);
        }
    }
}
