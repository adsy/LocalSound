using localsound.CoreUpdates.Dto.Enum;

namespace localsound.CoreUpdates.Dto.QueueMessage
{
    public class ServiceBusMessageDto<T>
    {
        public DeleteEntityTypeEnum Command { get; set; }
        public T Data { get; set; }
    }
}
