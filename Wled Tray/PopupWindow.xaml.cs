    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;
    using Wled_Tray;
    using System.Windows.Media.Imaging;
    using System.Windows.Shapes;
    using System.Windows.Media.Animation;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Net;
    using System.Xml.Linq;

    namespace Wled_Tray
    {
        public partial class PopupWindow : Window
        {
            internal System.Windows.Threading.DispatcherTimer hideTimer;
            private static int AnimationMS = 300;
            public PopupWindow()
            {
            
                InitializeComponent();
                hideTimer = new System.Windows.Threading.DispatcherTimer();
                hideTimer.Interval = TimeSpan.FromMilliseconds(50); // задержка перед скрытием
                hideTimer.Tick += (s, e) =>
                {
                    hideTimer.Stop();
                   this.Close();
                };
                GenerateCard("192.168.1.110");
                GenerateCard("192.168.1.111");
                GenerateCard("192.168.1.113");
            }

            public async void GenerateCard(string ip)
            {
                var card = await GetCardFromDevice(ip); // ⬅️ Мы получаем данные с WLED
                if (card == null)
                {
            
                    return;
                }

                if (CardsList.ItemsSource is List<Card> list)
                {
                    list.Add(card);
                    CardsList.ItemsSource = null;
                    CardsList.ItemsSource = list;
                }
                else
                {
                    CardsList.ItemsSource = new List<Card> { card };
                }
            }


            public async Task<Card> GetCardFromDevice(string ip)
            {
                var client = new HttpClient();
                try
                {
                    var xmlString = await client.GetStringAsync($"http://{ip}/win");
                    var xml = XDocument.Parse(xmlString);

                    var root = xml.Root;

                    int brightness = int.Parse(root.Element("ac")?.Value ?? "0");

                    var clElements = root.Elements("cl").ToList();
                    byte r = byte.Parse(clElements[0].Value);
                    byte g = byte.Parse(clElements[1].Value);
                    byte b = byte.Parse(clElements[2].Value);

                    int hue = ColorExChange.RgbToHue255(r, g, b);
                    string name = root.Element("ds")?.Value ?? ip;

                    return new Card
                    {
                        Title = name +$" [{ip}]",
                        Brightness = brightness,
                        Saturation = 100, // Saturation — можно заглушку
                        Color = hue,
                        IpAddress = ip
                    };
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка при получении данных с {ip}: {ex.Message}");
                    return null;
                }
            }


            private void Slider_OnValueChanged(object sender, System.Windows.Input.MouseButtonEventArgs e)
            {
                if (sender is Slider slider && slider.DataContext is Card card)
                {
                    ApplyCardSettings(card);
                }
            }

            private void ApplyCardSettings(Card card)
            {
                var color = ColorExChange.Hue255ToRgb(card.Color);
                string url = $"http://{card.IpAddress}/win&A={card.Brightness}&SA={card.Saturation}&R={color.R}&G={color.G}&B={color.B}";
                Debug.WriteLine(url);
                try
                {
                    using (var client = new WebClient())
                    {
                        string content = client.DownloadString(url);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Ошибка отправки данных на WLED: {ex.Message}");
                }
            }
            private void HueSlider_Loaded(object sender, RoutedEventArgs e)
            {
                if (sender is Slider slider)
                {
                    UpdateHueTrack(slider);

                    slider.ValueChanged += (s, args) =>
                    {
                        UpdateHueTrack(slider);
                    };
                }
            }

            private void UpdateHueTrack(Slider slider)
            {
                var hueValue = (int)slider.Value;
                var (r, g, b) = ColorExChange.Hue255ToRgb(hueValue);

                if (slider.Template.FindName("HueBar", slider) is Rectangle hueBar)
                {
                    hueBar.Fill = new SolidColorBrush(Color.FromRgb(r, g, b));
                }
            }
       
            private void Settings_Click(object sender, RoutedEventArgs e)
            {
                var mainWindow = Application.Current.Windows
                                   .OfType<MainWindow>()
                                   .FirstOrDefault();

                if (mainWindow == null)
                {
                    mainWindow = new MainWindow();
                    mainWindow.Show();
                }
                else
                {
                    if (!mainWindow.IsVisible)
                        mainWindow.Show();
                }
                mainWindow.Activate();
            }
            public void AnimateIn()

            {   // Позиция окна — в правом нижнем углу
                var screen = SystemParameters.WorkArea;

                double targetLeft = screen.Right - this.Width - 10;
                double startLeft = screen.Right;

                this.Left = startLeft;
                this.Top = screen.Bottom - this.Height - 10;

                // Начинаем с полной прозрачности
                this.Opacity = 0;

                // Анимация сдвига окна
                var moveAnimation = new DoubleAnimation
                {
                    From = startLeft,
                    To = targetLeft,
                    Duration = TimeSpan.FromMilliseconds(AnimationMS),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                // Анимация прозрачности (fade-in)
                var fadeAnimation = new DoubleAnimation
                {
                    From = 0,
                    To = 1,
                    Duration = TimeSpan.FromMilliseconds(AnimationMS),
                    EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
                };

                // Запускаем обе анимации одновременно
                this.BeginAnimation(Window.LeftProperty, moveAnimation);
                this.BeginAnimation(Window.OpacityProperty, fadeAnimation);
            }

            protected override void OnDeactivated(EventArgs e)
            {
                base.OnDeactivated(e);

                // Запускаем таймер на скрытие, вместо мгновенного hide
                if (!hideTimer.IsEnabled)
                    hideTimer.Start();
            }



        }
    }
