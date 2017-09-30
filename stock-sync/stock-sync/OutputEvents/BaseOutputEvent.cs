using System.Collections.Generic;
using Stock.Sync.Domain.InputEvents;

namespace Stock.Sync.Domain.OutputEvents
{
    // output events are by definition end of the road.
    internal class BaseOutputEvent
    {
        public IEnumerable<IStockEvent> GetSyncRules()
        {
            yield break;
        }

        public IEnumerable<IStockEvent> GetOutputEvents()
        {
            yield break;
        }
    }
}