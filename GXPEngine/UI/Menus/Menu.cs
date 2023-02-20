using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.UI.Menus
{
    internal class Menu : Pivot
    {

        //menu with a background, sprites, and buttons.

        Sprite background;
        List<Button> buttons;
        List<Sprite> sprites;

        public Menu(String background, int screenWidth, int screenHeight) 
        { 
            this.background = new Sprite(background, false, false);
            this.background.SetScaleXY(screenWidth / this.background.width, screenHeight / this.background.height);
            AddChild(this.background);

            this.buttons = new List<Button>();
            this.sprites = new List<Sprite>();
        }

        public void AddButton(Button button)
        {
            this.buttons.Add(button);
            AddChild(button);
        }
    }
}
