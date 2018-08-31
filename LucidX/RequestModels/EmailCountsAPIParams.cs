using System.Xml.Serialization;

namespace LucidX.RequestModels
{
    [XmlRoot("ElucidateAPIParams")]
    public class EmailCountsAPIParams
    {

        public string intUserID { get; set; }
        public string connectionName { get; set; }
    }
}
