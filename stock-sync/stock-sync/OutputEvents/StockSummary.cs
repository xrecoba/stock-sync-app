using System;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.OutputEvents
{
    class StockSummary : BaseOutputEvent
    {
        private readonly ProductsRepository productsRepository;

        public StockSummary(ProductsRepository productsRepository)
        {
            this.productsRepository = productsRepository;
        }

        public override void Apply()
        {
            
        }

        internal override string ToJson()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\'type\': \'StockSummary\', \'stocks\': {\'");

            var inventories = productsRepository.GetAllProducts().Select(p => $"\'{p.Id}\': {p.Stock}");
            sb.Append(String.Join(", ", inventories));

            sb.Append("}}");

            return sb.ToString();
        }
    }
}