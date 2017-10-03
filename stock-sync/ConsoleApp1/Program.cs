using System;
using Stock.Sync.Domain.Execution;
using Stock.Sync.Domain.Repositories;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var errorsLogger = new FileLogger(@"errors.json");
            var outputLogger = new FileLogger(@"outputFile.json");

            var lines = new InputLinesFromFileReader(errorsLogger).ReadLines(@"inputFile.json");
            var productsRepository = new ProductsRepository();
            var stockEventsFactory = new InputLinesToIStockEventsFactory(productsRepository);
            var stockEvents = stockEventsFactory.GetInputEvents(lines);

            var engine = new Engine(errorsLogger, outputLogger);
            engine.Run(productsRepository, stockEvents);

            errorsLogger.Dispose();
            outputLogger.Dispose();
        }
    }
}