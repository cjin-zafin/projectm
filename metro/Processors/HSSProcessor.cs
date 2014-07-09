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

            foreach(String filePath in filePathes)
            {
                if(filePath.Contains("PlatformMeasures"))
                {
                    processPlatformMeasureFile(hsData, filePath);
                }
                else if(filePath.Contains("HSS-ESM"))
                {
                    processEsmFile(hsData, filePath);
                }
                else if(filePath.Contains("SS7Statistics"))
                {
                    processSs7File(hsData, filePath);
                }
            }
            return hsData;
        }



        private void processEsmFile(HssDataSet hsData, string filePath)
        {
            String HssS6aUpdateLocationRequests = "HssS6aUpdateLocationRequests";
            String HssS6aUpdateLocationAnswersDiaSuccess = "HssS6aUpdateLocationAnswersDiaSuccess";

            String HssS6aCancelLocationRequests = "HssS6aCancelLocationRequests";
            String HssS6aCancelLocationAnswersDiaSuccess = "HssS6aCancelLocationAnswersDiaSuccess";

            String EsmMapSaiRequests = "EsmMapSaiRequests";
            String EsmMapSaiSuccessResponses = "EsmMapSaiSuccessResponses";

            String EsmExtDbModifySuccessResponses = "EsmExtDbModifySuccessResponses";
            String EsmExtDbModifyRequests = "EsmExtDbModifyRequests";

            String EsmExtDbSearchSuccessResponses = "EsmExtDbSearchSuccessResponses";
            String EsmExtDbSearchRequests = "EsmExtDbSearchRequests";

            int EsmMapSaiRequestsLine = 0;
            int EsmMapSaiSuccessResponsesLine = 0;
            int EsmExtDbModifySuccessResponsesLine = 0;
            int EsmExtDbModifyRequestsLine = 0;
            int EsmExtDbSearchSuccessResponsesLine = 0;
            int EsmExtDbSearchRequestsLine = 0;

            int HssS6aUpdateLocationRequestsLine = 0;
            int HssS6aUpdateLocationAnswersDiaSuccessLine = 0;

            int HssS6aCancelLocationRequestsLine = 0;
            int HssS6aCancelLocationAnswersDiaSuccessLine = 0;



            if(File.Exists(filePath))
            {
                System.IO.StreamReader file =
                       new System.IO.StreamReader(filePath);


                List<String> lines = new List<string>();
                int count = 0;

                string line;
                while((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                    if(line.Contains(EsmMapSaiRequests))
                    {
                        EsmMapSaiRequestsLine = count;
                    }
                    else if(line.Contains(EsmMapSaiSuccessResponses))
                    {
                        EsmMapSaiSuccessResponsesLine = count;
                    }
                    else if(line.Contains(EsmExtDbModifySuccessResponses))
                    {
                        EsmExtDbModifySuccessResponsesLine = count;
                    }
                    else if(line.Contains(EsmExtDbModifyRequests))
                    {
                        EsmExtDbModifyRequestsLine = count;
                    }
                    else if(line.Contains(EsmExtDbSearchSuccessResponses))
                    {
                        EsmExtDbSearchSuccessResponsesLine = count;
                    }
                    else if(line.Contains(EsmExtDbSearchRequests))
                    {
                        EsmExtDbSearchRequestsLine = count;
                    }
                    else if(line.Contains(HssS6aUpdateLocationRequests))
                    {
                        HssS6aUpdateLocationRequestsLine = count;
                    }
                    else if(line.Contains(HssS6aUpdateLocationAnswersDiaSuccess))
                    {
                        HssS6aUpdateLocationAnswersDiaSuccessLine = count;
                    }
                    else if(line.Contains(HssS6aCancelLocationRequests))
                    {
                        HssS6aCancelLocationRequestsLine = count;
                    }
                    else if(line.Contains(HssS6aCancelLocationAnswersDiaSuccess))
                    {
                        HssS6aCancelLocationAnswersDiaSuccessLine = count;
                    }

                    count++;
                }

                HssS6aUpdateLocationRequests = processMiData(HssS6aUpdateLocationRequestsLine, lines);
                HssS6aUpdateLocationAnswersDiaSuccess = processMiData(HssS6aUpdateLocationAnswersDiaSuccessLine, lines);
                double updateRatio = calculateRatio(HssS6aUpdateLocationAnswersDiaSuccess, HssS6aUpdateLocationRequests);

                HssS6aCancelLocationRequests = processMiData(HssS6aCancelLocationRequestsLine, lines);
                HssS6aCancelLocationAnswersDiaSuccess = processMiData(HssS6aCancelLocationAnswersDiaSuccessLine, lines);
                double cancelRatio = calculateRatio(HssS6aCancelLocationAnswersDiaSuccess, HssS6aCancelLocationRequests);

                EsmMapSaiRequests = processThridlineData(EsmMapSaiRequestsLine, lines);
                EsmMapSaiSuccessResponses = processThridlineData(EsmMapSaiSuccessResponsesLine, lines);
                double mapRatio = calculateRatio(EsmMapSaiSuccessResponses, EsmMapSaiRequests);
                EsmExtDbModifySuccessResponses = processThridlineData(EsmExtDbModifySuccessResponsesLine, lines);
                EsmExtDbModifyRequests = processThridlineData(EsmExtDbModifyRequestsLine, lines);
                double extDbRatio = calculateRatio(EsmExtDbModifySuccessResponses, EsmExtDbModifyRequests);
                EsmExtDbSearchSuccessResponses = processThridlineData(EsmExtDbSearchSuccessResponsesLine, lines);
                EsmExtDbSearchRequests = processThridlineData(EsmExtDbSearchRequestsLine, lines);
                double extDbSearchRatio = calculateRatio(EsmExtDbSearchSuccessResponses, EsmExtDbSearchRequests);

                hsData.sentAuthenticationInfo = mapRatio.ToString("0.00");
                hsData.exDbModify = extDbRatio.ToString("0.00");
                hsData.exDbSearch = extDbSearchRatio.ToString("0.00");
                hsData.updateLocation = updateRatio.ToString("0.00");
                hsData.cancelLocation = cancelRatio.ToString("0.00");
            }
        }

        private string processMiData(int HssS6aUpdateLocationRequestsLine, List<string> lines)
        {
            List<int> returnValues = new List<int>();

            String xmlString = "";
            String endLine = "</mi>";
            int endLineCount = 0;
            for(int i = HssS6aUpdateLocationRequestsLine - 3; i < lines.Count; i++)
            {
                xmlString += lines[i];

                if(lines[i].Contains(endLine))
                {
                    endLineCount = i;
                    break;
                }
            }

            XmlReader lineReader = XmlReader.Create(new StringReader(xmlString));

            while(lineReader.Read())
            {
                if(lineReader.NodeType == XmlNodeType.Element
                   && lineReader.Name == "r")
                {
                    String value = lineReader.ReadString();

                    returnValues.Add(int.Parse(value));
                }
            }

            int retVal = 0;
            foreach(int value in returnValues)
            {
                retVal += value;
            }

            return retVal.ToString();
        }

        private double calculateRatio(string EsmMapSaiSuccessResponses, string EsmMapSaiRequests)
        {
            if(EsmMapSaiRequests != null && EsmMapSaiSuccessResponses != null)
            {
                double ratio = double.Parse(EsmMapSaiSuccessResponses) / double.Parse(EsmMapSaiRequests);

                return ratio;
            }

            return 0.0;
        }

        private string processThridlineData(int nameline, List<string> lines)
        {
            String thirdLine = lines[nameline + 3];

            String thirdLineValue = null;

            XmlReader thirdLineReader = XmlReader.Create(new StringReader(thirdLine));

            while(thirdLineReader.Read())
            {
                if(thirdLineReader.NodeType == XmlNodeType.Element
                   && thirdLineReader.Name == "r")
                {
                    thirdLineValue = thirdLineReader.ReadString();
                }
            }

            return thirdLineValue;
        }

        private void processSs7File(HssDataSet hsData, string filePath)
        {
            if(File.Exists(filePath))
            {
                System.IO.StreamReader file =
                      new System.IO.StreamReader(filePath);


                List<String> lines = new List<string>();
                int sendLineNumber = 0;
                int receiveLineNumber = 0;
                int count = 0;

                string line;
                while((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                    if(line.Contains("tsp.sctp.data_chunk_received"))
                    {
                        receiveLineNumber = count;
                    }
                    else if(line.Contains("tsp.sctp.data_chunk_sent"))
                    {
                        sendLineNumber = count;
                    }

                    count++;
                }

                String sent = lines[sendLineNumber + 3];
                String received = lines[receiveLineNumber + 3];
                //sent = sent.Substring(2);
                //received = received.Substring(2);

                String sentValue = null;
                String receivedValue = null;

                XmlReader sentReader = XmlReader.Create(new StringReader(sent));
                XmlReader receivedReader = XmlReader.Create(new StringReader(received));

                while(sentReader.Read())
                {
                    if(sentReader.NodeType == XmlNodeType.Element
                       && sentReader.Name == "r")
                    {
                        //Debug.WriteLine(sentReader.ReadString() + " sent \n");
                        sentValue = sentReader.ReadString();
                    }
                }

                while(receivedReader.Read())
                {
                    if(receivedReader.NodeType == XmlNodeType.Element
                       && receivedReader.Name == "r")
                    {
                        //Debug.WriteLine(receivedReader.ReadString() + " receive \n");
                        receivedValue = receivedReader.ReadString();
                    }
                }

                if(sentValue != null && receivedValue != null)
                {
                    int ratio = int.Parse(receivedValue) / int.Parse(sentValue);
                    hsData.sctpResendRate = ratio.ToString();
                }
            }
        }

        private void processPlatformMeasureFile(HssDataSet hsData, String filePath)
        {
            if(File.Exists(filePath))
            {
                System.IO.StreamReader file =
                       new System.IO.StreamReader(filePath);


                List<String> lines = new List<string>();
                int systemLineNumber = 0;
                int count = 0;

                string line;
                while((line = file.ReadLine()) != null)
                {
                    lines.Add(line);
                    if(line.Contains("PlatformMeasures=DEFAULT, Source = _SYSTEM"))
                    {
                        systemLineNumber = count;
                    }

                    count++;
                }

                String maxCpu = lines[systemLineNumber + 2];
                String maxMem = lines[systemLineNumber + 6];

                XmlReader maxCpuReader = XmlReader.Create(new StringReader(maxCpu));
                XmlReader maxMemReader = XmlReader.Create(new StringReader(maxMem));

                while(maxCpuReader.Read())
                {
                    if(maxCpuReader.NodeType == XmlNodeType.Element
                       && maxCpuReader.Name == "r")
                    {
                        //Debug.WriteLine(maxCpuReader.ReadString() + " max \n");
                        hsData.maxCpuLoad = maxCpuReader.ReadString();
                    }
                }

                while(maxMemReader.Read())
                {
                    if(maxMemReader.NodeType == XmlNodeType.Element
                       && maxMemReader.Name == "r")
                    {
                        //Debug.WriteLine(maxMemReader.ReadString() + " mem \n");
                        hsData.MaxMemUsage = maxMemReader.ReadString();
                    }
                }
            }
        }
    }
}
