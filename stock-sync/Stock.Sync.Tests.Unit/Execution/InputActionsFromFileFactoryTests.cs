using System;
using System.Collections.Generic;
using System.Text;
using Stock.Sync.Domain.Execution;
using Stock.Sync.Domain.Repositories;
using Xunit;

namespace Stock.Sync.Tests.Unit.Execution
{
    public class InputActionsFromFileFactoryTests
    {
        [Fact]
        public void SampleWithExampleInputFile()
        {
            var lines = new InputLinesFromFileReader().ReadLines();
            var productsRepository = new ProductsRepository();
            var stockEventsFactory = new InputLinesToIStockEventsFactory(productsRepository, new ConsoleLogger());
            var stockEvents = stockEventsFactory.GetInputEvents(lines);

            var engine = new Engine();
            engine.Run(productsRepository, stockEvents);
        }
    }
}
