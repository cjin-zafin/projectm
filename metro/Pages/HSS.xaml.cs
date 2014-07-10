using metro.Processors;
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
    public partial class HSS : UserControl
    {

        protected class FirstGridData
        {
            public string cpu { get; set; }
            public string mem { get; set; }
            public string s6aUpdate { get; set; }
            public string s6aCancel { get; set; }
        }

        protected class SecontGridData
        {
            public String authInfo { get; set; }
            public String extDbModi { get; set; }
            public String extDbSearch { get; set; }
        }


        public HSS()
        {
            InitializeComponent();
        }

        private void analyzeButton_Click(object sender, RoutedEventArgs e)
        {
            HSSProcessor hsProcessor = new HSSProcessor();
            String file = "F:\\ProjectMTest\\log\\HSS\\A20140625.0000-0015_MIYHSS01FE01BER_HSS-ESM";
            String file1 = "F:\\ProjectMTest\\log\\HSS\\A20140625.0000-0015_MIYHSS01FE01BER_SS7Statistics";
            String file2 = "F:\\ProjectMTest\\log\\HSS\\A20140625.0000-0015_MIYHSS01FE01BER_PlatformMeasures";
            String file3 = "F:\\ProjectMTest\\log\\HSS\\A20140625.0000-0015_MIYHSS01FE01BER_IPv4_MeasurementJob";
            String file4 = "F:\\ProjectMTest\\log\\HSS\\A20140625.0000-0015_MIYHSS01FE01BER_Diameter_traffic_counters";
            String file5 = "F:\\ProjectMTest\\log\\HSS\\A20140625.0000-0015_MIYHSS01FE01BER_Diameter_error_counters";
            List<String> files = new List<string>();
            files.Add(file);
            files.Add(file1);
            files.Add(file2);
            files.Add(file3);
            files.Add(file4);
            files.Add(file5);

            HssDataSet hss = hsProcessor.processHssFileSet(files);

            List<FirstGridData> firstList = new List<FirstGridData>();
            FirstGridData fgd = new FirstGridData();
            fgd.cpu = hss.maxCpuLoad;
            fgd.mem = hss.MaxMemUsage;
            fgd.s6aUpdate = hss.updateLocation;
            fgd.s6aCancel = hss.cancelLocation;

            firstList.Add(fgd);
            firstGrid.ItemsSource = firstList;

            List<SecontGridData> secondList = new List<SecontGridData>();
            SecontGridData sgd = new SecontGridData();

            sgd.authInfo = hss.sentAuthenticationInfo;
            sgd.extDbModi = hss.exDbModify;
            sgd.extDbSearch = hss.exDbSearch;

            secondList.Add(sgd);
            secondGrid.ItemsSource = secondList;
        }
    }
}
