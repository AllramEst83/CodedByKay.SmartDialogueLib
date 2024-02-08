namespace CodedByKay.SmartDialogueLib.Models
{
    public class ChatMessage
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public string Message { get; set; } = string.Empty;
        public MessageType MessageType { get; set; } = MessageType.Unknown;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
