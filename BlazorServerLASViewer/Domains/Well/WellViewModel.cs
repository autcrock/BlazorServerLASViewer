using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlazorInputFile;

namespace BlazorServerLASViewer.Domains.Well
{
    public class WellViewModel
    {
        public int ContainerX { get; set; } = 0;
        public int ContainerY { get; set; } = 0;
        public int ElementX { get; set; } = 0;
        public int ElementY { get; set; } = 0;
        public int ContainerMouseX { get; set; } = 0;
        public int ContainerMouseY { get; set; } = 0;
        public bool OverChild { get; set; } = false;
        public int SelectedItem { get; set; } = -1;

        public static readonly int LogRedComponent = 255;
        public static readonly int LogGreenComponent = 255;
        public static readonly int LogBlueComponent = 255;

        public static readonly int LogBorderStroke = 3;
        public static readonly int LogOffset = LogBorderStroke;

        public static readonly int LogHeaderHeight = 100;
        public static readonly int LogHeaderLegendEntryHeight = 20;

        public static readonly int LogCurvesHeight = 600;
        
        public static readonly int LogWidth = 200;
        public static readonly int LogHeight = LogHeaderHeight + LogBorderStroke + LogCurvesHeight;

        public static readonly int WellRedComponent = 200;
        public static readonly int WellGreenComponent = 200;
        public static readonly int WellBlueComponent = 200;

        public static readonly int WellBorderWidth = 3;
        public static readonly int WellHeight = LogHeight + 2 * WellBorderWidth;

        public int WellWidth => (LogWidth + 2 * LogBorderStroke) * Logs.Count + WellBorderWidth * 2;


        public Well Well { get; set; }
        public List<Log<double>> Logs { get; set; }

        public static List<Log<T>> LogDataToLogs<T>(LogData<T> logData)
        {
            var logs = new List<Log<T>>();

            for (int i = 0; i < logData.LogCount; i++)
            {
                logs.Add(new Log<T>
                {
                    Data = logData.Logs[i],
                    Header = logData.Headers[i],
                    SvgViewRectangle = new Rectangle
                    {
                        RectangleId = i + 1,
                        X = LogOffset + i * (LogWidth + LogBorderStroke + LogOffset),
                        Y = LogOffset,
                        Width = LogWidth,
                        Height = LogHeight,
                        R = LogRedComponent,
                        G = LogGreenComponent,
                        B = LogBlueComponent
                    },
                    MaxDepth = Double.MinValue,
                    MinDepth = Double.MaxValue,
                    MaxDatum = Double.MinValue,
                    MinDatum = Double.MaxValue,
                });
            }

            return logs;
        }

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

            foreach (var marker in new List<char> { 'O', 'V', 'P', 'W', 'C' })
            {
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
    }
}