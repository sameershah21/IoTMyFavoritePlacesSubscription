using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System.Configuration;
using VehicleConsumer1Role;
using Newtonsoft.Json;

namespace FavSubscriptionConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string connectionString = ConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
            EventHubClient client = EventHubClient.CreateFromConnectionString(connectionString, "vehiclereadings");

            FavSubscription f1= new FavSubscription();
            f1.DeviceId = 16;
            f1.Street = "Quick Detour Rd";
            f1.GPS = "918.055687, -32.554555";
            f1.LastUpdated = DateTime.UtcNow;
            f1.FavHotelAvailable = 0;
            f1.FavRestaurantAvailable = 0;
            f1.FavVenueHallAvailable = 0;
            f1.FavZenPlaceAvailable = 1;
            string serializedString = JsonConvert.SerializeObject(f1);
            client.Send(new EventData(Encoding.UTF8.GetBytes(serializedString)));

            f1 = new FavSubscription();
            f1.DeviceId = 17;
            f1.Street = "Dolata Lane";
            f1.GPS = "98.055647, 132.554555";
            f1.LastUpdated = DateTime.UtcNow;
            f1.FavHotelAvailable = 0;
            f1.FavRestaurantAvailable = 1;
            f1.FavVenueHallAvailable = 0;
            f1.FavZenPlaceAvailable = 0;
            serializedString = JsonConvert.SerializeObject(f1);
            client.Send(new EventData(Encoding.UTF8.GetBytes(serializedString)));

            f1 = new FavSubscription();
            f1.DeviceId = 66;
            f1.Street = "as dfg Dolfghjfghjfghata Lane";
            f1.GPS = "98.055647, 132.554555";
            f1.LastUpdated = DateTime.UtcNow;
            f1.FavHotelAvailable = 0;
            f1.FavRestaurantAvailable = 1;
            f1.FavVenueHallAvailable = 0;
            f1.FavZenPlaceAvailable = 0;
            serializedString = JsonConvert.SerializeObject(f1);
            client.Send(new EventData(Encoding.UTF8.GetBytes(serializedString)));

            f1 = new FavSubscription();
            f1.DeviceId = 18;
            f1.Street = "fghDolatfgha Lane";
            f1.GPS = "98.055647, 132.554555";
            f1.LastUpdated = DateTime.UtcNow;
            f1.FavHotelAvailable = 0;
            f1.FavRestaurantAvailable = 1;
            f1.FavVenueHallAvailable = 0;
            f1.FavZenPlaceAvailable = 0;
            serializedString = JsonConvert.SerializeObject(f1);
            client.Send(new EventData(Encoding.UTF8.GetBytes(serializedString)));



            Console.WriteLine("Message Sent to Event Hub. Press Enter to continue");
            Console.ReadLine();
        }
    }
}
