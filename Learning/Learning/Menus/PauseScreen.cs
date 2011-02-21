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
        MenuItem exitGameItem;

        MainGame callingGame;

        public PauseScreen(MainGame game)
            : base("Game Paused")
        {
            callingGame = game;

            resumeGameItem = new MenuItem("Resume Game");
            optionsItem = new MenuItem("Options");
            exitGameItem = new MenuItem("Exit Game");

            resumeGameItem.Selected += new EventHandler(resumeGameItem_Selected);
            optionsItem.Selected += new EventHandler(optionsItem_Selected);
            exitGameItem.Selected += new EventHandler(exitGameItem_Selected);

            MenuItems.Add(resumeGameItem);
            MenuItems.Add(optionsItem);
            MenuItems.Add(exitGameItem);
           
        }

        void exitGameItem_Selected(object sender, EventArgs e)
        {
            ScreenManager.AddScreen(new ConfirmationDialog(
                "Are you sure?",
                closeConfirmed,
                closeDenied));
        }

        void closeConfirmed()
        {
            callingGame.ExitScreen();
            ExitScreen();
        }
        void closeDenied() { }

        void optionsItem_Selected(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void resumeGameItem_Selected(object sender, EventArgs e)
        {
            ExitScreen();
        }
    }
}
