using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using static System.String;

namespace Domains.WellModel
{
    // A LAS Log has a header describing metadata and data layout and meaning.
    public class LogHeader
    {
        public List<LogHeaderSegment> Segments { get; set; }

        public LogHeader()
        {
            Segments = new List<LogHeaderSegment>();
        }
        public LogHeader(List<LogHeaderSegment> insegments)
        {
            Segments = insegments;
        }

    }

    // A LAS Log header is composed of lines each with at most four items of non-formatting information. 
    public class LogHeaderQuadruple
    {
        public string Mnemonic;
        public string Unit;
        public string Value;
        public string Name;

        public LogHeaderQuadruple(string incoming)
        {
            var dotSplit = incoming.Split(new char[] { '.' }, 2);
            var colonSplit = dotSplit[1].Split(new char[] { ':' }, 2);
            var spaceSplit = colonSplit[0].Split(new char[] { ' ' }, 2);
            var firstField = dotSplit[0].Trim();
            var secondField = spaceSplit[0].Trim();
            var thirdField = Empty;
            var fourthField = Empty;
            if (spaceSplit.Length > 1) thirdField = spaceSplit[1].Trim();
            if (colonSplit.Length > 1) fourthField = colonSplit[1].Trim();

            Mnemonic = firstField;
            Unit = secondField;
            Value = thirdField;
            Name = fourthField;
        }
    }

    // Log Headers are named and are composed of segments composed of four items
    public class LogHeaderSegment
    {
        public string Name;
        public List<LogHeaderQuadruple> Data;
        public string OtherInformation;

        public LogHeaderSegment()
        {
            Name = Empty;
            Data = new List<LogHeaderQuadruple>();
            OtherInformation = Empty;
        }

        public LogHeaderSegment(string inString, bool other)
        {
            Name = Empty;
            Data = new List<LogHeaderQuadruple>();
            OtherInformation = Empty;


            if (IsNullOrEmpty(inString))
            {
                throw new Exception("LogHeaderSegment: No Input string.");
            }

            string[] lines = Regex.Split(inString, "\r\n|\r|\n");
            Name = lines[0];

            if (other)
            {
                OtherInformation = inString;
                return;
            }

            for (var i = 1; i < lines.Length; i++)
            {
                var line = lines[i];

                if (!IsNullOrEmpty(line) && (line[0] != '#'))
                {
                    Data.Add(new LogHeaderQuadruple(line));
                }
            }
        }
    }

}