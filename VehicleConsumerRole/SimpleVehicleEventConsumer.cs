using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure;
using System.Data.SqlClient;

namespace VehicleConsumer1Role
{
    class SimpleVehicleEventConsumer : IEventProcessor
    {
        PartitionContext partitionContext;

        public Task OpenAsync(PartitionContext context)
        {
            Trace.TraceInformation(string.Format("SimpleVehicleEventConsumer OpenAsync.  Partition: '{0}', Offset: '{1}'", context.Lease.PartitionId, context.Lease.Offset));
            this.partitionContext = context;
            return Task.FromResult<object>(null);
        }

        public async Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            bool outputSaved = false;
            try
            {
                foreach (EventData eventData in messages)
                {
                    try
                    {
                        //var newData = this.DeserializeEventData(eventData);
                        string data = Encoding.UTF8.GetString(eventData.GetBytes());
                        var favSubscriptionData = JsonConvert.DeserializeObject<FavSubscription>(data);
                        Trace.TraceInformation("Consumed and processed by Event Processor's Consumer Worker Role"+ favSubscriptionData.Street);
                        outputSaved = SaveFavSubscriptionData(favSubscriptionData);

                    }
                    catch (Exception oops)
                    {
                        Trace.TraceError(oops.Message);
                    }
                }
                if (outputSaved)
                {
                    //Mark message as read
                    await context.CheckpointAsync();
                }



            }
            catch (Exception exp)
            {
                Trace.TraceError("Error in processing: " + exp.Message);
            }
        }

        private bool SaveFavSubscriptionData(FavSubscription favSubscriptionData)
        {
            string sqlConnectionString = CloudConfigurationManager.GetSetting("SQLConnectionString");
            try
            {
                using (var connection = new SqlConnection(sqlConnectionString))
                {
                    using (var command = connection.CreateCommand())
                    {
                        connection.Open();
                        command.CommandText =
                            "INSERT FavSubscription(DeviceId,Street" +
                            ",GPS,LastUpdated,FavHotelAvailable,FavRestaurantAvailable" +
                            ",FavZenPlaceAvailable,FavVenueHallAvailable) VALUES" +
                            "(@DeviceId,@Street,@GPS,@LastUpdated,@FavHotelAvailable,"
                            + "@FavRestaurantAvailable,@FavZenPlaceAvailable,@FavVenueHallAvailable)";

                        command.Parameters.Add("@DeviceId", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@Street", System.Data.SqlDbType.NVarChar);
                        command.Parameters.Add("@GPS", System.Data.SqlDbType.NVarChar);
                        command.Parameters.Add("@LastUpdated", System.Data.SqlDbType.DateTime);
                        command.Parameters.Add("@FavHotelAvailable", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@FavRestaurantAvailable", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@FavZenPlaceAvailable", System.Data.SqlDbType.Int);
                        command.Parameters.Add("@FavVenueHallAvailable", System.Data.SqlDbType.Int);

                        command.Parameters["@DeviceId"].Value = favSubscriptionData.DeviceId+1;
                        command.Parameters["@Street"].Value = favSubscriptionData.Street + " From Event Processor";
                        command.Parameters["@GPS"].Value = favSubscriptionData.GPS;
                        command.Parameters["@LastUpdated"].Value = favSubscriptionData.LastUpdated;
                        command.Parameters["@FavHotelAvailable"].Value = favSubscriptionData.FavHotelAvailable;
                        command.Parameters["@FavRestaurantAvailable"].Value = favSubscriptionData.FavRestaurantAvailable;
                        command.Parameters["@FavZenPlaceAvailable"].Value = favSubscriptionData.FavZenPlaceAvailable;
                        command.Parameters["@FavVenueHallAvailable"].Value = favSubscriptionData.FavVenueHallAvailable;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            Trace.TraceWarning(string.Format("SimpleVehicleEventConsumer CloseAsync.  Partition '{0}', Reason: '{1}'.", this.partitionContext.Lease.PartitionId, reason.ToString()));
            if (reason == CloseReason.Shutdown)
            {
                await context.CheckpointAsync();
            }
        }

        //MetricEvent DeserializeEventData(EventData eventData)
        //{

        //    string data = Encoding.UTF8.GetString(eventData.GetBytes());
        //    return JsonConvert.DeserializeObject<MetricEvent>(data);
        //}
    }
}
