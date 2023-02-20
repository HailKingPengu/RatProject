using System;                                   // System contains a lot of default C# libraries 
using GXPEngine;                                // GXPEngine contains the engine
using System.Drawing;                           // System.Drawing contains drawing tools such as Color definitions
using TiledMapParser;
using System.Xml;
using GXPEngine.Particles;
using GXPEngine.UI;

public class Digging : Game
{

    MenuHandler menus;
    GameInstance game;

    public Digging() : base(1366, 768, false)
    {
        menus = new MenuHandler();
        AddChild(menus);

        game = new GameInstance(width, height, "backGround.png");
        AddChild(game);

        //game2 = new GameInstance(width / 2, height);
        //game2.SetXY(width/2, 0);
        //AddChild(game2);
    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new Digging().Start();
    }
}
