using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wled_Tray
{
    public static class AutoStartManager
    {
        private const string RunRegistryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
        private static string AppName = "WledTray"; 

        public static void EnableAutoStart()
        {
            string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RunRegistryKey, true))
                {
                    if (key == null)
                        throw new Exception("Не удалось открыть ключ реестра для автозапуска.");

                    key.SetValue(AppName, $"\"{exePath}\"");
                }
            }
            catch ()
            {
            }
        }

        public static void DisableAutoStart()
        {
            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RunRegistryKey, true))
                {
                    if (key == null)
                        return; // Ключа нет — значит ничего не надо удалять

                    if (key.GetValue(AppName) != null)
                        key.DeleteValue(AppName);
                }
            }
            catch (Exception ex)
            {
                // Лог или сообщение об ошибке
                throw new Exception("Ошибка при отключении автозапуска: " + ex.Message);
            }
        }

        public static bool IsAutoStartEnabled()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RunRegistryKey, false))
            {
                if (key == null)
                    return false;

                return key.GetValue(AppName) != null;
            }
        }
    }
}
