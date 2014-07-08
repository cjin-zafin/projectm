using System;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Controls;
using System.Collections.Generic;
using System.ComponentModel;

namespace metro
{
    public partial class MainWindow : MetroWindow
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public Boolean PASSED
        {
            get { return passed; }
            set
            {
                passed = value;
                if(PropertyChanged != null)
                {
                    tab1.IsEnabled = true;
                }

            }
        }
        public static bool passed;
        public MainWindow()
        {
            InitializeComponent();
            tab1.IsEnabled = false;
            tab1.IsEnabled = true;
        }


        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            SettingsData.location = "F:\\ProjectMTest\\log";
        }
    }
}
