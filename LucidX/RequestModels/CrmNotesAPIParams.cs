using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class CrmNotesAPIParams
    {
        public string entityCode { get; set; }
        public string accountCode { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string connectionName { get; set; }
    }
}
