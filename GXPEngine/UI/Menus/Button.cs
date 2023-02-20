using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.UI.Menus
{
    internal class Button : Sprite
    {

        public Button(String image, int width, int height, int function) : base(image)
        {
            this.SetScaleXY(width / this.width, height / this.height);
        }


    }
}
