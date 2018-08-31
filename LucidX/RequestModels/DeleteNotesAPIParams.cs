using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class DeleteNotesAPIParams
    {
       
        public string notesId { get; set; }
        public string connectionName { get; set; }
       

    }
}
