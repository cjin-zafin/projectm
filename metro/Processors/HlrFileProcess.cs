using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace metro.Processors
{
    public class SAEOverflow
    {
        public String saeMachineName;
        public String overflowNumber;
    }

    public class C7SL1
    {
        public String c7Name;
        public String flippingNumber;
    }

    public class LastAuth
    {
        public String threeDRequestCount;
        public String threeDGenerated;
        public String fiveDRequestCount;
        public String fiveDGenerated;
    }

    public class LoadDataSet
    {
        public String cpuLoad;
        public String spxCpuLoad;
        public List<SAEOverflow> saeOverflowList = new List<SAEOverflow>();
        public List<C7SL1> c7sl1List = new List<C7SL1>();
        public LastAuth lastAuth = new LastAuth();
    }

    class HlrFileProcess
    {
        private LoadDataSet loadedData = new LoadDataSet();

        public LoadDataSet processHLRLogFile(String filePath)
        {
            System.IO.StreamReader file =
                    new System.IO.StreamReader(filePath);

            List<string> fileLines = new List<string>();
            string line;

            int count = 0;
            List<int> accLoadLineNumbers = new List<int>();
            List<int> saeLineNumbers = new List<int>();
            List<int> c7LineNumbers = new List<int>();
            List<int> authenLineNumbers = new List<int>();
            while ((line = file.ReadLine()) != null)
            {
                fileLines.Add(line);
                if (line.Contains("SAE"))
                {
                    saeLineNumbers.Add(count);
                }
                if (line.Contains("ACCLOAD"))
                {
                    accLoadLineNumbers.Add(count);
                }
                if (line.Contains("C7SL1"))
                {
                    c7LineNumbers.Add(count);
                }
                if (line.Contains("AUTHEN"))
                {
                    authenLineNumbers.Add(count);
                }
                count++;
            }

            file.Close();

            String cpuLoad = processCPULoad(fileLines, accLoadLineNumbers);
            String spxCpuLoad = processSPXLoad(fileLines, accLoadLineNumbers);
            List<SAEOverflow> saeList = processSaeList(fileLines, saeLineNumbers);
            List<C7SL1> c7List = processC7List(fileLines, c7LineNumbers);
            LastAuth lastAuth = processLastAuth(fileLines, authenLineNumbers);

            loadedData.c7sl1List = c7List;
            loadedData.saeOverflowList = saeList;
            loadedData.cpuLoad = cpuLoad;
            loadedData.spxCpuLoad = spxCpuLoad;
            loadedData.lastAuth = lastAuth;


            return this.loadedData;
        }

        private LastAuth processLastAuth(List<string> fileLines, List<int> authenLineNumbers)
        {
            LastAuth retVal = new LastAuth();

            int lastAuth = authenLineNumbers[authenLineNumbers.Count - 1];

            String line7Data = fileLines[lastAuth + 9];
            string[] line7Set = line7Data.Split(null);
            String line7 = getAccStyleData(line7Set);
            retVal.threeDRequestCount = line7;

            String line13Data = fileLines[lastAuth + 15];
            string[] line13Set = line13Data.Split(null);
            String line13 = getAccStyleData(line13Set);
            retVal.threeDGenerated = line13;

            String line11Data = fileLines[lastAuth + 13];
            string[] line11Set = line11Data.Split(null);
            String line11 = getAccStyleData(line11Set);
            retVal.fiveDGenerated = line11;

            String line14Data = fileLines[lastAuth + 16];
            string[] line14Set = line14Data.Split(null);
            String line14 = getAccStyleData(line14Set);
            retVal.fiveDRequestCount = line14;

            return retVal;
        }

        private List<C7SL1> processC7List(List<string> fileLines, List<int> c7LineNumbers)
        {
            List<C7SL1> retVal = new List<C7SL1>();
            for (int i = 1; i < c7LineNumbers.Count; i++)
            {
                string saeLine = fileLines[c7LineNumbers[i]];
                string[] saeData = saeLine.Split(null);

                string c7MachineName = "";
                if (saeData.Count() > 0)
                {
                    c7MachineName = saeData[saeData.Count() - 1];
                }

                string hoverLine = fileLines[c7LineNumbers[i] + 8];
                string[] hoverData = hoverLine.Split(null);
                String hover = getAccStyleData(hoverData);

                int hoverInt = int.Parse(hover);

                if (hoverInt != 0)
                {
                    C7SL1 c7Error = new C7SL1();
                    c7Error.c7Name = c7MachineName;
                    c7Error.flippingNumber = hover;

                    retVal.Add(c7Error);
                }
            }

            return retVal;
        }

        private List<SAEOverflow> processSaeList(List<string> fileLines, List<int> saeLineNumbers)
        {
            List<SAEOverflow> retVal = new List<SAEOverflow>();
            for (int i = 1; i < saeLineNumbers.Count; i++)
            {
                string saeLine = fileLines[saeLineNumbers[i]];
                string[] saeData = saeLine.Split(null);

                string saeMachineName = "";
                if (saeData.Count() > 0)
                {
                    saeMachineName = saeData[saeData.Count() - 1];
                }

                string overflowLine = fileLines[saeLineNumbers[i] + 7];
                string[] overflowData = overflowLine.Split(null);
                String overflow = getAccStyleData(overflowData);

                int overflowInt = int.Parse(overflow);

                if (overflowInt != 0)
                {
                    SAEOverflow saeError = new SAEOverflow();
                    saeError.saeMachineName = saeMachineName;
                    saeError.overflowNumber = overflow;

                    retVal.Add(saeError);
                }
            }

            return retVal;
        }

        private string processSPXLoad(List<string> fileLines, List<int> accLoadLineNumbers)
        {
            String retVal = "";

            int lastAcc = accLoadLineNumbers[accLoadLineNumbers.Count - 2];

            String accLine = fileLines[lastAcc];
            string[] accData = accLine.Split(null);
            String accload = getAccStyleData(accData);

            String scanLine = fileLines[lastAcc + 1];
            string[] scanData = scanLine.Split(null);
            String scan = getAccStyleData(scanData);

            double accInt = double.Parse(accload);
            double scanInt = double.Parse(scan);

            double percentage = scanInt / accInt;
            retVal = percentage.ToString("#0.##%");

            Debug.WriteLine("the result of SPX iiiiiis            " + retVal);

            return retVal;
        }

        private String processCPULoad(List<string> fileLines, List<int> accLoadLineNumbers)
        {
            String retVal = "";

            int lastAcc = accLoadLineNumbers[accLoadLineNumbers.Count - 1];

            String accLine = fileLines[lastAcc];
            string[] accData = accLine.Split(null);
            String accload = getAccStyleData(accData);

            String scanLine = fileLines[lastAcc + 1];
            string[] scanData = scanLine.Split(null);
            String scan = getAccStyleData(scanData);

            double accInt = double.Parse(accload);
            double scanInt = double.Parse(scan);

            double percentage = scanInt / accInt;
            retVal = percentage.ToString("#0.##%");

            Debug.WriteLine("the result iiiiiis            " + retVal);

            return retVal;
        }

        private string getAccStyleData(string[] accData)
        {
            String retVal = "N/A";

            int dataCount = 0;
            foreach (string acc in accData)
            {
                if (!String.IsNullOrEmpty(acc) && !String.IsNullOrWhiteSpace(acc))
                {
                    dataCount++;
                    if (dataCount == 2)
                    {
                        retVal = acc;
                        break;
                    }
                }
            }

            return retVal;
        }
    }
}
