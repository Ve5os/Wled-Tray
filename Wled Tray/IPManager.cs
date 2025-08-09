using System;
using System.Collections.Generic;
using System.IO;

namespace Wled_Tray
{
    internal static class IPManager
    {
        private static readonly string AppDataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "Wled_Tray"
        );

        private static readonly string IpFilePath = Path.Combine(AppDataFolder, "ips.txt");

        /// <summary>
        /// Инициализация файла IP — создаёт папку и пустой файл, если их нет
        /// </summary>
        public static void Ini()
        {
            try
            {
                Directory.CreateDirectory(AppDataFolder);

                if (!File.Exists(IpFilePath))
                {
                    File.WriteAllText(IpFilePath, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при инициализации IP файла: " + ex.Message);
            }
        }

        /// <summary>
        /// Загружает список IP из файла
        /// </summary>
        public static List<string> LoadIps()
        {
            try
            {
                if (!File.Exists(IpFilePath))
                    return new List<string>();

                var lines = File.ReadAllLines(IpFilePath);
                return new List<string>(lines);
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Сохраняет новый IP в файл (если его там ещё нет)
        /// </summary>
        public static void SaveIp(string ip)
        {
            try
            {
                Directory.CreateDirectory(AppDataFolder);

                var ips = LoadIps();
                if (!ips.Contains(ip))
                {
                    ips.Add(ip);
                    File.WriteAllLines(IpFilePath, ips);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при сохранении IP: " + ex.Message);
            }
        }

        /// <summary>
        /// Удаляет IP из файла
        /// </summary>
        public static void DeleteIp(string ip)
        {
            try
            {
                if (!File.Exists(IpFilePath))
                    return;

                var ips = LoadIps();
                if (ips.Remove(ip))
                {
                    File.WriteAllLines(IpFilePath, ips);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении IP: " + ex.Message);
            }
        }
    }
}
