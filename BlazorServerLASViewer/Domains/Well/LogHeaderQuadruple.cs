using System;

namespace BlazorServerLASViewer.Domains.Well
{
    public class LogHeaderQuadruple
    {
        public string Mnemonic;
        public string Units;
        public string Value;
        public string Name;

        public LogHeaderQuadruple(string incoming)
        {
            var dotSplit = incoming.Split(new char[] { '.' }, 2);
            var colonSplit = dotSplit[1].Split(new char[] { ':' }, 2);
            var spaceSplit = colonSplit[0].Split(new char[] { ' ' }, 2);
            var firstField = dotSplit[0].Trim();
            var secondField = spaceSplit[0].Trim();
            var thirdField = String.Empty;
            var fourthField = String.Empty;
            if (spaceSplit.Length > 1) thirdField = spaceSplit[1].Trim();
            if (colonSplit.Length > 1) fourthField = colonSplit[1].Trim();

            Mnemonic = firstField;
            Units = secondField;
            Value = thirdField;
            Name = fourthField;
        }
    }
}