using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class OrdersAPIParams
    {
        public string processedBy { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string connectionName { get; set; }
    }
}
