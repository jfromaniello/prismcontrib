using System.Windows.Input;

namespace Commanding.Modules.Order.PresentationModels
{
    public class OrdersToolbarPresentationModel
    {
        public OrdersToolbarPresentationModel(OrdersCommandProxy ordersCommands)
        {
            SaveAllOrdersCommand = ordersCommands.SaveAllOrdersCommands;
        }

        public ICommand SaveAllOrdersCommand { get; private set; }
    }
}
