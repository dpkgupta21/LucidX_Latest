using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class EmailDetailsAPIParams
    {
        public string MailID { get; set; }
        public string uid { get; set; }
        public string connectionName { get; set; }
    }
}
