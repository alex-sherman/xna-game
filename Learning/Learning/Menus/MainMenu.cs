using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Menus
{
    class MainMenu : MenuScreen
    {
        public MainMenu() : base("GAME!")
        {
            MenuItem playGameItem = new MenuItem("Play");
            MenuItem loadItem = new MenuItem("Load Game");
            MenuItem optionsItem = new MenuItem("Options");
            MenuItem exitItem = new MenuItem("Exit");

            // add event handlers
            playGameItem.Selected += new EventHandler(playGameItem_Selected);
            loadItem.Selected += new EventHandler(loadItem_Selected);
            exitItem.Selected += new EventHandler(exitItem_Selected);
            optionsItem.Selected += new EventHandler(optionsItem_Selected);

            // add to menu
            MenuItems.Add(playGameItem);
            MenuItems.Add(loadItem);
            MenuItems.Add(optionsItem);
            MenuItems.Add(exitItem);
        }

        void loadItem_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new LoadScreen());
        }

        void optionsItem_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenu());
        }

        void playGameItem_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new MainGame());
        }

        void exitItem_Selected(object sender, EventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
