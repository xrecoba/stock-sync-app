using System.Collections.Generic;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.OutputEvents;

namespace stock.sync.SyncRules
{
    /// <summary>
    /// Syncronization event base class.
    /// Like a normal event except that it never raises a new syncronization rule.
    /// </summary>
    internal class SyncEvent
    {
        protected readonly List<BaseOutputEvent> OutputEvents;

        public SyncEvent()
        {
            OutputEvents = new List<BaseOutputEvent>();
        }

        public IEnumerable<IStockEvent> GetSyncRules()
        {
            yield break;
        }

        public IEnumerable<BaseOutputEvent> GetOutputEvents()
        {
            return OutputEvents;
        }
    }
}