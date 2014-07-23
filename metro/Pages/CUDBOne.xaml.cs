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
using System.Collections;

namespace metro
{
    public partial class CUDBOne : UserControl
    {
        private Hashtable nameAddressMap = new Hashtable();
        private System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();

        public CUDBOne()
        {
            InitializeComponent();
        }

        private void on_browse(object sender, RoutedEventArgs e)
        {
            string folderName = "";

            System.Windows.Forms.DialogResult result = browse.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                folderName = browse.SelectedPath;
                logAddress.Text = folderName;
            }
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
            String fileName = FileHelper.getFileName(listBox.SelectedValue);
            var item = nameAddressMap[fileName];
            String filePath = item.ToString();

            CudbFileProcess cudbFileProcess = new CudbFileProcess();
            CudbDataSet loadDataSet = cudbFileProcess.processCudbLogFile(filePath);

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

        private void CUDB1_Loaded(object sender, RoutedEventArgs e)
        {
            logAddress.Text = SettingsData.location;
            analyzeButton.IsEnabled = false;
            onSearch();
        }

        private void On_List_Selection(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems.Count != 0)
            {
                analyzeButton.IsEnabled = true;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            onSearch();
        }

        private void onSearch()
        {
            String logPathText = logAddress.Text;

            FileHelper.searchFileAndPopulate(logPathText, listBox, nameAddressMap, "CUDB01");
        }
    }
}
