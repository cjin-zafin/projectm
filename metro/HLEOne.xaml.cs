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
using System.ComponentModel;

namespace metro
{
    public partial class HLEOne : UserControl
    {
        private void INotifyPropertyChanged(string p)
        {
        }


        public HLEOne()
        {
            InitializeComponent();
            List<GAY> Items = new List<GAY>();
            GAY gay = new GAY();
            gay.CpuLoad = "41";
            gay.SaxOverFlowCount = "15";
            gay.SpxCPULoad = "551";
            gay.C7sl1 = "哈哈";
            Items.Add(gay);

            loadDataGrid.ItemsSource = Items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        public class GAY
        {
            public string CpuLoad { get; set; }
            public string SpxCPULoad { get; set; }
            public string SaxOverFlowCount { get; set; }
            public string C7sl1 { get; set; }
        }

        public TextBox getLogAddressBox()
        {
            return logAddress;
        }
    }
}
