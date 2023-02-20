using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class Light : Sprite
    {

        public Transformable point;
        Transformable offset;

        public Light(Transformable point, Transformable offset, float Scale) : base("circle2.png", false, false)
        {
            SetOrigin(width/2, height/2);
            blendMode = BlendMode.LIGHTING;
            scale= Scale;
            this.point = point;
            this.offset = offset;
        }

        void Update()
        {
            SetXY(point.x - offset.x, point.y - offset.y);
        }
    }
}
