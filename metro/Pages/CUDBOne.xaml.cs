using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using metro.Processors;

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

        protected class FirstGridData
        {
            public string HlrUserCount { get; set; }
            public string HlrActiveUserCount { get; set; }
            public string GpsUserCount { get; set; }
            public string AuthInfoCount { get; set; }
        }

        protected class SecontGridData
        {
            public String TwoGAuthInfoCount { get; set; }
            public String ThreeGAuthInfoCount { get; set; }
            public String fourGUserCount { get; set; }
        }

        private void analyzeButton_click(object sender, RoutedEventArgs e)
        {
            CudbFileProcess cudbFileProcess = new CudbFileProcess();
            CudbDataSet loadDataSet = cudbFileProcess.processCudbLogFile("F:\\ProjectMTest\\log\\2014-06-26-12-CUDB01.txt");

            List<FirstGridData> firstData = new List<FirstGridData>();
            FirstGridData fd = new FirstGridData();
            fd.HlrUserCount = loadDataSet.hlrUserCount;
            fd.HlrActiveUserCount = loadDataSet.hlrUserActCount;
            fd.GpsUserCount = loadDataSet.gprsUserCount;
            fd.AuthInfoCount = loadDataSet.authInfoCount;

            firstData.Add(fd);
            firstGrid.ItemsSource = firstData;

            List<SecontGridData> secondData = new List<SecontGridData>();
            SecontGridData sd = new SecontGridData();
            sd.fourGUserCount = loadDataSet.fourGUserCount;
            sd.ThreeGAuthInfoCount = loadDataSet.threeGAuthInfoCount;
            sd.TwoGAuthInfoCount = loadDataSet.twoGAuthInfoCount;

            secondData.Add(sd);
            secondGrid.ItemsSource = secondData;
        }
    }
}
