using System.Xml.Serialization;

namespace LucidX.RequestModels
{

    [XmlRoot("ElucidateAPIParams")]
    public class LoginAPIParams
    {
        [XmlElement]
        public string userID { get; set; }
        [XmlElement]
        public string strPwd { get; set; }
        [XmlElement]
        public string connectionName { get; set; }
    }
}
