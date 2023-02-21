using GXPEngine.UI.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.UI
{
    internal class MenuHandler : Pivot
    {

        public bool isActive = true;

        int currentMenu;
        List<Menu> menus = new List<Menu>();

        Digging main;

        public MenuHandler(int screenWidth, int screenHeight, Digging main) 
        {
            Menu mainMenu = new Menu("StartPage.png", screenWidth, screenHeight);
            mainMenu.AddButton(new Button("button.png", 150, 300, 64, 64, new int[] { 0, 1 }, this));
            mainMenu.AddButton(new Button("button.png", 300, 300, 64, 64, new int[] { 0, -1 }, this));
            mainMenu.AddButton(new Button("button.png", 450, 300, 64, 64, new int[] { 0, 2 }, this));
            menus.Add(mainMenu);
            AddChild(mainMenu);

            Menu optionsMenu = new Menu("PauseMenu.png", screenWidth, screenHeight);
            menus.Add(optionsMenu);
            //AddChild(optionsMenu);

            this.main = main;

        }

        public void LoadScene(int scene)
        {
            RemoveChild(menus[currentMenu]);

            if (scene == -1)
            {
                main.PlayGame();
            }

            if (scene >= 0)
            {
                currentMenu = scene;

                AddChild(menus[currentMenu]);
            }
        }

        public void Update()
        {
            if (isActive)
            {
                if(Input.GetKeyDown(Key.A))
                {
                    menus[currentMenu].PreviousButton();
                }

                if (Input.GetKeyDown(Key.D))
                {
                    menus[currentMenu].NextButton();
                }

                if (Input.GetKeyDown(Key.E))
                {
                    menus[currentMenu].ButtonPressed();
                }
            }
        }
    }
}
