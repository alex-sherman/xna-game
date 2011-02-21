using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Learning.Menus
{
    class OptionsMenu : MenuScreen
    {
        MenuItem saveKeyItem;
        MenuItem loadKeyItem;
        MenuItem backItem;

        public OptionsMenu()
            : base("Options")
        {
            saveKeyItem = new MenuItem(string.Empty);
            loadKeyItem = new MenuItem(string.Empty);
            backItem = new MenuItem(string.Empty);

            UpdateMenuText();

            saveKeyItem.Selected += new EventHandler(saveKeyItem_Selected);
            loadKeyItem.Selected += new EventHandler(loadKeyItem_Selected);
            backItem.Selected += new EventHandler(backItem_Selected);

            MenuItems.Add(saveKeyItem);
            MenuItems.Add(loadKeyItem);
            MenuItems.Add(backItem);
        }

        protected void UpdateMenuText()
        {
            saveKeyItem.Text = String.Format("QuickSave: {0}", GameConstants.quickSaveKey);
            loadKeyItem.Text = String.Format("QuickLoad: {0}", GameConstants.quickLoadKey);
            backItem.Text = "Back";
        }

        void backItem_Selected(object sender, EventArgs e)
        {
            ExitScreen();
        }

        void loadKeyItem_Selected(object sender, EventArgs e)
        {
            loadKeyItem.Pulsate = true;
            catchNextInput(setLoadKey);
        }

        void setLoadKey(Keys key)
        {
            GameConstants.quickLoadKey = key;
            loadKeyItem.Pulsate = false;
            UpdateMenuText();
        }

        void saveKeyItem_Selected(object sender, EventArgs e)
        {
            saveKeyItem.Pulsate = true;
            catchNextInput(setSaveKey);
        }
        void setSaveKey(Keys key)
        {
            GameConstants.quickSaveKey = key;
            saveKeyItem.Pulsate = false;
            UpdateMenuText();
        }
    }
}
