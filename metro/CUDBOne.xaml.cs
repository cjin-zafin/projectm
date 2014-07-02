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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace metro
{
    /// <summary>
    /// Interaction logic for CUDBOne.xaml
    /// </summary>
    public partial class CUDBOne : UserControl
    {
        public CUDBOne()
        {
            InitializeComponent();
        }

        private void analyzeButton_click(object sender, RoutedEventArgs e)
        {
            HlrFileProcess hlrFileProcess = new HlrFileProcess();
            LoadDataSet loadDataSet = hlrFileProcess.processHLRLogFile("F:\\ProjectMTest\\log\\2014-06-26-12-CUDB01.txt");
        }
    }
}
