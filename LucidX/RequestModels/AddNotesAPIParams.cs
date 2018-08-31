using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class AddNotesAPIParams
    {
        public string entityCode { get; set; }
        public string accountCode { get; set; }
        public string notesHeader { get; set; }
        public string notesDetail { get; set; }
        public string notesDetail_Add { get; set; }
        public int userId { get; set; }
        public int actionTypeId { get; set; }
        public string sendTo { get; set; }
        public int privacyId { get; set; }
        public int accountId { get; set; }
        public int notesId { get; set; }
        public string connectionName { get; set; }
       

    }
}
