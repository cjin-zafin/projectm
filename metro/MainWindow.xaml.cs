using System;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace metro
{
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
