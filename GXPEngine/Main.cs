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
        game = new GameInstance(width, height, "backGround.png", this);
        game.paused = true;
        //AddChild(game);

        menus = new MenuHandler(width, height, this, game);
        AddChild(menus);

        //game2 = new GameInstance(width / 2, height);
        //game2.SetXY(width/2, 0);
        //AddChild(game2);
    }

    public void PlayGame()
    {
        RemoveChild(menus);
        menus.isActive = false;

        AddChild(game);
        //game.paused = false;
        game.StartGame();
    }

    public void PauseGame()
    {
        AddChildAt(menus, 1);
        menus.isActive = true;
        menus.AddChild(menus.menus[3]);
        menus.currentMenu = 3;
        game.paused = true;
    }

    public void ResumeGame()
    {
        RemoveChild(menus);
        menus.isActive = false;
        menus.RemoveChild(menus.menus[menus.currentMenu]);
        game.paused = false;
        game.AddChild(game.p2Camera);
    }

    static void Main()                          // Main() is the first method that's called when the program is run
    {
        new Digging().Start();
    }
}
