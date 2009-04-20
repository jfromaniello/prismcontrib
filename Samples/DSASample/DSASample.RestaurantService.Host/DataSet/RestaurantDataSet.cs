using System.IO;
using System.Reflection;

namespace RestaurantService.Host.DataSet
{   
    public partial class RestaurantDataSet
    {
        public static RestaurantDataSet LoadData()
        {
            RestaurantDataSet ds = new RestaurantDataSet();

            Assembly currentAssembly = Assembly.GetExecutingAssembly();
            using (Stream dataStream = currentAssembly.GetManifestResourceStream("RestaurantService.Host.DataSet.RestaurantData.xml"))
            {
                ds.ReadXml(dataStream);
            }
            return ds;
        }
    }
}
