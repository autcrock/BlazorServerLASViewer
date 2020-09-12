namespace BlazorServerLASViewer.Domains.Well
{
    public class Log<T>
    {
        public LogHeaderQuadruple Header { get; set; }
        public LogDatum<T>[] Data { get; set; }
        public Rectangle SvgViewRectangle { get; set; }

        public double MaxDepth { get; set; }
        public double MinDepth { get; set; }
        public double MaxDatum { get; set; }
        public double MinDatum { get; set; }
    }
}