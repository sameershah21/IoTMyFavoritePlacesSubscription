using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.ServiceBus.Messaging;
using Microsoft.Azure;

namespace VehicleConsumer1Role
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        private EventProcessorHost eventProcessorHost;
        private string eventHubConnectionString = string.Empty;
        private string eventHubName = string.Empty;
        private string consumerGroupName = string.Empty;
        private string storageAccountName = string.Empty;
        private string storageAccountKey = string.Empty;
        private string storageConnectionString = string.Empty;
        private string eventProcessorHostName = string.Empty;


        public override void Run()
        {
            Trace.TraceInformation("VehicleConsumer1Role is running");

           // try
            {
                this.RunAsync(this.cancellationTokenSource.Token).Wait();
                this.runCompleteEvent.WaitOne();
            }
            //finally
            //{
            //    this.runCompleteEvent.Set();
            //}
        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
            ServicePointManager.DefaultConnectionLimit = 12;

            eventHubConnectionString = CloudConfigurationManager.GetSetting("EventHubConnectionString");
            eventHubName = CloudConfigurationManager.GetSetting("EventHubName");
            consumerGroupName = CloudConfigurationManager.GetSetting("ConsumerGroupName");
            storageAccountKey = CloudConfigurationManager.GetSetting("AzureStorageAccountKey");
            storageAccountName = CloudConfigurationManager.GetSetting("AzureStorageAccountName");
            storageConnectionString = string.Format("DefaultEndpointsProtocol=https;AccountName={0};AccountKey={1}", storageAccountName, storageAccountKey);
            eventProcessorHostName = RoleEnvironment.CurrentRoleInstance.Id;


            eventProcessorHost = new EventProcessorHost(eventProcessorHostName, eventHubName, consumerGroupName, eventHubConnectionString, storageConnectionString);
            

            bool result = base.OnStart();

            Trace.TraceInformation("VehicleConsumer1Role has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("VehicleConsumer1Role is stopping");

            //this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.Set();
            eventProcessorHost.UnregisterEventProcessorAsync().Wait();


            base.OnStop();

            Trace.TraceInformation("VehicleConsumer1Role has stopped");
        }

        private async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following with your own logic.
            while (!cancellationToken.IsCancellationRequested)
            {
                Trace.TraceInformation("Working");
                await Task.Delay(1000);
            }
        }
    }
}


