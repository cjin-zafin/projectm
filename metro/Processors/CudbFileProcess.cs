using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace metro
{
    public class CudbDataSet
    {
        public String hlrUserCount;
        public String hlrUserActCount;
        public String gprsUserCount;
        public String authInfoCount;
        public String twoGAuthInfoCount;
        public String threeGAuthInfoCount;
        public String fourGUserCount;
    }


    class CudbFileProcess
    {
        public CudbDataSet processCudbLogFile(String filePath)
        {
            CudbDataSet cudbDataSet = new CudbDataSet();

            System.IO.StreamReader file =
                    new System.IO.StreamReader(filePath);

            List<string> fileLines = new List<string>();

            //int hlrUserCountLine = 0;
            //int hlrUserActCountLine = 0;
            //int gprsUserCountLine = 0;
            //int authInfoCountLine = 0;
            //int twoGAuthInfoCountLine = 0;
            //int threeGAuthInfoCountLine = 0;
            //int fourGUserCountLine = 0;

            string line;

            int count = 0;
            while ((line = file.ReadLine()) != null)
            {
                if (line.Contains("HLRSUBS; NMOBSUBSCNT"))
                {
                    cudbDataSet.hlrUserCount = filterData(line);
                }

                if (line.Contains("HLRSUBS; NACTSUBSCNT") && count == 0)
                {
                    cudbDataSet.hlrUserActCount = filterData(line);
                    count++;
                }

                if (line.Contains("NAMSUBS; BOTHGPRSCNT") )
                {
                    cudbDataSet.gprsUserCount = filterData(line);
                }

                if (line.Contains("AUCSUBS; NSUBSCNT"))
                {
                    cudbDataSet.authInfoCount = filterData(line);
                }

                if (line.Contains("AUCSUBS; NGSUBSCNT"))
                {
                    cudbDataSet.twoGAuthInfoCount = filterData(line);
                }

                if (line.Contains("AUCSUBS; NUSUBSCNT") )
                {
                    cudbDataSet.threeGAuthInfoCount = filterData(line);
                }

                if (line.Contains("HssEsmUsersStored; TotalNumberOfEpsUsersStored"))
                {
                    cudbDataSet.fourGUserCount = filterData(line);
                }

            }

            return cudbDataSet;
        }

        private string filterData(string line)
        {
            string[] chars = line.Split(';');

            if (chars.Count() == 4)
            {
                return chars[chars.Count() - 1];
            }
            else
            {
                return "N/A";
            }
        }
    }
}
