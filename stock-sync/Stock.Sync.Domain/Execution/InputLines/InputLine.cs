using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Stock.Sync.Domain.Execution
{
    [DebuggerDisplay("{type}-{id}-{stock}-{timestamp}-{parent_id}")]
    public class InputLine
    {
        public InputLine(string type, int id, int stock, int timestamp, string parentId)
        {
            this.type = type;
            this.id = id;
            this.stock = stock;
            this.timestamp = timestamp;
            parent_id = parentId;
        }

        public string type;
        public int id;
        public int stock;
        public int timestamp;
        public string parent_id;
    }
}
