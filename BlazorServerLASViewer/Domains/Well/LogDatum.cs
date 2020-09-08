namespace BlazorServerLASViewer.Domains.Well
{
    public class LogDatum<T>
    {
        public double Depth { get; set; }
        public string DepthUnits { get; set; }
        public string DatumUnits { get; set; }

        public T Datum { get; set; }
    }
}