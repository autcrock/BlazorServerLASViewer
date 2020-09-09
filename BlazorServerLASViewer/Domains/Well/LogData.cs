using System;
using System.Linq;
using System.Text.RegularExpressions;
using static System.String;

namespace BlazorServerLASViewer.Domains.Well
{
    // A LAS Log has a data section containing well log data in arbitrary quantity,
    // set out as described in the log header.  Each log can be string or doubles represented as strings.
    // This implementation assumes no string logs for the time being.
    public class LogData
    {
        public int LogCount { get; set; }
        public int SampleCount { get; set; }

        public LogHeaderQuadruple[] Headers { get; set; }

        public LogDatum<double> [][] Logs { get; set; }

        public LogData(int logCount, string logHeadersString, string logValuesString)
        {
            if (IsNullOrEmpty(logValuesString))
            {
                return;
            }

            // LOG HEADER EXTRACTION

            // Remove the first line containing the ~ASCII identifier
            var index = logHeadersString.IndexOf(Environment.NewLine, StringComparison.Ordinal);
            logHeadersString = logHeadersString.Substring(index + Environment.NewLine.Length).Trim();
            var logHeaderSegments = logHeadersString
                .Split(Environment.NewLine)
                .Where(line => line != null && line[0] != '#')
                .ToArray();
            Headers = logHeaderSegments.Select(segment => new LogHeaderQuadruple(segment)).ToArray();


            // LOG VALUE EXTRACTION

            // Remove the first line containing the ~ASCII identifier
            index = logValuesString.IndexOf(Environment.NewLine, StringComparison.Ordinal);
            logValuesString = logValuesString.Substring(index + Environment.NewLine.Length).Trim();
            
            // Split into words and convert to raw log data
            var words = Regex.Split(logValuesString, @"\s+");
            LogCount = logCount;

            SampleCount = words.Length / LogCount;
            Logs = new LogDatum<double>[LogCount][];

            var depthUnits = Headers[0].Units;
            
            for (var logIndex = 0; logIndex < LogCount; logIndex++)
            {
                Logs[logIndex] = new LogDatum<double>[SampleCount];
                var sampleUnits = Headers[logIndex].Units;

                for (var sampleIndex = 0; sampleIndex < SampleCount; sampleIndex++)
                {
                    if (logIndex == 0)
                    {
                        // Depth log in LAS format, treat is specially
                        var depthSampleIndex = LogCount * sampleIndex;
                        var logSampleIndex = depthSampleIndex;

                        var sample = Convert.ToDouble(words[logSampleIndex]);
                        var depth = Convert.ToDouble(words[depthSampleIndex]);

                        Logs[logIndex][sampleIndex] = new LogDatum<double>()
                        {
                            Datum = sample,
                            DatumUnits = sampleUnits,
                            Depth = depth,
                            DepthUnits = depthUnits
                        };
                    } else
                    {
                        var depthSampleIndex = LogCount * sampleIndex;
                        var logSampleIndex = depthSampleIndex + logIndex;

                        var sample = Convert.ToDouble(words[logSampleIndex]);
                        var depth = Convert.ToDouble(words[depthSampleIndex]);

                        Logs[logIndex][sampleIndex] = new LogDatum<double>()
                        {
                            Datum = sample,
                            DatumUnits = sampleUnits,
                            Depth = depth,
                            DepthUnits = depthUnits
                        };

                    }
                }
            }
        }
    }
}