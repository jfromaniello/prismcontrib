using System.Runtime.Serialization;

namespace RestaurantService.Host.DataContracts
{
    [DataContract]
    public class MenuItem
    {
        protected string identifier;
        protected string number;
        protected string name;
        protected string description;
        protected string imageLocation;
        protected decimal price;
        protected int quantity;
        protected int preparationTime;

        [DataMember]
        public string Identifier
        {
            get { return this.identifier; }
            set { this.identifier = value; }
        }

        [DataMember]
        public string Number
        {
            get { return this.number; }
            set { this.number = value; }
        }

        [DataMember]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [DataMember]
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        [DataMember]
        public string ImageLocation
        {
            get { return this.imageLocation; }
            set { this.imageLocation = value; }
        }

        [DataMember]
        public decimal Price
        {
            get { return this.price; }
            set { this.price = value; }
        }

        [DataMember]
        public int Quantity
        {
            get { return this.quantity; }
            set { this.quantity = value; }
        }

        [DataMember]
        public int PreparationTime
        {
            get { return this.preparationTime; }
            set { this.preparationTime = value; }
        }
    }
}
