using LocalSound.Shared.Package.ServiceBus.Dto.Enum;

namespace LocalSound.Shared.Package.ServiceBus.Dto.QueueMessage
{
    public class QueueMessageDto
    {
        public DeleteEntityTypeEnum Command { get; set; }
        public dynamic Data { get; set; }
    }
}
