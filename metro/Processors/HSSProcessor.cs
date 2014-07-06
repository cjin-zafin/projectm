using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace metro.Processors
{
    public class HssDataSet
    {
        public String maxCpuLoad;
        public String MaxMemUsage;
        public String updateLocation;
        public String cancelLocation;
        public String sctpResendRate;
        public String sentAuthenticationInfo;
        public String exDbModify;
        public String exDbSearch;
    }

    class HSSProcessor
    {
        public HssDataSet processHssFileSet(List<String> filePathes)
        {
            HssDataSet hsData = new HssDataSet();

            foreach (String filePath in filePathes)
            {
                if (filePath.Contains("PlatformMeasures"))
                {
                    processPlatformMeasureFile(hsData, filePath);
                }
                else if (filePath.Contains("HSS-ESM"))
                {
                    processEsmFile(hsData, filePath);
                }
                else if (filePath.Contains("SS7Statistics"))
                {
                    processSs7File(hsData, filePath);
                }
            }
            return hsData;
        }

        private void processSs7File(HssDataSet hsData, string filePath)
        {
            if (File.Exists(filePath))
            {
                System.IO.StreamReader file =
                      new System.IO.StreamReader(filePath);


                List<String> lines = new List<string>();
                int sendLineNumber = 0;
                int receiveLineNumber = 0;
                int count = 0;

                string line;
                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                    if (line.Contains("tsp.sctp.data_chunk_received"))
                    {
                        receiveLineNumber = count;
                    }
                    else if (line.Contains("tsp.sctp.data_chunk_sent"))
                    {
                        sendLineNumber = count;
                    }

                    count++;
                }

                String sent = lines[sendLineNumber + 3];
                String received = lines[receiveLineNumber + 3];

                XmlReader sentReader = XmlReader.Create(new StringReader(sent));
                XmlReader receivedReader = XmlReader.Create(new StringReader(received));

                while (maxCpuReader.Read())
                {
                    if (maxCpuReader.NodeType == XmlNodeType.Element
                       && maxCpuReader.Name == "r")
                    {
                        Debug.WriteLine(maxCpuReader.ReadString() + " max \n");
                        maxCpu = maxCpuReader.ReadString();
                    }
                }

                while (maxMemReader.Read())
                {
                    if (maxMemReader.NodeType == XmlNodeType.Element
                       && maxMemReader.Name == "r")
                    {
                        Debug.WriteLine(maxMemReader.ReadString() + " mem \n");
                        maxCpu = maxMemReader.ReadString();
                    }
                }
            }
        }

        private void processEsmFile(HssDataSet hsData, string filePath)
        {
            throw new NotImplementedException();
        }

        private void processPlatformMeasureFile(HssDataSet hsData, String filePath)
        {
            if (File.Exists(filePath))
            {
                System.IO.StreamReader file =
                       new System.IO.StreamReader(filePath);


                List<String> lines = new List<string>();
                int systemLineNumber = 0;
                int count = 0;

                string line;
                while ((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                    if (line.Contains("PlatformMeasures=DEFAULT, Source = _SYSTEM"))
                    {
                        systemLineNumber = count;
                    }

                    count++;
                }

                String maxCpu = lines[systemLineNumber + 2];
                String maxMem = lines[systemLineNumber + 6];

                XmlReader maxCpuReader = XmlReader.Create(new StringReader(maxCpu));
                XmlReader maxMemReader = XmlReader.Create(new StringReader(maxMem));

                while (maxCpuReader.Read())
                {
                    if (maxCpuReader.NodeType == XmlNodeType.Element
                       && maxCpuReader.Name == "r")
                    {
                        Debug.WriteLine(maxCpuReader.ReadString() + " max \n");
                        hsData.maxCpuLoad = maxCpuReader.ReadString();
                    }
                }

                while (maxMemReader.Read())
                {
                    if (maxMemReader.NodeType == XmlNodeType.Element
                       && maxMemReader.Name == "r")
                    {
                        Debug.WriteLine(maxMemReader.ReadString() + " mem \n");
                        hsData.MaxMemUsage = maxMemReader.ReadString();
                    }
                }
            }
        }
    }
}
