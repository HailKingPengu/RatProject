using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine.UI.Menus
{
    internal class VolumeSlider : Pivot
    {
        public float value = 0.5f;

        public bool selected;

        AnimationSprite slider;
        Sprite bar;

        int offsetY;

        int id;
        GameInstance gameInstance;

        public VolumeSlider(int id, string sliderImage, string barImage, int offsetY, GameInstance gameInstance)
        {
            slider = new AnimationSprite(sliderImage, 2, 1);
            bar = new Sprite(barImage);
            AddChild(bar);
            AddChild(slider);

            bar.SetOrigin(bar.width/2, bar.height/2);
            bar.SetXY(683, 384 + offsetY);

            slider.SetOrigin(slider.width/2, slider.height/2);
            slider.SetXY(683 - 140 + 290 * value, 384 + offsetY);

            this.gameInstance = gameInstance;
            this.offsetY = offsetY;
            this.id = id;
        }

        public void Selected(bool selected)
        {
            this.selected = selected;
            if (selected == false)
            {
                slider.currentFrame = 0;
            }
            else
            {
                slider.currentFrame = 1;
            }
        }

        public float ValueAdded()
        {
            if (value < 1)
            {
                value += 0.1f;
                value = Mathf.Clamp(value, 0, 1);
                gameInstance.volumes[id] = value;
                slider.SetXY(683 - 140 + 290 * value, 384 + offsetY);
            }

            return value;
        }

        public float ValueSubtracted()
        {
            if (value > 0)
            {
                value -= 0.1f;
                value = Mathf.Clamp(value, 0, 1);
                gameInstance.volumes[id] = value;
                slider.SetXY(683 - 140 + 290 * value, 384 + offsetY);
            }

            return value;
        }
    }
}
