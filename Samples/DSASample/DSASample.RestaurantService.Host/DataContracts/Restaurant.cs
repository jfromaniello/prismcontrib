using System.Runtime.Serialization;

namespace RestaurantService.Host.DataContracts
{
    [DataContract]
    public class Restaurant
    {
        protected string identifier;
        protected string name;
        protected string description;
        protected string imageLocation;

        [DataMember]
        public string Identifier
        {
            get { return this.identifier; }
            set { this.identifier = value; }
        }

        [DataMember]
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        [DataMember]
        public string ImageLocation
        {
            get { return this.imageLocation; }
            set { this.imageLocation = value; }
        }

        [DataMember]
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
    }
}
