using System;

namespace LucidX.ResponseModels
{
    public class CrmNotesResponse
    {
        public string NotesId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string NotesSubject { get; set; }
        public string NotesDetail { get; set; }
        public string CreatedBy { get; set; }
        public string UserName { get; set; }
        public string ImageType { get; set; }
        public string ActionTypeId { get; set; }
        public string SendTo { get; set; }
        public string PrivacyId { get; set; }
        public string CreatedBy1 { get; set; }

    }
}
