using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VehicleConsumer1Role
{
    public class FavSubscription
    {
        public int DeviceId { get; set; }
        public string Street { get; set; }
        public string GPS { get; set; }
        public DateTime LastUpdated { get; set; }
        public int FavHotelAvailable { get; set; }
        public int FavRestaurantAvailable { get; set; }
        public int FavZenPlaceAvailable { get; set; }
        public int FavVenueHallAvailable { get; set; }


    }
}
