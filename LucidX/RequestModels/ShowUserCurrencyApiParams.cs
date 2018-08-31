using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class ShowUserCurrencyAPIParams
    {
        public string connectionName { get; set; }
        public string countryCode { get; set; }
    }
}
