using RestaurantService.Host.DataSet;

namespace RestaurantService.Host.Translator
{
    public static class RestaurantRepository
    {
        private static readonly RestaurantDataSet _instance = RestaurantDataSet.LoadData();

        public static RestaurantDataSet Instance
        {
            get { return _instance; }
        }
    }
}
