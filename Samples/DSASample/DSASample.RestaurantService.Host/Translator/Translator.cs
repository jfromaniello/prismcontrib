using RestaurantService.Host.DataSet;
using RestaurantService.Host.DataContracts;

namespace RestaurantService.Host.Translator
{
    public static class DataTranslator
    {
        public static MenuItem TranslateFromMenuItemRowToMenuItemEntity(RestaurantDataSet.MenuItemRow menuItemRow)
        {
            MenuItem item = new MenuItem();

            item.Identifier = menuItemRow.Identifier;
            item.Name = menuItemRow.IsNameNull() ? null : menuItemRow.Name;
            item.Description = menuItemRow.IsDescriptionNull() ? null : menuItemRow.Description;
            item.Number = menuItemRow.IsNumberNull() ? null : menuItemRow.Number;
            item.Price = menuItemRow.IsPriceNull() ? 0 : menuItemRow.Price;
            item.Quantity = menuItemRow.IsQuantityNull() ? 0 : menuItemRow.Quantity;
            item.PreparationTime = menuItemRow.IsPreparationTimeNull() ? 0 : menuItemRow.PreparationTime;
            item.ImageLocation = menuItemRow.IsImageLocationNull() ? null : menuItemRow.ImageLocation;

            return item;
        }

        public static Restaurant TranslateFromRestaurantRowToRestaurantEntity(RestaurantDataSet.RestaurantRow restaurantRow)
        {
            Restaurant restaurant = new Restaurant();

            restaurant.Identifier = restaurantRow.Identifier;
            restaurant.Name = restaurantRow.IsNameNull() ? null : restaurantRow.Name;
            restaurant.Description = restaurantRow.IsDescriptionNull() ? null : restaurantRow.Description;
            restaurant.ImageLocation = restaurantRow.IsImageLocationNull() ? null : restaurantRow.ImageLocation;

            return restaurant;
        }
    }
}
