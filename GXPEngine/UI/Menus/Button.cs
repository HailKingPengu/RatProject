using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.UI.Menus
{
    internal class Button : AnimationSprite
    {

        MenuHandler menuHandler;

        public bool selected;
        int[] function;
        //function is an array with length two: first int is function, second int is what is Affected

        //button has three states: idle, selected and pressed
        public Button(String image, int x, int y, int width, int height, int[] function, MenuHandler menuHandler) : base(image, 3, 1)
        {
            this.x = x;
            this.y = y;

            this.SetScaleXY(width / this.width, height / this.height);

            this.function = function;
            this.menuHandler = menuHandler;
        }

        public void Selected(bool selected)
        {
            this.selected = selected;
            if (selected == false)
            {
                currentFrame = 0;
            } 
            else
            {
                currentFrame = 1;
            }
        }

        public void Pressed()
        {
            switch (function[0])
            {
                case 0:
                    menuHandler.LoadScene(function[1]);
                    break;
            }
        }
    }
}
