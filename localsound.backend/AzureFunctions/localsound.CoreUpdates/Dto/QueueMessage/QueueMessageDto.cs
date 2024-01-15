using localsound.CoreUpdates.Dto.Enum;

namespace localsound.CoreUpdates.Dto.QueueMessage
{
    public class QueueMessageDto
    {
        public DeleteEntityTypeEnum Command { get; set; }
        public dynamic Data { get; set; }
    }
}
