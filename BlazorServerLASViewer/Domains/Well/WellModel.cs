using System.Collections.Generic;

namespace BlazorServerLASViewer.Domains.Well
{
    public class WellModel
    {
        public string ReturnedValue;
        public WellHeader Header;
        public List<Log<double>> Logs;

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