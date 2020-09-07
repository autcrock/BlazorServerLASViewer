namespace Domains.WellModel
{
    public class WellHeader
    {
        public string Name { get; set; }

        WellHeader()
        {
        }

        public WellHeader(string name)
        {
            Name = name;
        }
    }
}