using System.Text.RegularExpressions;
using System;
using static System.String;
using System.Linq;

namespace Domains.WellModel
{
    // A LAS Log has a data section containing well log data in arbitrary quantity,
    // set out as described in the log header.  Each log can be string or doubles represented as strings.
    // This implementation assumes no string logs for the time being.
    public class LogData
    {
        public int LogCount { get; set; }
        public int SampleCount { get; set; }

        public LogHeaderQuadruple[] Headers { get; set; }

        public LogDoubleDatum [][] Logs { get; set; }

        public LogData()
        {
        }

        public LogData(int logCount, int sampleCount, LogDoubleDatum[][] data)
        {
            LogCount = logCount;
            SampleCount = sampleCount;
            Logs = data;
        }

        public LogData(int logCount, string logHeadersString, string logValuesString)
        {
            if (IsNullOrEmpty(logValuesString))
            {
                return;
            }

            /// LOG HEADER EXTRACTION

            // Remove the first line containing the ~ASCII identifier
            var hindex = logHeadersString.IndexOf(Environment.NewLine);
            logHeadersString = logHeadersString.Substring(hindex + Environment.NewLine.Length).Trim();
            var logHeaderSegments = logHeadersString
                .Split(Environment.NewLine)
                .Where(line => line != null && line[0] != '#')
                .ToArray();
            Headers = logHeaderSegments.Select(segment => new LogHeaderQuadruple(segment)).ToArray();


            /// LOG VALUE EXTRACTION

            // Remove the first line containing the ~ASCII identifier
            var index = logValuesString.IndexOf(Environment.NewLine);
            logValuesString = logValuesString.Substring(index + Environment.NewLine.Length).Trim();
            
            // Split into words and convert to raw log data
            var words = Regex.Split(logValuesString, @"\s+");
            LogCount = logCount;

            SampleCount = words.Length / LogCount;
            Logs = new LogDoubleDatum[LogCount][];

            var depthUnits = Headers[0].Unit;
            
            for (var logIndex = 0; logIndex < LogCount; logIndex++)
            {
                Logs[logIndex] = new LogDoubleDatum[SampleCount];
                var sampleUnits = Headers[logIndex].Unit;

                for (var sampleIndex = 0; sampleIndex < SampleCount; sampleIndex++)
                {
                    if (logIndex == 0)
                    {
                        // Depth log in LAS format, treat is specially
                        var depthSampleIndex = LogCount * sampleIndex;
                        var logSampleIndex = depthSampleIndex;

                        var sample = Convert.ToDouble(words[logSampleIndex]);
                        var depth = Convert.ToDouble(words[depthSampleIndex]);

                        Logs[logIndex][sampleIndex] = new LogDoubleDatum()
                        {
                            Datum = sample,
                            DatumUnits = depthUnits,
                            Depth = depth,
                            DepthUnits = depthUnits
                        };
                    } else
                    {
                        var depthSampleIndex = LogCount * sampleIndex;
                        var logSampleIndex = depthSampleIndex + logIndex;

                        var sample = Convert.ToDouble(words[logSampleIndex]);
                        var depth = Convert.ToDouble(words[depthSampleIndex]);

                        Logs[logIndex][sampleIndex] = new LogDoubleDatum()
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

    public class LogDatumBase
    {
        public double Depth { get; set; }
        public string DepthUnits { get; set; }
        public string DatumUnits { get; set; }
    }

    // A LogDoubleDatum attaches a depth to a double numerical value from a well log.
    // For example, it could be a resistivity value.
    public class LogDoubleDatum : LogDatumBase
    {
        public double Datum { get; set; }

        public LogDoubleDatum()
        {
        }

    }

    // A LogStringDatum is a string attached to a depth.
    // Could be a rock chip logging descriptor for example.
    public class LogStringDatum : LogDatumBase
    {
        public string Datum { get; set; }

        public LogStringDatum()
        {
        }

    }

}