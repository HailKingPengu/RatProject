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
        List<GameObject> selectables;
        List<Sprite> sprites;

        GameObject selectedItem;
        String backgroundString;

        Digging main;

        int currentID;
        int allIDS;

        public Menu(String backgroundString, int screenWidth, int screenHeight, Digging main) 
        { 
            this.background = new Sprite(backgroundString, false, false);
            //this.background.SetScaleXY((float)screenWidth / (float)this.background.width, (float)screenHeight / (float)this.background.height);
            AddChild(this.background);

            selectables = new List<GameObject>();
            sprites = new List<Sprite>();
            this.backgroundString = backgroundString;
            this.main = main;
        }

        public void AddButton(Button button)
        {
            selectables.Add(button);
            AddChild(button);

            allIDS++;
        }

        public void AddSlider(VolumeSlider slider)
        {
            selectables.Add(slider);
            AddChild(slider);

            allIDS++;
        }

        public void Update()
        {
            if (Input.GetKeyDown(Key.K))
            {
                if (backgroundString.Contains("PausingMenu.png"))
                {
                    main.ResumeGame();
                }
            }
        }

        public void Initialize()
        {
            NextButton();
            PreviousButton();
        }

        public void NextButton()
        {
            if (selectables[currentID] is Button)
            {
                Button selectedButton = selectables[currentID] as Button;
                selectedButton.Selected(false);
            }
            else if (selectables[currentID] is VolumeSlider)
            {
                VolumeSlider selectedSlider = selectables[currentID] as VolumeSlider;
                selectedSlider.Selected(false);
            }

            currentID++;
            if (currentID == selectables.Count)
            {
                currentID = 0;
            }

            if (selectables[currentID] is Button)
            {
                Button selectedButton = selectables[currentID] as Button;
                selectedButton.Selected(true);
            }
            else if (selectables[currentID] is VolumeSlider)
            {
                VolumeSlider selectedSlider = selectables[currentID] as VolumeSlider;
                selectedSlider.Selected(true);
            }

        }

        public void PreviousButton()
        {
            if (selectables[currentID] is Button)
            {
                Button selectedButton = selectables[currentID] as Button;
                selectedButton.Selected(false);
            }
            else if (selectables[currentID] is VolumeSlider)
            {
                VolumeSlider selectedSlider = selectables[currentID] as VolumeSlider;
                selectedSlider.Selected(false);
            }

            currentID--;
            if (currentID == -1)
            {
                currentID = allIDS - 1;
            }

            if (selectables[currentID] is Button)
            {
                Button selectedButton = selectables[currentID] as Button;
                selectedButton.Selected(true);
            }
            else if (selectables[currentID] is VolumeSlider)
            {
                VolumeSlider selectedSlider = selectables[currentID] as VolumeSlider;
                selectedSlider.Selected(true);
            }
        }

        public void ButtonPressed()
        {
            if (selectables[currentID] is Button)
            {
                Button selectedButton = selectables[currentID] as Button;
                selectedButton.Pressed();
            }
            else if (selectables[currentID] is VolumeSlider)
            {
                VolumeSlider selectedSlider = selectables[currentID] as VolumeSlider;
                if (Input.GetKey(Key.E))
                {
                    selectedSlider.ValueAdded();
                }
                if (Input.GetKey(Key.Q))
                {
                    selectedSlider.ValueSubtracted();
                }
            }
        }
    }
}
