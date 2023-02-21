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
        public Button(String image, int x, int y, float scale, int[] function, MenuHandler menuHandler) : base(image, 2, 1)
        {
            this.x = x;
            this.y = y;

            SetOrigin(width/2, height/2);
            this.scale = scale;

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
