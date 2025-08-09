using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Wled_Tray
{
    public partial class MainWindow : Window
    {
        private LanguagePackage languagePackage;
        private ObservableCollection<SimpleCard> cards = new ObservableCollection<SimpleCard>();
        public string ErrorMessage = "Устройство WLED не в сети.";
        public string InvalidIP = "Невалидный IP-адрес!";
        public MainWindow()
        {
            InitializeComponent();
            this.Hide();

            // Инициализация IPManager (создаём файл, если его нет)
            IPManager.Ini();

            LoadUI();

            CardList.ItemsSource = cards;

            languagePackage = new LanguagePackage(this, null);
            languagePackage.Update();
        }
        private async void LoadUI()
        {
            // Загружаем IP в карточки
            foreach (var ip in IPManager.LoadIps())
            {
                // Получаем имя устройства через WLED API
                string name = await GetNameFromDevice(ip);
                cards.Add(new SimpleCard
                {
                    Title = $"{name} [{ip}]"
                });
            }
        }

        private void DragBorder_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ButtonState == System.Windows.Input.MouseButtonState.Pressed)
            {
                try { this.DragMove(); } catch { }
            }
        }

        private void Rus_Clk(object sender, RoutedEventArgs e) => languagePackage.Write("ru");
        private void Eng_Clk(object sender, RoutedEventArgs e) => languagePackage.Write("en");
        private void CloseBTN_Click(object sender, RoutedEventArgs e) => this.Close();

        private void Main_Clk(object sender, RoutedEventArgs e)
        {
            _1.Visibility = Visibility.Visible;
            _2.Visibility = Visibility.Hidden;
            _3.Visibility = Visibility.Hidden;
        }

        private void Wled_Clk(object sender, RoutedEventArgs e)
        {
            _1.Visibility = Visibility.Hidden;
            _2.Visibility = Visibility.Visible;
            _3.Visibility = Visibility.Hidden;
        }

        private void Info_Clk(object sender, RoutedEventArgs e)
        {
            _1.Visibility = Visibility.Hidden;
            _2.Visibility = Visibility.Hidden;
            _3.Visibility = Visibility.Visible;
        }

        public async Task<string> GetNameFromDevice(string ip)
        {
            var client = new HttpClient();
            try
            {
                var xmlString = await client.GetStringAsync($"http://{ip}/win");
                var xml = XDocument.Parse(xmlString);

                var root = xml.Root;

                // Получаем имя устройства
                string name = root.Element("ds")?.Value;

                // Если имени нет — вернуть "WLED"
                return string.IsNullOrWhiteSpace(name) ? "WLED" : name;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ошибка при получении имени с {ip}: {ex.Message}");
                return null; // при ошибке вернуть Null
            }
        }
        
        public class SimpleCard
        {
            public string Title { get; set; }
        }

        private async void Add_Clk(object sender, RoutedEventArgs e)
        {
            string ip = IpInput.Text.Replace(',', '.').Trim();

            // Проверка валидности IP
            if (!System.Net.IPAddress.TryParse(ip, out _))
            {
                MessageBox.Show($"{InvalidIP}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (ip.Length > 6 && !cards.Any(c => c.Title == ip))
            {
                
                // Получаем имя устройства через WLED API
                string name = await GetNameFromDevice(ip);
                if (string.IsNullOrEmpty(name))
                {
                    MessageBox.Show($"{ip} {ErrorMessage}.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Добавляем в UI
                cards.Add(new SimpleCard
                {
                    Title = $"{name} [{ip}]"
                });

                // Сохраняем в файл
                IPManager.SaveIp(ip);
            }
        }

        private void DeleteCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is SimpleCard card)
            {
                // Удаляем из UI
                cards.Remove(card);

                // Удаляем из файла
                IPManager.DeleteIp(card.Title);
            }
        }
    }
}
