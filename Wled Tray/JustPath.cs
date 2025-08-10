using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wled_Tray
{
    internal class JustPath
    {
        public static string GetFolder()
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
            return folderPath;
        }
        public static string GetIni()
        {
            return Path.Combine(GetFolder(), "Settings.ini");
        }
        public static string GetIps()
        {
            return Path.Combine(GetFolder(), "ips.txt");
        }
    }
}
