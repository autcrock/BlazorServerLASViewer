using System.Collections.Generic;

namespace Domains.WellModel
{
    public class WellModel
    {
        public string ReturnedValue;
        public WellHeader Header;
        public List<Log> Logs;

        public WellModel()
        {
            ReturnedValue = "Empty WellModel";
        }
        public WellModel(string message)
        {
            ReturnedValue = message;
        }
    }


}