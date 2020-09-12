using System.Collections.Generic;

namespace BlazorServerLASViewer.Domains.Well
{
    public class MainViewModel
    {
        public int ContainerX { get; set; } = 0;
        public int ContainerY { get; set; } = 0;
        public int ElementX { get; set; } = 0;
        public int ElementY { get; set; } = 0;
        public int ContainerMouseX { get; set; } = 0;
        public int ContainerMouseY { get; set; } = 0;
        public bool OverChild { get; set; } = false;
        public int SelectedItem { get; set; } = -1;

        public Well Well { get; set; }
        public List<Log<double>> Logs { get; set; }
    }
}