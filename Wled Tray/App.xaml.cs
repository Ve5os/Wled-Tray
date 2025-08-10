using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Wled_Tray.Properties;

namespace Wled_Tray
{
    public partial class App : System.Windows.Application
    {
        NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        private PopupWindow popupWindow = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IPManager.Ini();
            trayGenerate();
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown; 
        }

        private void trayGenerate()
        {
            IniFile MyIni = new IniFile(JustPath.GetIni());
            trayIcon = new NotifyIcon();

            switch (MyIni.Read("Icon"))
            {
                case "1":
                    trayIcon.Icon = Wled_Tray.Properties.Resources.ledstrip;
                    break;
                case "2":
                    trayIcon.Icon = Wled_Tray.Properties.Resources.wled;
                    break;
                case "3":
                    trayIcon.Icon = Wled_Tray.Properties.Resources.zledstrips;
                    break;
                case "4":
                    trayIcon.Icon = Wled_Tray.Properties.Resources.WledTray;
                    break;
                default:
                    trayIcon.Icon = Wled_Tray.Properties.Resources.wled;
                    break;
            }

            trayIcon.Visible = true;
            trayIcon.Text = "Wled Tray";
            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Настройки", null, OnSettingsClicked_tray);
            trayMenu.Items.Add("Выход", null, OnExitClicked_tray);

            trayIcon.ContextMenuStrip = trayMenu;

            trayIcon.MouseClick += TrayIcon_Click;
        }
     
        #region Tray Buttons
        private void OnExitClicked_tray(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            System.Windows.Application.Current.Shutdown();
        }
        private void OnSettingsClicked_tray(object sender, EventArgs e)
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
        #endregion
    
        protected override void OnExit(ExitEventArgs e)
        {
            trayIcon.Visible = false;
            trayIcon.Dispose();
            base.OnExit(e);
        }


     
    }
}
