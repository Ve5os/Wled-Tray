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
            IniFile MyIni = GetIni();

            if (!MyIni.KeyExists("Lang"))
            {
                MyIni.Write("Lang", "ru");
            }

            if (MyIni.Read("Lang") == "en")
            {
                try
                {
                    mainWindow.ErrorMessage = "The WLED device is offline.";
                    mainWindow.InvalidIP = "Invalid IP address!";
                    mainWindow.Main.Content = "⚙️ Main";
                    mainWindow.Information.Content = "⚠️ Information";
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
            IniFile MyIni = GetIni();
            MyIni.Write("Lang", s);
            Update();
        }
        private IniFile GetIni()
        {
            // Получаем путь к папке AppData\Roaming
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // Формируем путь к папке Wled_Tray
            string folderPath = Path.Combine(appDataPath, "Wled_Tray");

            // Проверяем, существует ли папка, если нет — создаём
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Формируем полный путь к файлу Settings.ini
            string iniFilePath = Path.Combine(folderPath, "Settings.ini");

            // Инициализируем твой ini-файл с полным путём
            return new IniFile(iniFilePath);
        }
    }

}
