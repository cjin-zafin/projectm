using metro.Processors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public partial class HSSTwo : UserControl
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
            public String SCTPRate { get; set; }
            public String authInfo { get; set; }
            public String extDbModi { get; set; }
            public String extDbSearch { get; set; }
        }

        private Hashtable nameAddressMap = new Hashtable();
        private System.Windows.Forms.FolderBrowserDialog browse = new System.Windows.Forms.FolderBrowserDialog();
        private FileSystemWatcher fsWatcher = new FileSystemWatcher();

        public HSSTwo()
        {
            InitializeComponent();
        }

        public void OnChanged(object sender, FileSystemEventArgs e)
        {
            this.Dispatcher.Invoke(new Action(delegate
            {
                onSearch();
            }));
        }

        public void OnError(object sender, ErrorEventArgs e)
        {
            //do nothing
        }

        private void on_browse(object sender, RoutedEventArgs e)
        {
            string folderName = "";

            System.Windows.Forms.DialogResult result = browse.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                folderName = browse.SelectedPath;
                logAddress.Text = folderName;
                fsWatcher.Path = folderName;
            }
        }


        private void onSearch()
        {
            String logPathText = logAddress.Text;

            if (Directory.Exists(logPathText))
            {
                listBox.Items.Clear();
                nameAddressMap.Clear();
                HSSDataProvider.hssFileListMap.Clear();
                FileHelper.searchFileAndPopulate(logPathText, listBox, nameAddressMap, "HSS01FE02");

                if (!listBox.Items.IsEmpty)
                {
                    listBox.SelectedIndex = 0;
                    analyzeNow();
                }
            }
        }

        private void On_List_Selection(object sender, RoutedEventArgs e)
        {
            if (listBox.SelectedItems.Count != 0)
            {
                analyzeButton.IsEnabled = true;
            }
        }

        private void On_Load(object sender, RoutedEventArgs e)
        {
            logAddress.Text = SettingsData.location;
            analyzeButton.IsEnabled = false;
            onSearch();

            if (Directory.Exists(logAddress.Text))
            {
                fsWatcher.Path = logAddress.Text;
                fsWatcher.IncludeSubdirectories = true;

                fsWatcher.NotifyFilter = NotifyFilters.Attributes |
                                                     NotifyFilters.CreationTime |
                                                     NotifyFilters.DirectoryName |
                                                     NotifyFilters.FileName |
                                                     NotifyFilters.LastAccess |
                                                     NotifyFilters.LastWrite |
                                                     NotifyFilters.Security |
                                                     NotifyFilters.Size;

                fsWatcher.EnableRaisingEvents = true;
                // Raise Event handlers.
                fsWatcher.Changed += new FileSystemEventHandler(OnChanged);
                fsWatcher.Created += new FileSystemEventHandler(OnChanged);
                fsWatcher.Deleted += new FileSystemEventHandler(OnChanged);
                fsWatcher.Renamed += new RenamedEventHandler(OnChanged);
                fsWatcher.Error += new ErrorEventHandler(OnError);
            }
        }

        private void analyzeButton_click(object sender, RoutedEventArgs e)
        {
            analyzeNow();
        }

        private void analyzeNow()
        {
            HSSProcessor hsProcessor = new HSSProcessor();
            String curDisplay = FileHelper.getFileName(listBox.SelectedValue);

            String dateTag = nameAddressMap[curDisplay].ToString();

            if (HSSDataProvider.hssFileListMap.ContainsKey(dateTag))
            {
                List<string> files = HSSDataProvider.hssFileListMap[dateTag];

                HssDataSet hss = hsProcessor.processHssFileSet(files);

                List<FirstGridData> firstList = new List<FirstGridData>();
                FirstGridData fgd = new FirstGridData();
                fgd.cpu = hss.maxCpuLoad;
                if (fgd.cpu == null)
                {
                    fgd.cpu = "N/A";
                }
                fgd.mem = hss.MaxMemUsage;
                if (fgd.mem == null)
                {
                    fgd.mem = "N/A";
                }
                fgd.s6aUpdate = hss.updateLocation;
                if (fgd.s6aUpdate == null)
                {
                    fgd.s6aUpdate = "N/A";
                }
                fgd.s6aCancel = hss.cancelLocation;
                if (fgd.s6aCancel == null)
                {
                    fgd.s6aCancel = "N/A";
                }

                firstList.Add(fgd);
                firstGrid.ItemsSource = firstList;

                List<SecontGridData> secondList = new List<SecontGridData>();
                SecontGridData sgd = new SecontGridData();

                sgd.SCTPRate = hss.sctpResendRate;
                if (sgd.SCTPRate == null)
                {
                    sgd.SCTPRate = "N/A";
                }
                sgd.authInfo = hss.sentAuthenticationInfo;
                if (sgd.authInfo == null)
                {
                    sgd.authInfo = "N/A";
                }
                sgd.extDbModi = hss.exDbModify;
                if (sgd.extDbModi == null)
                {
                    sgd.extDbModi = "N/A";
                }
                sgd.extDbSearch = hss.exDbSearch;
                if (sgd.extDbSearch == null)
                {
                    sgd.extDbSearch = "N/A";
                }

                secondList.Add(sgd);
                secondGrid.ItemsSource = secondList;
            }
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(logAddress.Text))
            {
                fsWatcher.Path = logAddress.Text;
                SettingsData.location = logAddress.Text;
                onSearch();
            }
        }
    }
}
