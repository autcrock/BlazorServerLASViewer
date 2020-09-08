namespace BlazorServerLASViewer.Domains.Well
{
    public class WellHeader
    {
        public string Name { get; set; }

        public WellHeader(string name)
        {
            Name = name;
        }
    }
}