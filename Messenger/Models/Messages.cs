namespace Messenger.Models
{
    using System;
    
    public partial class Messages
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public System.DateTime DateTime { get; set; }
        public bool IsReaded { get; set; }
        public bool IsForwarded { get; set; }
        public bool IsReply { get; set; }
        public Nullable<int> ReplyTo { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        public int ChatId { get; set; }
    }
}
