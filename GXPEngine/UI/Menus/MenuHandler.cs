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

        public int currentMenu;
        public List<Menu> menus = new List<Menu>();

        Digging main;

        GameInstance game;

        int canClick;

        public MenuHandler(int screenWidth, int screenHeight, Digging main, GameInstance game) 
        {
            Menu mainMenu = new Menu("StartPage.png", screenWidth, screenHeight, main);
            mainMenu.AddButton(new Button(0, "PlayButton.png", screenWidth / 2, 420, 0.7f, new int[] { 0, -1 }, this));
            mainMenu.AddButton(new Button(1, "OptionsButton.png", screenWidth/2, 530, 0.7f, new int[] { 0, 1 }, this));
            mainMenu.AddButton(new Button(2, "HowToButton.png", screenWidth / 2, 620, 0.5f, new int[] { 0, 2 }, this));
            mainMenu.Initialize();
            menus.Add(mainMenu);
            AddChild(mainMenu);

            Menu optionsMenu = new Menu("StartPage.png", screenWidth, screenHeight, main);
            optionsMenu.AddSlider(new VolumeSlider(0, "Slider.png", "VolumeSlider.png", 50, game));
            optionsMenu.AddSlider(new VolumeSlider(1, "Slider.png", "MusicSlider.png", 200, game));
            optionsMenu.AddButton(new Button(2, "BackButton.png", screenWidth / 2, 680, 1, new int[] { 0, 0 }, this));
            optionsMenu.Initialize();
            menus.Add(optionsMenu);

            Menu howToMenu = new Menu("HowToMenu.png", screenWidth, screenHeight, main);
            howToMenu.AddButton(new Button(0, "ControlsButton.png", screenWidth / 2 - 200, 650, 0.5f, new int[] { 0, 7 }, this));
            howToMenu.AddButton(new Button(1, "BackButton.png", screenWidth / 2 + 200, 650, 1, new int[] { 0, 0 }, this));
            howToMenu.Initialize();
            menus.Add(howToMenu);

            Menu pauseMenu = new Menu("PausingMenu.png", screenWidth, screenHeight, main);
            pauseMenu.AddButton(new Button(0, "OptionsButton.png", screenWidth / 2, 340, 0.7f, new int[] { 0, 4 }, this));
            pauseMenu.AddButton(new Button(1, "ControlsButton.png", screenWidth / 2, 500, 0.5f, new int[] { 0, 5 }, this));
            pauseMenu.Initialize();
            menus.Add(pauseMenu);

            Menu pauseOptionsMenu = new Menu("PausingMenu.png", screenWidth, screenHeight, main);
            pauseOptionsMenu.AddSlider(new VolumeSlider(0, "Slider.png", "VolumeSlider.png", -80, game));
            pauseOptionsMenu.AddSlider(new VolumeSlider(1, "Slider.png", "MusicSlider.png", 70, game));
            pauseOptionsMenu.AddButton(new Button(2, "BackButton.png", screenWidth / 2, 550, 1, new int[] { 0, 3 }, this));
            pauseOptionsMenu.Initialize();
            menus.Add(pauseOptionsMenu);

            Menu pauseControlMenu = new Menu("PauseControlsMenu.png", screenWidth, screenHeight, main);
            pauseControlMenu.AddButton(new Button(0, "BackButton.png", screenWidth / 2 + 150, 560, 1, new int[] { 0, 3 }, this));
            pauseControlMenu.Initialize();
            menus.Add(pauseControlMenu);

            Menu GameOverMenu = new Menu("GameOverMenu.png", screenWidth, screenHeight, main);
            GameOverMenu.AddButton(new Button(0, "PlayAgainButton.png", screenWidth / 2, 340, 0.7f, new int[] { 0, -1 }, this));
            GameOverMenu.AddButton(new Button(1, "MainMenuButton.png", screenWidth / 2, 550, 1, new int[] { 0, 0 }, this));
            menus.Add(GameOverMenu);

            Menu controlMenu = new Menu("ControlMenu.png", screenWidth, screenHeight, main);
            controlMenu.AddButton(new Button(0, "BackButton.png", screenWidth / 2, 680, 1, new int[] { 0, 2 }, this));
            controlMenu.Initialize();
            menus.Add(controlMenu);

            this.game = game;
            this.main = main;
            
            //menus[currentMenu].NextButton();
        }

        public void LoadScene(int scene)
        {
            RemoveChild(menus[currentMenu]);

            if (scene == -1)
            {
                main.PlayGame();
                game.ChangeSong(1);
            }

            if (scene == -2)
            {
                main.ResumeGame();
                game.ChangeSong(1);
            }

            if (scene == -3)
            {
                main.RestartGame();
                game.ChangeSong(1);
            }

            if (scene >= 0)
            {
                currentMenu = scene;

                AddChild(menus[currentMenu]);
            }
        }

        public void Update()
        {
            if (isActive && canClick < 0)
            {
                if(Input.GetKeyDown(Key.A))
                {
                    canClick = 50;
                    menus[currentMenu].PreviousButton();
                }

                if (Input.GetKeyDown(Key.D))
                {
                    canClick = 50;
                    menus[currentMenu].NextButton();
                }

                if (Input.GetKey(Key.E) || Input.GetKey(Key.Q))
                {
                    canClick = 30;
                    menus[currentMenu].ButtonPressed();
                }
            }
            canClick--;
        }
    }
}
