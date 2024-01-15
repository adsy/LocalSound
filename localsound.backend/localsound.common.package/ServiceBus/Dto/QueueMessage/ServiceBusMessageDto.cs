using LocalSound.Shared.Package.ServiceBus.Dto.Enum;

namespace LocalSound.Shared.Package.ServiceBus.Dto.QueueMessage
{
    public class ServiceBusMessageDto<T>
    {
        public DeleteEntityTypeEnum Command { get; set; }
        public T Data { get; set; }
    }
}
