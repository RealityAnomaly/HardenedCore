using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DRXNextGeneration.Common
{
    public class DrxCommand
    {
        public string Command;
        public IDictionary<string, dynamic> Parameters = new Dictionary<string, dynamic>();
        public DrxCommandSubject Subject;

        public enum DrxCommandSubject
        {
            StoreService,
            Store,
            StoreDocuments,
            StoreFlags,
            Document,
            Flag,
        }
    }
}
