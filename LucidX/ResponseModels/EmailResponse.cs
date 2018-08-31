namespace LucidX.ResponseModels
{
    public class EmailResponse
    {
        public string MailId { get; set; }
        public string DisplayDate { get; set; }
        public string myGroup { get; set; }
        public string Received { get; set; }
        public string FolderName { get; set; }
        public string AccountCode { get; set; }
        public string Sender { get; set; }
        public string SenderName { get; set; }
        public string Subject { get; set; }
        public int eMailTypeId { get; set; }
        public bool Unread { get; set; }
        public bool Important { get; set; }
        public int Attachment { get; set; }
        public string SenderEmail { get; set; }

    }
}
