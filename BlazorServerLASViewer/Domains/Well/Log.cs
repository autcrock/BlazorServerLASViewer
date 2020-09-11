namespace BlazorServerLASViewer.Domains.Well
{
    public class Log<T>
    {
        public LogHeaderQuadruple Header { get; set; }
        public LogDatum<T>[] Data { get; set; }
        public Rectangle SvgViewRectangle { get; set; }
    }
}