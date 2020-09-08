using System.Collections.Generic;

namespace BlazorServerLASViewer.Domains.Well
{
    // A LAS Log has a header describing metadata and data layout and meaning.
    public class LogHeader
    {
        public List<LogHeaderSegment> Segments { get; set; }

        public LogHeader()
        {
            Segments = new List<LogHeaderSegment>();
        }
        public LogHeader(List<LogHeaderSegment> inSegments)
        {
            Segments = inSegments;
        }

    }

}