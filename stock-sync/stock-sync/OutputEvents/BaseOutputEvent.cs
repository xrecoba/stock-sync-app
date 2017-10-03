using System.Collections.Generic;
using Stock.Sync.Domain.InputEvents;

namespace Stock.Sync.Domain.OutputEvents
{
    // output events are by definition end of the road.
    public abstract class BaseOutputEvent : IStockEvent
    {
        public abstract void Apply();

        public IEnumerable<IStockEvent> GetSyncRules()
        {
            yield break;
        }

        public IEnumerable<BaseOutputEvent> GetOutputEvents()
        {
            yield break;
        }

        internal abstract string ToJson();        
    }
}