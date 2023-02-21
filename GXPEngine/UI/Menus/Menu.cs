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

        int currentButton;

        public Menu(String background, int screenWidth, int screenHeight) 
        { 
            this.background = new Sprite(background, false, false);
            //this.background.SetScaleXY((float)screenWidth / (float)this.background.width, (float)screenHeight / (float)this.background.height);
            AddChild(this.background);

            buttons = new List<Button>();
            sprites = new List<Sprite>();
        }

        public void AddButton(Button button)
        {
            buttons.Add(button);
            AddChild(button);
        }

        public void NextButton()
        {
            buttons[currentButton].Selected(false);
            currentButton++;
            if (currentButton == buttons.Count)
            {
                currentButton = 0;
            }
            buttons[currentButton].Selected(true);
        }

        public void PreviousButton()
        {
            buttons[currentButton].Selected(false);
            currentButton--;
            if (currentButton == -1)
            {
                currentButton = buttons.Count - 1;
            }
            buttons[currentButton].Selected(true);
        }

        public void ButtonPressed()
        {
            buttons[currentButton].Pressed();
        }
    }
}
