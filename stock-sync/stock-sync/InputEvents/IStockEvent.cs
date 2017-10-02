using System.Collections.Generic;
using Stock.Sync.Domain.OutputEvents;

namespace Stock.Sync.Domain.InputEvents
{
    interface IStockEvent
    {
        void Apply();
        
        IEnumerable<IStockEvent> GetSyncRules();

        IEnumerable<BaseOutputEvent> GetOutputEvents();
    }
}