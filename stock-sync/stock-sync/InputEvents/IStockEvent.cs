using System.Collections.Generic;

namespace Stock.Sync.Domain.InputEvents
{
    interface IStockEvent
    {
        void Apply();
        IEnumerable<IStockEvent> GetSyncRules();
    }
}