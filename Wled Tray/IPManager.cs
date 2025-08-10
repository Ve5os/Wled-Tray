using System;
using System.Collections.Generic;
using System.IO;

namespace Wled_Tray
{
    internal static class IPManager
    {
       


        /// <summary>
        /// Инициализация файла IP — создаёт папку и пустой файл, если их нет
        /// </summary>
        public static void Ini()
        {
            try
            {
              

                if (!File.Exists(JustPath.GetIps()))
                {
                    File.WriteAllText(JustPath.GetIps(), string.Empty);
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
                if (!File.Exists(JustPath.GetIps()))
                    return new List<string>();

                var lines = File.ReadAllLines(JustPath.GetIps());
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

                var ips = LoadIps();
                if (!ips.Contains(ip))
                {
                    ips.Add(ip);
                    File.WriteAllLines(JustPath.GetIps(), ips);
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
                if (!File.Exists(JustPath.GetIps()))
                    return;

                var ips = LoadIps();
                if (ips.Remove(ip))
                {
                    File.WriteAllLines(JustPath.GetIps(), ips);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при удалении IP: " + ex.Message);
            }
        }
    }
}
