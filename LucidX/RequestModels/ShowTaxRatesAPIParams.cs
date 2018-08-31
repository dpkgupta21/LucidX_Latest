using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class ShowTaxRatesAPIParams
    {
        public string connectionName { get; set; }
        public string countryCode { get; set; }
        public string taxTypeID { get; set; }
    }
}
