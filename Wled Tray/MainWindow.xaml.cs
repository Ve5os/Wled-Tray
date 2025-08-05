using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Wled_Tray
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LanguagePackage languagePackage;
        public MainWindow()
        {
            InitializeComponent();
            this.Hide();
            CardList.ItemsSource = cards;
            languagePackage = new LanguagePackage(this, null); // передаём себя
            languagePackage.Update();

        }
        private void DragBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
            {
                try { this.DragMove(); } catch { }
            }
        }
        private void Rus_Clk(object sender, RoutedEventArgs e)
        {
            languagePackage.Write("ru");
        }
        private void Eng_Clk(object sender, RoutedEventArgs e)
        {
            languagePackage.Write("en");
        }
        private void CloseBTN_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
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
        public class SimpleCard
        {
            public string Title { get; set; }
        }

        private ObservableCollection<SimpleCard> cards = new ObservableCollection<SimpleCard>();


        private void Add_Clk(object sender, RoutedEventArgs e)
        {
            if (IpInput.Text.Length > 6)
            {
                cards.Add(new SimpleCard { Title = $" WLED Device Address: {IpInput.Text.Replace(',', '.')}" });
            }
        }

        private void DeleteCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is SimpleCard card)
            {
                cards.Remove(card);
            }
        }

    }
}
