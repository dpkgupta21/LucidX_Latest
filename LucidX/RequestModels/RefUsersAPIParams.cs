using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class RefUsersAPIParams
    {
        public string connectionName { get; set; }
        public string compcode { get; set; }

    }
}
