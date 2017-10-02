using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Stock.Sync.Domain.Execution
{
    public class InputLinesFromFileReader
    {
        ILogger _logger = new ConsoleLogger();
        int _lastArrivalTimestamp = Int32.MinValue;

        public IEnumerable<InputLine> ReadLines()
        {
            var aggregatedLines = new List<InputLine>();

            foreach (var line in File.ReadLines(@"inputFile.json"))
            {
                InputLine input = JsonConvert.DeserializeObject<InputLine>(line);

                if (MandatoryParametersArePresent(input.type, line) && !IsLateArrival(input))
                {
                    aggregatedLines.Add(input);
                    _lastArrivalTimestamp = input.timestamp;
                }
                else
                {
                    if (IsLateArrival(input))
                    {
                        _logger.LogMessage($"Ignoring line as it is a late arrival: {line}");
                    }
                    else
                    {
                        _logger.LogMessage($"Ignoring line because type is not known or at least one parameter is missing: {line}");
                    }
                }
            }

            return aggregatedLines;
        }

        // Definitively it is very arguable to put this here or not. 
        //the class is only supposed to have knowledge and information about how to read rows of a file.
        // But I add extra responsibility which belongs to the domain here. My reasons are:
        // 1. Filter as early as possible to avoid useless processing.
        // 2. Performance of the removal, as any other solution would require either to iterate the whole list at least once to remove late
        // arrivals or else to keep track of the timestamp management in the Engine.cs, which I think is already too complex due to nesting.
        private bool IsLateArrival(InputLine line)
        {
            if (_lastArrivalTimestamp < line.timestamp)
            {
                return false;
            }
            else
            {
                return true;
            }
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