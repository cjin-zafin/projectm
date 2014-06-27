using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace metro
{
  public class LoadData
  {
      private String cpuLoad;
      private String spxCPULoad;
      private String saxOverFlowCount;
      private String c7sl1;

      public LoadData(String cpu, String spx, String sax, String c7sl1)
      {
          this.cpuLoad = cpu;
          this.spxCPULoad = spx;
          this.saxOverFlowCount = sax;
          this.c7sl1 = c7sl1;
      }

      public String CpuLoad
      {
          get { return cpuLoad; }
          set { cpuLoad = value; }
      }

      public String SpxCPULoad
      {
          get { return spxCPULoad; }
          set { spxCPULoad = value; }
      }

      public String SaxOverFlowCount
      {
          get { return saxOverFlowCount; }
          set { saxOverFlowCount = value; }
      }

      public String C7sl1
      {
          get { return C7sl1; }
          set { C7sl1 = value; }
      }
  }
}
