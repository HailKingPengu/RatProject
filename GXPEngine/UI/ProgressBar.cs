using GXPEngine.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.UI
{
    internal class ProgressBar : Pivot
    {
        public EasyDraw background;
        EasyDraw bar;

        public float progress;

        public ProgressBar(int width, int height, Color backgroundColor, Color barColor)
        {
            background = new EasyDraw(width, height);
            background.Clear(backgroundColor);
            AddChild(background);

            bar = new EasyDraw(width, height);
            bar.Clear(barColor);
            AddChild(bar);
        }

        public void Update()
        {
            SetScaleXY(progress, 1);
        }
    }
}
