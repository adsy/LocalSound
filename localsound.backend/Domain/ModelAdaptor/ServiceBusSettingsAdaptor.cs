namespace localsound.backend.Domain.ModelAdaptor
{
    public class ServiceBusSettingsAdaptor
    {
        public const string ServiceBusSettings = "AzureServiceBus";

        public string ConnectionString { get; set; }
        public TrackQueuesSettingsAdaptor Queues { get; set; }
    }
}
