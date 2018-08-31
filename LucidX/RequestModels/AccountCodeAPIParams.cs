using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class AccountCodeAPIParams
    {
        public string connectionName { get; set; }
        public string compCode { get; set; }
    }
}
