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
    /// <summary>
    /// Interaction logic for HSS.xaml
    /// </summary>
    /// 
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

    public partial class HSS : UserControl
    {
        public HSS()
        {
            InitializeComponent();
        }

        private void analyzeButton_Click(object sender, RoutedEventArgs e)
        {
            HSSProcessor hsProcessor = new HSSProcessor();
            String file = "F:\\ProjectMTest\\log\\HSS\\A20140625.0000-0015_MIYHSS01FE01BER_HSS-ESM";
            List<String> files = new List<string>();
            files.Add(file);
            HssDataSet hss = hsProcessor.processHssFileSet(files);

            List<FirstGridData> firstList = new List<FirstGridData>();
            FirstGridData fgd = new FirstGridData();
            fgd.cpu = hss.maxCpuLoad;
            fgd.mem = hss.MaxMemUsage;

        }
    }
}
