using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class LightingOverlay : Pivot
    {

        EasyDraw overlay;

        List<Light> lights = new List<Light>();

        Transformable offset;

        public LightingOverlay(int width, int height) 
        {
            overlay = new EasyDraw(width, height, false);
            overlay.Clear(100, 100, 100);
            overlay.blendMode = BlendMode.MULTIPLY;
            AddChild(overlay);
        }

        public void AddLightSource(Transformable point, float scale)
        {
            Light light = new Light(point, this, scale);
            lights.Add(light);
            AddChild(light);
        }

        public void UpdateOverlay(float cameraDepth)
        {
            overlay.Clear(255 - Convert.ToInt32(Mathf.Clamp(cameraDepth, 0, 200)));
            Console.WriteLine(cameraDepth);

            foreach(Light light in lights)
            {
                //if(light.point.x > 50)
                //{
                //    light.alpha= 1;
                //}
                float brightness = Mathf.Clamp(cameraDepth * 3, 0, 255) / 400;

                light.SetColor(brightness, brightness, brightness);
                //Console.WriteLine(Convert.ToUInt32(Mathf.Clamp(cameraDepth * 2, 0, 255)));

            }
        }
    }
}
