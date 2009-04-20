using System.Collections.Generic;

namespace Commanding.Modules.Order.Services
{
    public interface IOrdersRepository
    {
        IEnumerable<Order> GetOrdersToEdit();
    }
}