using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace BlazorServerLASViewer.Domains.Well
{
    // A LAS Log header is composed of lines each with at most four items of non-formatting information. 

    // Log Headers are named and are composed of segments composed of four items
    public class LogHeaderSegment
    {
        public string Name;
        public List<LogHeaderQuadruple> Data;
        public string OtherInformation;

        public LogHeaderSegment()
        {
            Name = String.Empty;
            Data = new List<LogHeaderQuadruple>();
            OtherInformation = String.Empty;
        }

        public LogHeaderSegment(string inString, bool other)
        {
            Name = String.Empty;
            Data = new List<LogHeaderQuadruple>();
            OtherInformation = String.Empty;


            if (String.IsNullOrEmpty(inString))
            {
                throw new Exception("LogHeaderSegment: No Input string.");
            }

            var lines = Regex.Split(inString, "\r\n|\r|\n");
            Name = lines[0];

            if (other)
            {
                OtherInformation = inString;
                return;
            }

            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i];

                if (!String.IsNullOrEmpty(line) && (line[0] != '#'))
                {
                    Data.Add(new LogHeaderQuadruple(line));
                }
            }
        }
    }
}