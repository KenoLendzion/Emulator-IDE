using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EmulatorGUI.MVVM.Model;
using EmulatorGUI.MVVM.View;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Navigation;

namespace EmulatorGUI.MVVM.ViewModel
{
    internal class MainWindowViewModel : ObservableObject
    {
        public RelayCommand MoveWindowCommand { get; set; }
        public RelayCommand MinimizeWindowCommand { get; set; }
        public RelayCommand MaximizeWindowCommand { get; set; }
        public RelayCommand ShutDownWindowCommand { get; set; }
        public RelayCommand OpenFileCommand { get; set; }
        public RelayCommand RunCommand { get; set; }
        public RelayCommand AboutButtonCommand { get; set; }

        private ObservableCollection<FileTabItem> _fileTabControlItems;

        public ObservableCollection<FileTabItem> FileTabControlItems
        {
            get { return _fileTabControlItems; }
            set { _fileTabControlItems = value; }
        }

        public MainWindowViewModel()
        {
            FileTabControlItems = new ObservableCollection<FileTabItem>();
            MoveWindowCommand = new RelayCommand( Application.Current.MainWindow.DragMove);

            MinimizeWindowCommand = new RelayCommand(MinimizeWindow);
            MaximizeWindowCommand = new RelayCommand(MaximizeWindow);
            ShutDownWindowCommand = new RelayCommand(ShutDownWindow);
            OpenFileCommand = new RelayCommand(OpenFile);
            RunCommand = new RelayCommand(Run);
            AboutButtonCommand = new RelayCommand(AboutButton);
        }

        private void MinimizeWindow()
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void MaximizeWindow()
        {
            if( Application.Current.MainWindow.WindowState == WindowState.Maximized )
            {
                Application.Current.MainWindow.WindowState = WindowState.Normal;
            }
            else
            {
                Application.Current.MainWindow.WindowState = WindowState.Maximized;
            }
        }

        private void ShutDownWindow()
        {
            Application.Current.Shutdown();
        }

        private void OpenFile()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();

            byte[] filebytes = File.ReadAllBytes(openFileDialog.FileName);

            string content = BitConverter.ToString(filebytes).Replace("-","");

            for( int stringPostition = 0; stringPostition < content.Length; stringPostition+=4 )
            {
                content = content.Insert(stringPostition, "\n");
            }

            FileTabItem tabItem = new FileTabItem()
            {
                FileName = openFileDialog.SafeFileName,
                FullFilePath = openFileDialog.FileName,
                Content = content
            };

            FileTabControlItems.Add(tabItem);
        }

        private void Run()
        {

        }

        private void AboutButton()
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.Show();
        }
    }
}
