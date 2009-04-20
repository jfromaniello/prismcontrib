using System.Collections.ObjectModel;
using Commanding.Modules.Order.Services;
using Microsoft.Practices.Composite.Events;

namespace Commanding.Modules.Order.PresentationModels
{
    public class OrdersEditorPresentationModel
    {
        private readonly OrdersCommandProxy commandProxy;
        private readonly IOrdersRepository ordersRepository;

        public ObservableCollection<OrderPresentationModel> Orders { get; private set; }

        public OrdersEditorPresentationModel(OrdersCommandProxy commandProxy, IOrdersRepository ordersRepository)
        {
            this.commandProxy = commandProxy;
            this.ordersRepository = ordersRepository;
            Orders = new ObservableCollection<OrderPresentationModel>();

            PopulateOrders();
        }

        private void PopulateOrders()
        {
            foreach (Services.Order order in ordersRepository.GetOrdersToEdit())
            {
                var orderPresentationModel = new OrderPresentationModel()
                                                 {
                                                     OrderName = order.Name,
                                                     DeliveryDate = order.DeliveryDate
                                                 };
                orderPresentationModel.Saved += OrderSaved;
                commandProxy.SaveAllOrdersCommands.RegisterCommand(orderPresentationModel.SaveOrderCommand);
                Orders.Add(orderPresentationModel);
            }
        }

        private void OrderSaved(object sender, DataEventArgs<OrderPresentationModel> e)
        {
            if (e != null && e.Value != null)
            {
                OrderPresentationModel order = e.Value;
                if (Orders.Contains(order))
                {
                    order.Saved -= OrderSaved;
                    commandProxy.SaveAllOrdersCommands.UnregisterCommand(order.SaveOrderCommand);
                    Orders.Remove(order);
                }
            }
        }
    }
}