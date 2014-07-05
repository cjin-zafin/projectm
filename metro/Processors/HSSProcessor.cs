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
            throw new NotImplementedException();
        }

        private void processEsmFile(HssDataSet hsData, string filePath)
        {
            throw new NotImplementedException();
        }

        private void processPlatformMeasureFile(HssDataSet hsData, String filePath)
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

            String sysLine = lines[systemLineNumber];

            XmlReader xReader = XmlReader.Create(new StringReader(sysLine));

            Debug.WriteLine(xReader.ReadString() + "33 \n");

            while (xReader.Read())
            {
                 if (xReader.NodeType == XmlNodeType.Element
                    && xReader.Name == "moid")
                 {
                     Debug.WriteLine(xReader.ReadString() + "11 \n");
                 }
            }
        }
    }
}
