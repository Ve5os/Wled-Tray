using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wled_Tray
{
    internal class LanguagePackage
    {
        private MainWindow mainWindow;
        private PopupWindow popupWindow;

        public LanguagePackage(MainWindow main, PopupWindow popup)
        {
            mainWindow = main;
            popupWindow = popup;
        }

        public void Update()
        {
            IniFile MyIni = new IniFile("Settings.ini");

            if (!MyIni.KeyExists("Lang"))
            {
                MyIni.Write("Lang", "ru");
            }

            if (MyIni.Read("Lang") == "en")
            {
                mainWindow.Main.Content = "⚙️ Main";
                mainWindow.Information.Content = "⚠️ Information";
                mainWindow.LangLabel.Content = "Language";
                mainWindow.AddBtn.Content = "Add";
                mainWindow.Title = "Settings Wled Tray";
                mainWindow.TittleLabel.Content = "Settings Wled Tray";
            }
        }

        public void Write(string s)
        {
            IniFile MyIni = new IniFile("Settings.ini");
            MyIni.Write("Lang", s);
            Update();
        }
    }

}
