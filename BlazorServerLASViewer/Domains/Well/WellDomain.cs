using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using static System.String;
using BlazorInputFile;
using System.Threading.Tasks;
using System;

namespace Domains.WellModel
{

    // A well log internal representation holding metadata in the header member
    // and the log data in the data member.
    public class Well
    {
        public WellHeader Header { set; get; }
        public LogData Data { set; get; }
        public string JsonHolder { set; get; }

        public Well()
        {
        }
        public Well(WellHeader header, LogData data)
        {
            Header = header;
            Data = data;
            JsonHolder = "";
        }

        // Load and parse a LAS file, which only contains numerical well logs.
        // Go the quick and dirty suck it all into memory and string split approach.
        // No error handling.
        // Untested for LAS 3.x files or for contractor specific non-standard variations.

        static public async Task<Well> GetWellFromFileListEntryAsync(IFileListEntry file)
        {
            string data;

            using (var sr = new StreamReader(file.Data))
            {
                var incomingData = new StringBuilder();
                string line;
                while ((line = await sr.ReadLineAsync()) != null)
                {
                    incomingData.Append(line + Environment.NewLine);
                }
                data = incomingData.ToString();
            }
            // The segments of a LAS file are separated by a tilde.
            // Each type of segment has a label.
            List<string> segments = new List<string>(data.Split('~').Where(seg => !IsNullOrEmpty(seg)));

            var headerSegments = new List<LogHeaderSegment>();
            int numberOfLogs = 0;
            
            foreach (var marker in new List<char> { 'O', 'V', 'P', 'W', 'C' }) {
                var segment = segments.SingleOrDefault(segment => segment[0] == marker);
                if (segment != null)
                {
                    if (marker == 'C')
                    {
                        var curveHeaderSegment = new LogHeaderSegment(segment, false);
                        headerSegments.Add(curveHeaderSegment);
                        numberOfLogs = curveHeaderSegment.Data.Count;
                    }
                    else
                    {
                        headerSegments.Add(new LogHeaderSegment(segment, marker == 'O'));
                    }
                }
            }

            var rv = new Well
            {
                Header = new WellHeader(file.Name),
                Data = new LogData(
                    numberOfLogs,
                    segments.Single(segment => segment[0] == 'C'),
                    segments.Single(segment => segment[0] == 'A')
                ),
                JsonHolder = ""
            };

            return rv;
        }
    }

}