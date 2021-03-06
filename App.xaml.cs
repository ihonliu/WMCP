﻿//    WMCP Windows media control panel
//    Copyright (c) 2020 - 2020 Ihon Liu
//    
//    This program is free software: you can redistribute it and/or modify
//    it under the terms of the GNU General Public License as published by
//    the Free Software Foundation, either version 3 of the License, or
//    (at your option) any later version.
//
//    This program is distributed in the hope that it will be useful,
//    but WITHOUT ANY WARRANTY; without even the implied warranty of
//    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//    GNU General Public License for more details.
//
//    You should have received a copy of the GNU General Public License
//    along with this program.  If not, see <https://www.gnu.org/licenses/>.
using Hardcodet.Wpf.TaskbarNotification;
using System.Windows;
using WMCP.Libs;

namespace WMCP {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
        private TaskbarIcon notifyIcon;

        protected override void OnStartup(StartupEventArgs e) {
            base.OnStartup(e);

            //create the notifyicon (it's a resource declared in NotifyIconResources.xaml
            notifyIcon = (TaskbarIcon)FindResource("TrayIcon");
            notifyIcon.Icon = TrayIconFontConstructor.GenerateFontIcon();
            notifyIcon.Visibility = Visibility.Visible;
        }

        protected override void OnExit(ExitEventArgs e) {
            notifyIcon.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
