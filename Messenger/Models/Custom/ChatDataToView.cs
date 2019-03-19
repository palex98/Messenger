using System;

namespace Messenger.Models.Custom
{
    public class ChatDataToView
    {
        public int ChatId { get; set; }
        public int Receiver { get; set; }
        public string PartnerFullName { get; set; }
        public DateTime? LastMessage { get; set; }
        public int Unreaded { get; set; }
        public string Color { get; set; }
    }
}