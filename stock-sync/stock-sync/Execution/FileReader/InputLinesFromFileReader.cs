using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Stock.Sync.Domain.Execution
{
    public class InputLinesFromFileReader
    {
        ILogger _logger = new ConsoleLogger();

        public IEnumerable<InputLine> ReadLines()
        {
            var aggregatedLines = new List<InputLine>();

            foreach (var line in File.ReadLines(@"inputFile.json"))
            {
                InputLine input = JsonConvert.DeserializeObject<InputLine>(line);

                if (MandatoryParametersArePresent(input.type, line))
                {
                    aggregatedLines.Add(input);
                }
                else
                {
                    _logger.LogMessage($"Ignoring line because type is not known or at least one parameter is missing: {line}");
                }
            }

            return aggregatedLines;
        }

        private bool MandatoryParametersArePresent(string inputType, string line)
        {
            switch (inputType)
            {
                case Constants.ProductCreated:
                    return line.Contains("id") && line.Contains("stock") && line.Contains("timestamp") && line.Contains("parent_id");
                case Constants.ProductUpdated:
                    return line.Contains("id") && line.Contains("stock") && line.Contains("timestamp");
                case Constants.ProductEnded:
                    return line.Contains("id") && line.Contains("timestamp");
            }
            return false;
        }
    }
}