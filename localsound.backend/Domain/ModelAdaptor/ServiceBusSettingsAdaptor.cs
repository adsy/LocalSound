namespace localsound.backend.Domain.ModelAdaptor
{
    public class ServiceBusSettingsAdaptor
    {
        public const string ServiceBusSettings = "AzureServiceBus";

        public DeleteEntityQueueSettingsAdaptor DeleteEntityQueue { get; set; }
    }
}
