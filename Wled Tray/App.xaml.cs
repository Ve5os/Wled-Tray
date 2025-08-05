using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Wled_Tray
{
    public partial class App : System.Windows.Application
    {
        NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
         
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            trayIcon = new NotifyIcon();
            trayIcon.Icon = new Icon("wled.ico"); // Добавь иконку в корень проекта
            trayIcon.Visible = true;
            trayIcon.Text = "WLED Контроллер";
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;

            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Настройки", null, OnSettingsClicked);
            trayMenu.Items.Add("Выход", null, OnExitClicked);

            trayIcon.ContextMenuStrip = trayMenu;

            trayIcon.MouseClick += TrayIcon_Click;

            
        }
        private void OnExitClicked(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
        private void OnSettingsClicked(object sender, EventArgs e)
        {
            var mainWindow = System.Windows.Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

            if (mainWindow == null)
            {
                mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                if (!mainWindow.IsVisible)
                    mainWindow.Show();

                mainWindow.Activate();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            base.OnExit(e);
        }
        private PopupWindow popupWindow = null;

        private void TrayIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (popupWindow == null)
                {
                    popupWindow = new PopupWindow();
                    popupWindow.Closed += (s, args) => popupWindow = null;
                }
                try
                {
                    if (popupWindow.IsVisible)
                    {
                        popupWindow.Close();
                    }
                    else
                    {
                        popupWindow.Show();
                        popupWindow.AnimateIn();
                        popupWindow.Activate();
                    }
                }
                catch (InvalidOperationException ex)
                {
                    Debug.WriteLine($"[WLED Popup Error]: {ex.Message}");
                }
            }
        }

    }
}
