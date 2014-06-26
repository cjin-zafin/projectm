using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

namespace metro
{
    public partial class MainPage : UserControl
    {
        

        public MainPage()
        {


            InitializeComponent();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<LoadData> LoadDataSet = new ObservableCollection<LoadData>();



            LoadDataSet.Add(new LoadData("1", "1", "1", "1"));

            loadDataGrid.ItemsSource = LoadDataSet;

            
        }


        private void LoadData_Loaded(object sender, RoutedEventArgs e)
        {
            ObservableCollection<LoadData> LoadDataSet = new ObservableCollection<LoadData>();
            //var items = new List<LoadData>();

           LoadData load = new LoadData("1", "1", "1", "1");


           LoadDataSet.Add(load);
          //  items.Add(load);

           loadDataGrid.ItemsSource = LoadDataSet;
        }
    }
}
