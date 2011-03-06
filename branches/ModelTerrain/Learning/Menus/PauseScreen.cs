using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Learning.Menus
{
    class PauseScreen : MenuScreen
    {
        MenuItem resumeGameItem;
        MenuItem optionsItem;
        MenuItem mainMenuItem;
        MenuItem exitGameItem;

        MainGame callingGame;

        public PauseScreen(MainGame game)
            : base("Game Paused")
        {
            callingGame = game;

            resumeGameItem = new MenuItem("Resume Game");
            optionsItem = new MenuItem("Options");
            mainMenuItem = new MenuItem("Main Menu");
            exitGameItem = new MenuItem("Exit Game");

            resumeGameItem.Selected += new EventHandler(resumeGameItem_Selected);
            optionsItem.Selected += new EventHandler(optionsItem_Selected);
            mainMenuItem.Selected += new EventHandler(mainMenuItem_Selected);
            exitGameItem.Selected += new EventHandler(exitGameItem_Selected);

            MenuItems.Add(resumeGameItem);
            MenuItems.Add(optionsItem);
            MenuItems.Add(mainMenuItem);
            MenuItems.Add(exitGameItem);
           
        }

        void mainMenuItem_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(
                new ConfirmationDialog(
                "Are you sure?",
                mainMenuConfirmed,
                closeDenied));
        }

        void exitGameItem_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(
                new ConfirmationDialog(
                "Are you sure?",
                exitGameConfirmed,
                closeDenied));
        }

        void mainMenuConfirmed()
        {
            callingGame.ExitScreen();
            ExitScreen();
        }
        void exitGameConfirmed()
        {
            ScreenManager.Game.Exit();
        }
        void closeDenied() { }

        void optionsItem_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenu());
        }

        void resumeGameItem_Selected(object sender, EventArgs e)
        {
            ExitScreen();
        }
    }
}
