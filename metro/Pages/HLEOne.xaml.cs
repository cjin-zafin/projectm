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
using System.Diagnostics;
using metro.Processors;

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
        }

        private void analyzeButton_click(object sender, RoutedEventArgs e)
        {
            HSSProcessor hsProcessor = new HSSProcessor();
            String file = "F:\\ProjectMTest\\log\\HSS\\A20140625.0000-0015_MIYHSS01FE01BER_SS7Statistics";
            List<String> files = new List<string>();
            files.Add(file);
            hsProcessor.processHssFileSet(files);


            HlrFileProcess hlrFileProcess = new HlrFileProcess();
            LoadDataSet loadDataSet = hlrFileProcess.processHLRLogFile("F:\\ProjectMTest\\log\\2014-06-30-10-HLRFE01ERROR.txt");
            //Debug.WriteLine("cpu load = " + loadDataSet.cpuLoad + "\n");
            //Debug.WriteLine("SPX CPU LOAD = " + loadDataSet.spxCpuLoad + "\n");

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

        protected class FirstGridData
        {
            public string CpuLoad { get; set; }
            public string SpxCPULoad { get; set; }
            public string SaxOverFlowCount { get; set; }
            public string C7sl1 { get; set; }
        }

        protected class SecontGridData
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
    }
}
