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
            var dotSplit = incoming.Split(new [] { '.' }, 2);
            var colonSplit = dotSplit[1].Split(new [] { ':' }, 2);
            var spaceSplit = colonSplit[0].Split(new [] { ' ' }, 2);
            var firstField = dotSplit[0].Trim();
            var secondField = spaceSplit[0].Trim();
            var thirdField = string.Empty;
            var fourthField = string.Empty;
            if (spaceSplit.Length > 1) thirdField = spaceSplit[1].Trim();
            if (colonSplit.Length > 1) fourthField = colonSplit[1].Trim();

            Mnemonic = firstField;
            Units = secondField;
            Value = thirdField;
            Name = fourthField;
        }
    }
}