using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class AccountOrdersAPIParams
    {
        public int accountTypeID { get; set; }
        public int compCode { get; set; }
        public string connectionName { get; set; }
    }
}
