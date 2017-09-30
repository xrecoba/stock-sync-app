using System.Collections.Generic;
using Stock.Sync.Domain.InputEvents;

namespace stock.sync.SyncRules
{
    /// <summary>
    /// Syncronization event base class.
    /// Like a normal event except that it never raises a new syncronization rule.
    /// </summary>
    internal class SyncEvent
    {
        protected readonly List<IStockEvent> OutputEvents;

        public SyncEvent()
        {
            OutputEvents = new List<IStockEvent>();
        }

        public IEnumerable<IStockEvent> GetSyncRules()
        {
            yield break;
        }

        public IEnumerable<IStockEvent> GetOutputEvents()
        {
            return OutputEvents;
        }
    }
}