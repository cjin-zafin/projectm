using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace metro.Processors
{
    class FileHelper
    {
        internal static string getFileName(object p)
        {
            if (p != null)
            {
                String print = p.ToString();
                string[] group = print.Split(':');

                String fileName = group[1].Trim();

                return fileName;
            }
            else
            {
                return "C:\\";
            }
        }

        internal static void searchFileAndPopulate(string logPathText, System.Windows.Controls.ListBox listBox, Hashtable nameAddressMap, String searchString)
        {
            if (Directory.Exists(logPathText))
            {
                listBox.Items.Clear();
                nameAddressMap.Clear();
                string[] directories = Directory.GetDirectories(logPathText);
                List<string> directoryList = directories.ToList();
                directoryList.Add(logPathText);

                foreach (string directory in directoryList)
                {
                    string[] fileList = Directory.GetFiles(directory);

                    if (fileList.Length != 0)
                    {
                        addThings(fileList, listBox, nameAddressMap, searchString);
                    }
                }
            }
        }

        private static void addThings(string[] fileList, System.Windows.Controls.ListBox listBox, Hashtable nameAddressMap, String searchString)
        {
            for (int i = fileList.Length - 1; i != 0; i--)
            {
                String file = fileList[i];
                if (file.Contains(searchString))
                {
                    FileInfo fileInfo = new FileInfo(file);

                    String fileName = fileInfo.Name;

                    nameAddressMap.Add(fileName, file);
                    ListBoxItem item = new ListBoxItem();
                    item.ToolTip = file;
                    item.Content = fileName;
                    // item.
                    //item.Selected(On_List_Selection);

                    listBox.Items.Add(item);
                }
            }
        }
    }
}
