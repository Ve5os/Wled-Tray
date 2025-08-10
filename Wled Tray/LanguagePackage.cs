using System;
using System.Collections.Generic;
using System.IO;
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
            IniFile MyIni = new IniFile(JustPath.GetIni());

            if (!MyIni.KeyExists("Lang"))
            {
                MyIni.Write("Lang", "en");
            }

            if (MyIni.Read("Lang") == "en")
            {
                try
                {
                    mainWindow.ErrorMessage = "The WLED device is offline.";
                    mainWindow.InvalidIP = "Invalid IP address!";
                    mainWindow.Main.Content = "⚙️ Main";
                    mainWindow.Information.Content = "⚠️ Information";
                    mainWindow.AutoLabel.Content = "Autostart";
                    mainWindow.AddAuto.Content = "Enable";
                    mainWindow.RemoveAuto.Content = "Disable";
                    mainWindow.LangLabel.Content = "Language";
                    mainWindow.AddBtn.Content = "Add";
                    mainWindow.Title = "Settings Wled Tray";
                    mainWindow.TittleLabel.Content = "Settings Wled Tray";
                }
                catch { }
            }
        }

        public void Write(string s)
        {
            IniFile MyIni = new IniFile(JustPath.GetIni());
            MyIni.Write("Lang", s);
            Update();
        }
    }
}
