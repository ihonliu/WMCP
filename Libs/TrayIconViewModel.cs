//    WMCP Windows media control panel
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
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace WMCP.Libs {
    public class TrayIconViewModel {
        /// <summary>
        /// Shows a window, if none is already open.
        /// </summary>
        public ICommand ShowWindowCommand {
            get {
                return new DelegateCommand {
                    CanExecuteFunc = () => Application.Current.MainWindow == null,
                    CommandAction = () => {
                        if (Application.Current.MainWindow == null) Application.Current.MainWindow = new MainWindow();
                        Application.Current.MainWindow.Show();
                        Application.Current.MainWindow.Activate();
                        Application.Current.MainWindow.Focus();
                    }
                };
            }
        }

        /// <summary>
        /// Hides the main window. This command is only enabled if a window is open.
        /// </summary>
        public ICommand HideWindowCommand {
            get {
                return new DelegateCommand {
                    CommandAction = () => {
                        Application.Current.MainWindow.Close();
                    },
                    CanExecuteFunc = () => Application.Current.MainWindow != null
                };
            }
        }

        public ICommand ShowHideCommand {
            get {
                return new DelegateCommand {
                    CommandAction = () => {
                        if (Application.Current.MainWindow == null) {
                            Application.Current.MainWindow = new MainWindow();
                            Application.Current.MainWindow.Show();
                        }
                        else {
                            if (Application.Current.MainWindow.Visibility != Visibility.Visible)
                                Application.Current.MainWindow.Show();
                            else
                                Application.Current.MainWindow.Hide();
                        }
                    },
                    CanExecuteFunc = () => { return true; },
                };
            }
        }

        /// <summary>
        /// Shuts down the application.
        /// </summary>
        public ICommand ExitApplicationCommand {
            get {
                return new DelegateCommand { CommandAction = () => Application.Current.Shutdown() };
            }
        }
    }

    /// <summary>
    /// Simplistic delegate command for the demo.
    /// </summary>
    public class DelegateCommand : ICommand {
        public Action CommandAction { get; set; }
        public Func<bool> CanExecuteFunc { get; set; }

        public void Execute(object parameter) {
            CommandAction();
        }

        public bool CanExecute(object parameter) {
            return CanExecuteFunc == null || CanExecuteFunc();
        }

        public event EventHandler CanExecuteChanged {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
