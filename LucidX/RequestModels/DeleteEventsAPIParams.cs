using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class DeleteEventsAPIParams
    {
       
        public string entryId { get; set; }
        public string connectionName { get; set; }
       

    }
}
