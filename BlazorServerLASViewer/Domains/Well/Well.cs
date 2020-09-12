using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorInputFile;

namespace BlazorServerLASViewer.Domains.Well
{

    // A well log internal representation holding metadata in the header member
    // and the log data in the data member.
    public class Well
    {
        public WellHeader Header { set; get; }
        public LogData<double> DoubleLogs { set; get; }

        // Load and parse a LAS file, which only contains numerical well logs.
        // Go the quick and dirty suck it all into memory and string split approach.
        // No error handling.
        // Untested for LAS 3.x files or for contractor specific non-standard variations.

        public static async Task<Well> GetWellFromFileListEntryAsync(IFileListEntry file)
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
            var segments = new List<string>(data.Split('~').Where(seg => !String.IsNullOrEmpty(seg)));

            var headerSegments = new List<LogHeaderSegment>();
            var numberOfLogs = 0;
            
            foreach (var marker in new List<char> { 'O', 'V', 'P', 'W', 'C' }) {
                var segment = segments.SingleOrDefault(s => s[0] == marker);
                
                if (segment == null) continue;
                
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

            var rv = new Well
            {
                Header = new WellHeader(file.Name),
                DoubleLogs = new LogData<double>(
                    numberOfLogs,
                    segments.Single(segment => segment[0] == 'C'),
                    segments.Single(segment => segment[0] == 'A')
                ),
            };

            return rv;
        }

        public static List<Log<T>> LogDataToLogs<T>(LogData<T> logData)
        {
            var logs = new List<Log<T>>();

            for (int i = 0; i < logData.LogCount; i++)
            {
                logs.Add( new Log<T>
                {
                    Data = logData.Logs[i],
                    Header = logData.Headers[i],
                    SvgViewRectangle = new Rectangle {
                        RectangleId = i + 1,
                        X = i*100+1,
                        Y = 1,
                        Width  = 200,
                        Height = 800,
                        R = 255,
                        G = 255,
                        B = 255
                    }
                });
            }

            return logs;
        }
    }

}