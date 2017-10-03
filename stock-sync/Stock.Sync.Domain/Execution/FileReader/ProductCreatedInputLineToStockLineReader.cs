using System;
using Stock.Sync.Domain.InputEvents;
using Stock.Sync.Domain.Repositories;

namespace Stock.Sync.Domain.Execution
{
    class ProductCreatedInputLineToStockLineReader : IInputLineToStockEventAdapter
    {
        public IStockEvent GetInputEvent(ProductsRepository repository, InputLine inputLine)
        {
            int? parentId;
            if (inputLine.parent_id == "None")
            {
                parentId = null;
            }
            else if (int.TryParse(inputLine.parent_id, out var productId))
            {
                parentId = productId;
            }
            else
            {
                throw new ArgumentException("Parent type must have value 'None' for parent items or numeric");
            }

            return new ProductCreated(repository, inputLine.id, inputLine.stock, parentId);
        }
    }
}