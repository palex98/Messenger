namespace Messenger.Models.Custom
{
    public class ChatDataToView
    {
        public int chatId { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public string partnerFullName { get; set; }
        public string textOfLastMsg { get; set; }
    }
}