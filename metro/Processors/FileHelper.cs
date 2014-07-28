using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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
            string[] directories = Directory.GetDirectories(logPathText);
            List<string> directoryList = directories.ToList();

            foreach (string directory in directoryList)
            {
                searchFileAndPopulate(directory, listBox, nameAddressMap, searchString);
            }

            DirectoryInfo di = new DirectoryInfo(logPathText);
            FileSystemInfo[] files = di.GetFileSystemInfos();
            var orderedFiles = files.OrderByDescending(f => f.LastWriteTime);

            List<String> fileList = new List<string>();
            foreach (FileSystemInfo fi in orderedFiles)
            {
                fileList.Add(fi.FullName);
            }

            if (fileList.Count != 0)
            {
                addThings(fileList, listBox, nameAddressMap, searchString);
            }
        }

        private static void addThings(List<String> fileList, System.Windows.Controls.ListBox listBox, Hashtable nameAddressMap, String searchString)
        {
            if (!searchString.Contains("HSS"))
            {
                foreach (String file in fileList)
                {
                    if (file.Contains(searchString))
                    {
                        FileInfo fileInfo = new FileInfo(file);

                        String fileName = fileInfo.Name;

                        nameAddressMap.Add(fileName, file);
                        ListBoxItem item = new ListBoxItem();
                        item.ToolTip = file;
                        item.Content = fileName;

                        listBox.Items.Add(item);
                    }
                }
            }
            else
            {
                List<string> hssList = new List<string>();
                foreach (string file in fileList)
                {
                    if (file.Contains(searchString) && File.Exists(file))
                    {
                        hssList.Add(file);
                    }
                }

                List<string> dateTags = new List<string>();
                foreach (string file in hssList)
                {
                    FileInfo fileInfo = new FileInfo(file);

                    string[] tags = fileInfo.Name.Split('_');
                    if (tags[0] != null)
                    {
                        string dateTag = tags[0];
                        if(!HSSDataProvider.hssFileListMap.ContainsKey(dateTag)){
                            dateTags.Add(dateTag);
                            List<string> newList = new List<string>();
                            newList.Add(file);

                            HSSDataProvider.hssFileListMap.Add(dateTag, newList);

                            //Debug.WriteLine(dateTag + " " + "\n");
                        }
                        else
                        {
                            HSSDataProvider.hssFileListMap[dateTag].Add(file);
                        }
                    }
                }

                foreach (string dateTag in dateTags)
                {
                    List<string> valueList = HSSDataProvider.hssFileListMap[dateTag];

                    if (valueList.Count > 3)
                    {
                        List<char> displayTag = dateTag.ToList();

                        string year = string.Join("", displayTag.GetRange(1, 4).ToArray()) + "年";
                        string month = string.Join("", displayTag.GetRange(5, 2).ToArray()) + "月";
                        string day = string.Join("", displayTag.GetRange(7, 2).ToArray()) + "日";
                        string time = string.Join("", displayTag.GetRange(10, 9).ToArray());

                        string displayTime = year + "-" + month + "-" + day + "-" + time + "时";


                        nameAddressMap.Add(displayTime, dateTag);

                        ListBoxItem item = new ListBoxItem();
                        item.Content = displayTime;

                        listBox.Items.Add(item);
                    }
                }
            }

        }
    }
}
