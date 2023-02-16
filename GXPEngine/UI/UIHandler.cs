using GXPEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GXPEngine
{
    internal class UIHandler : Pivot
    {

        public ProgressBar diggingP1;
        public ProgressBar diggingP2;

        EasyDraw depthTooltip1;

        Player player1; 

        public UIHandler(Player player1, Player player2)
        {
            this.player1 = player1;

            depthTooltip1 = new EasyDraw(200, 40, false);
            depthTooltip1.SetXY(50, 50);
            AddChild(depthTooltip1);
            depthTooltip1.Fill(255);
        }

        public void Update()
        {
            depthTooltip1.ClearTransparent();
            depthTooltip1.Text(player1.tileY.ToString());
        }

    }
}
