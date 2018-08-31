using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class EntityCodeAPIParams
    {
        public int userId { get; set; }
        public string compCode { get; set; }
        public string connectionName { get; set; }
    }
}
