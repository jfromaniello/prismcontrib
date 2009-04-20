using System.ServiceModel;
using RestaurantService.Host.DataContracts;

namespace RestaurantService.Host
{
    [ServiceContract]
    public interface IMenuService
    {
        [OperationContract]
        MenuItem[] GetMenuItems(string restaurantId);

        [OperationContract]
        Restaurant[] GetRestaurants();
    }
}
