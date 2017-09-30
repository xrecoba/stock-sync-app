using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Newtonsoft.Json;

namespace Stock.Sync.Domain.Execution
{
    class InputActionsFromFileFactory
    {
        public IEnumerable<InputLine> LoadJson()
        {
            var aggregatedLines = new List<InputLine>();

            foreach (var line in File.ReadLines(@"inputFile.json"))
            {
                InputLine input = JsonConvert.DeserializeObject<InputLine>(line);
                aggregatedLines.Add(input);
            }

            return aggregatedLines;
        }

        [DebuggerDisplay("{type}-{id}-{stock}-{timestamp}-{parent_id}")]
        public class InputLine
        {
            public string type;
            public int id;
            public int stock;
            public int timestamp;
            public string parent_id;
        }
    }
}
