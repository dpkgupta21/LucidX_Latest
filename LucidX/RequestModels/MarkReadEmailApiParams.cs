using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class MarkReadEmailApiParams
    {

        public string mailId { get; set; }
        public bool read { get; set; }
        public string connectionName { get; set; }
    }
}
