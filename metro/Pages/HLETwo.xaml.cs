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
using System.ComponentModel;
using metro.Processors;
using System.Collections;

namespace metro
{

    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class HLETwo : UserControl
    {
        private Hashtable nameAddressMap = new Hashtable();
        private System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();

        private void INotifyPropertyChanged(string p)
        {
        }


        public HLETwo()
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

        private void analyzeButton_click(object sender, RoutedEventArgs e)
        {
            String fileName = FileHelper.getFileName(listBox.SelectedValue);

            HlrFileProcess hlrFileProcess = new HlrFileProcess();
            var item = nameAddressMap[fileName];

            if (item != null)
            {
                String filePath = item.ToString();

                LoadDataSet loadDataSet = hlrFileProcess.processHLRLogFile(filePath);

                List<FirstGridData> Items = new List<FirstGridData>();

                populateSecondGrid(loadDataSet);


                int rowCount = 0;


                if (loadDataSet.saeOverflowList.Count > loadDataSet.c7sl1List.Count)
                {
                    rowCount = loadDataSet.saeOverflowList.Count;
                }
                else
                {
                    rowCount = loadDataSet.c7sl1List.Count;
                }

                if (rowCount != 0)
                {
                    loadDataGrid.Height = 167;
                    for (int i = 0; i < rowCount; i++)
                    {
                        FirstGridData firstData = new FirstGridData();
                        firstData.CpuLoad = loadDataSet.cpuLoad;
                        firstData.SpxCPULoad = loadDataSet.spxCpuLoad;

                        if (loadDataSet.saeOverflowList.Count == 0)
                        {
                            firstData.SaxOverFlowCount = "PASS";
                        }
                        else if (i < loadDataSet.saeOverflowList.Count)
                        {
                            String saeData = loadDataSet.saeOverflowList[i].saeMachineName + " = " + loadDataSet.saeOverflowList[i].overflowNumber;
                            firstData.SaxOverFlowCount = saeData;
                        }

                        if (loadDataSet.c7sl1List.Count == 0)
                        {
                            firstData.C7sl1 = "PASS";
                        }
                        else if (i < loadDataSet.c7sl1List.Count)
                        {
                            String c7Data = loadDataSet.c7sl1List[i].c7Name + " = " + loadDataSet.c7sl1List[i].flippingNumber;
                            firstData.C7sl1 = c7Data;
                        }

                        Items.Add(firstData);
                    }
                }
                else
                {
                    loadDataGrid.Height = 80;

                    FirstGridData firstData = new FirstGridData();
                    firstData.CpuLoad = loadDataSet.cpuLoad;
                    firstData.SpxCPULoad = loadDataSet.spxCpuLoad;

                    if (loadDataSet.saeOverflowList.Count == 0)
                    {
                        firstData.SaxOverFlowCount = "PASS";
                    }

                    if (loadDataSet.c7sl1List.Count == 0)
                    {
                        firstData.C7sl1 = "PASS";
                    }

                    Items.Add(firstData);
                }

                for (int i = 0; i < Items.Count; i++)
                {
                    if (i > 0)
                    {
                        Items[i].SpxCPULoad = "";
                        Items[i].CpuLoad = "";
                    }
                }


                loadDataGrid.ItemsSource = Items;
            }
        }

        public class FirstGridData
        {
            public string CpuLoad { get; set; }
            public string SpxCPULoad { get; set; }
            public string SaxOverFlowCount { get; set; }
            public string C7sl1 { get; set; }
        }

        public class SecontGridData
        {
            public String threeDim { get; set; }
            public String fiveDim { get; set; }
        }

        public TextBox getLogAddressBox()
        {
            return logAddress;
        }

        private void populateSecondGrid(LoadDataSet loadDataSet)
        {
            List<SecontGridData> secondGridData = new List<SecontGridData>();
            SecontGridData data = new SecontGridData();

            LastAuth lastAuth = loadDataSet.lastAuth;

            String dim3 = lastAuth.threeDRequestCount + "/" + lastAuth.threeDGenerated;
            data.threeDim = dim3;

            String dim5 = lastAuth.fiveDRequestCount + "/" + lastAuth.fiveDGenerated;
            data.fiveDim = dim5;

            secondGridData.Add(data);

            secondGrid.ItemsSource = secondGridData;
        }

        private void HLETwo_Loaded(object sender, RoutedEventArgs e)
        {
            logAddress.Text = SettingsData.location;
            analyzeButton.IsEnabled = false;
            onSearch();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            onSearch();
        }

        private void onSearch()
        {
            String logPathText = logAddress.Text;

            FileHelper.searchFileAndPopulate(logPathText, listBox, nameAddressMap, "HLRFE02");
        }

        private void On_List_Selection(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems.Count != 0)
            {
                analyzeButton.IsEnabled = true;
            }
        }
    }
}
