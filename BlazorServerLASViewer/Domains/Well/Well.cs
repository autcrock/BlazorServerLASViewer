namespace BlazorServerLASViewer.Domains.Well
{

    // A well log internal representation holding metadata in the header member
    // and the log data in the data member.
    public class Well
    {
        public WellHeader Header { set; get; }
        public LogData<double> DoubleLogs { set; get; }
    }

}