using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Stock.Sync.Domain.Execution
{
    public class InputLinesFromFileReader
    {
        public IEnumerable<InputLine> ReadLines()
        {
            var aggregatedLines = new List<InputLine>();

            foreach (var line in File.ReadLines(@"inputFile.json"))
            {
                InputLine input = JsonConvert.DeserializeObject<InputLine>(line);
                aggregatedLines.Add(input);
            }

            return aggregatedLines;
        }
    }
}